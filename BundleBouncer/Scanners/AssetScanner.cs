

/**
* BundleBouncer Asset Scanner
* 
* Copyright (c) 2022 BundleBouncer Contributors
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

//using AssetsTools.NET.Extra;
using BundleBouncer.Data;
using BundleBouncer.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BundleBouncer
{
    internal class AssetScanner
    {
        private static BundleBouncer bb;
        private static dnYara.YaraContext yaraContext;
        private static dnYara.CompiledRules rules;
        private static dnYara.CompiledRules userRules;
        private static dnYara.CustomScanner stock_rules_scanner;
        private static dnYara.CustomScanner user_rules_scanner;
        //private static AssetsManager assetsManager;

        // Used to determine which scan cache entries get nuked.
        private static Queue<byte[]> previousScans = new Queue<byte[]>();
        private static Dictionary<byte[], EScanResult> previouslyScanned = new Dictionary<byte[], EScanResult>();

        internal static void Init(BundleBouncer bb)
        {
            AssetScanner.bb = bb;
            SetupYara();
            SetupValidation();
        }

        private static void SetupValidation()
        {
            Validation.Logging.OnError_Obj = (x) => Logging.Error(x);
            Validation.Logging.OnError_Str = (x) => Logging.Error(x);
            Validation.Logging.OnError_StrObjA = (a, b) => Logging.Error(a, b);

            Validation.Logging.OnInfo_Obj = (x) => Logging.Info(x);
            Validation.Logging.OnInfo_Str = (x) => Logging.Info(x);
            Validation.Logging.OnInfo_StrObjA = (a, b) => Logging.Info(a, b);

            Validation.Logging.OnWarning_Obj = (a) => Logging.Warning(a);
            Validation.Logging.OnWarning_Str = (a) => Logging.Warning(a);
            Validation.Logging.OnWarning_StrObjA = (a, b) => Logging.Warning(a, b);
        }

        private static void SetupYara()
        {
            yaraContext = new dnYara.YaraContext();
            rules = new dnYara.CompiledRules(bb.YaraCompiledRuleset);
            Logging.Info($"YARA: Loaded {rules.RuleCount} rules, {rules.NamespacesCount} namespaces, {rules.StringsCount} strings from the global ruleset.");
            stock_rules_scanner = new dnYara.CustomScanner(rules);

            if (Directory.Exists(bb.YaraUserRulesDir))
            {
                using (var compiler = new dnYara.Compiler())
                {
                    compiler.DeclareExternalStringVariable("type", EScanObjectType.ASSETBUNDLE.ToString()); // ASSET_BUNDLE ...
                    compiler.DeclareExternalStringVariable("name", ""); // prefab_v1...
                    foreach (var rulefile in Directory.GetFiles(bb.YaraUserRulesDir, "*.yara", SearchOption.AllDirectories))
                    {
                        compiler.AddRuleFile(rulefile);
                    }
                    userRules = compiler.Compile(); // TODO: Combine somehow
                    Logging.Info($"YARA: Loaded {userRules.RuleCount} rules, {userRules.NamespacesCount} namespaces, {userRules.StringsCount} strings from your personal ruleset.");

                    // YARA scanner
                    user_rules_scanner = new dnYara.CustomScanner(userRules);
                }
            }
            else
            {
                Logging.Info($"YARA: Directory {bb.YaraUserRulesDir} does not exist; Will not load user-provided rules.");
            }
        }

        internal static bool ScanFile(string filename, string source, byte[] hash = null, string hashstr = null)
        {
            if (hash == null)
            {
                hash = IOTool.SHA256File(filename);
            }
            if (hashstr == null)
            {
                hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            }
            if (previouslyScanned.TryGetValue(hash, out EScanResult result))
            {
                Logging.Info($"{hashstr} - Scanned already, using cached result of {result}.");
                return result == EScanResult.FAILED;
            }
            var r = InternalScanResult(filename, source, hash, hashstr);
            if (previouslyScanned.ContainsKey(hash))
            {
                previousScans = new Queue<byte[]>(previousScans.Where(x => !x.Equals(hash)));
                previouslyScanned.Remove(hash);
            }
            if (previousScans.Count >= 200)
            {
                var oldhash = previousScans.Dequeue();
                previouslyScanned.Remove(oldhash);
            }
            previouslyScanned[hash] = r;
            previousScans.Enqueue(hash);
            return r == EScanResult.FAILED;
        }

        private static EScanResult InternalScanResult(string filename, string source, byte[] hash, string hashstr)
        {

            if (AvatarShitList.IsAssetBundleHashWhitelisted(hash))
            {
                Logging.Info($"Skipping whitelisted file {filename} ({hashstr}).");
                return EScanResult.PASSED;
            }

            Logging.Info($"Checking {filename} ({hashstr})...");

            // Simple hash check.  Fast, so we do this first.
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
            {
                BundleBouncer.NotifyUserOfBlockedBundle(hash, source);
                return EScanResult.FAILED;
            }

            // Check against YARA rules.  Fast-ish.
            // A few of these check the header before we try anything else.
            if (MatchesYaraRules(filename, source, hash, hashstr))
            {
                //CleanupAssets();
                BundleBouncer.NotifyUserOfBlockedBundle(hash, source);
                return EScanResult.FAILED;
            }

            if (!TryParsingBundle(filename, source, hash, hashstr))
            {
                //CleanupAssets();
                BundleBouncer.NotifyUserOfBlockedBundle(hash, source);
                return EScanResult.FAILED;
            }
            CleanupAssets();
            return EScanResult.PASSED;
        }

        private static bool TryParsingBundle(string filename, string source, byte[] hash, string hashstr)
        {
            try
            {
                var fileSize = (ulong)(new FileInfo(filename).Length);
                using (FileStream s = File.OpenRead(filename))
                {
                    using (var vbr = new ValidatingBinaryReader(s))
                    {
                        var abf = new AssetBundleFile();
                        abf.OnBlockRead = (BlockRow6 block, byte[] bytes) => {
                            var blockid = $"block[{block.index}]";
                            Logging.Info($"Scanning {blockid} with YARA...");
                            using(var ms = new MemoryStream(bytes))
                                if(MatchesYaraRules(filename, source, hash, hashstr, ms, EScanObjectType.BLOCK, blockid))
                                    throw new FailedValidation(blockid, "YARA rule(s) matched.");
                        };
                        abf.Read(vbr, fileSize);
                    }
                }
            }
            catch(FailedValidation failure)
            {
                Logging.Error(failure);
                return false;
            }
            catch (Exception e)
            {
                Logging.Error(e);
                return false;
            }
            return true;
        }
        /*
        private static bool TryLoadingBundle(string filename, string source, byte[] hash, string hashstr, out BundleFileInstance bfi)
        {
            bfi = null;
            try
            {
                // TODO QuickHeaderCheck(filename);
                assetsManager.LoadBundleFile(filename);
                return true;
            }
            catch (Exception e)
            {
                Logging.Error("Received error!");
                Logging.Error(e.ToString());
                return false;
            }
        }
        */

        private static void CleanupAssets()
        {
            //assetsManager.UnloadAll(true);
            //assetsManager.UnloadAllAssetsFiles(true);
        }

        private static bool MatchesYaraRules(string filename, string source, byte[] hash, string hashstr)
        {
            using(var fs = File.OpenRead(filename))
                return MatchesYaraRules(filename, source, hash, hashstr, fs, EScanObjectType.ASSETBUNDLE, "");
        }

        private static bool MatchesYaraRules(string filename, string source, byte[] hash, string hashstr, Stream s, EScanObjectType type, string rscname)
        {
            var matches = new List<dnYara.ScanResult>();
            var ev = new dnYara.ExternalVariables();
            var objType = Enum.GetName(typeof(EScanObjectType), type);
            ev.StringVariables["type"] = objType;
            ev.StringVariables["name"] = rscname;
            Logging.Info($"Scanning {objType} {rscname}...");
            if (rules != null)
                matches.AddRange(stock_rules_scanner.ScanStream(s, ev));
            if (userRules != null)
                matches.AddRange(user_rules_scanner.ScanStream(s, ev));
            if (matches.Count == 0)
                return false;
            foreach (var scanResult in matches)
            {
                var id = scanResult.MatchingRule.Identifier;
                BundleBouncer.NotifyUserOfBlockedBundle(hash, source + "/YARA:" + id);
                if (rscname != "")
                    filename += "/" + rscname;
                if (scanResult.Matches.Count == 1)
                {
                    Logging.Gottem($"File {filename} matched YARA rule {id}.{scanResult.Matches.First().Key}!");
                }
                else
                {
                    Logging.Gottem($"File {filename} matched YARA rule {id}:");
                    foreach (var vd in scanResult.Matches)
                    {
                        Logging.Gottem($" - Rule {vd.Key}");
                    }
                }
            }
            return true;
        }
    }
}