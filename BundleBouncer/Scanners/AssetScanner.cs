

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

using AssetsTools.NET.Extra;
using BundleBouncer.Data;
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
        private static dnYara.Scanner scanner;
        private static AssetsManager assetsManager;

        // Used to determine which scan cache entries get nuked.
        private static Queue<byte[]> previousScans = new Queue<byte[]>();
        private static Dictionary<byte[], EScanResult> previouslyScanned = new Dictionary<byte[], EScanResult>();

        internal static void Init(BundleBouncer bb)
        {
            AssetScanner.bb = bb;
            SetupYara();
            SetupAssetTools();
        }

        private static void SetupAssetTools()
        {
            assetsManager = new AssetsManager();
            // Not needed
            //assetsManager.LoadClassDatabase("path/to/classdata.tpk");
        }

        private static void SetupYara()
        {
            yaraContext = new dnYara.YaraContext();
            rules = new dnYara.CompiledRules(bb.YaraCompiledRuleset);
            Logging.Info($"YARA: Loaded {rules.RuleCount} rules, {rules.NamespacesCount} namespaces, {rules.StringsCount} strings from the global ruleset.");
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
                }
            }
            else
            {
                Logging.Info($"YARA: Directory {bb.YaraUserRulesDir} does not exist; Will not load user-provided rules.");
            }

            // YARA scanner
            scanner = new dnYara.Scanner();
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

            // Try loading from AssetTools. Slow, but needed by some Yara rules.
            if (!TryLoadingBundle(filename, source, hash, hashstr, out BundleFileInstance bfi))
            {
                CleanupAssets();
                BundleBouncer.NotifyUserOfBlockedBundle(hash, source);
                return EScanResult.FAILED;
            }
            CleanupAssets();
            return EScanResult.PASSED;
        }

        private static bool TryLoadingBundle(string filename, string source, byte[] hash, string hashstr, out BundleFileInstance bfi)
        {
            bfi = null;
            try
            {
                // TODO QuickHeaderCheck(filename);
                assetsManager.LoadBundleFile(filename);
                /*
                bfi = assetsManager.LoadBundleFile(filename, false);
                BundleHelper.UnpackInfoOnly(bfi.file);
                for(int i = 0;i<bfi.file.NumFiles;i++)
                {
                    if (!bfi.file.IsAssetsFile(i))
                    {
                        Logging.Warning($"{filename}[{i}] is not AssetFile");
                    }
                    else
                    {
                        var af = assetsManager.LoadAssetsFileFromBundle(bfi, i);
                        for(int j = 0; j < af.table.assetFileInfoCount; j++)
                        {
                            var afi = af.table.assetFileInfo[j];
                            assetsManager.GetTypeInstance(af, afi);
                        }
                    }
                    break; // >0 result in false positives.
                }
                */
                return true;
            }
            catch (Exception e)
            {
                Logging.Error("Received error!");
                Logging.Error(e.ToString());
                return false;
            }
        }

        private static void QuickHeaderCheck(string filename)
        {
            using (var s = File.OpenRead(filename))
            {
                using (var rdr = new ValidatingBinaryReader(s))
                {

                }
            }
        }

        private static void CleanupAssets()
        {
            assetsManager.UnloadAll(true);
            assetsManager.UnloadAllAssetsFiles(true);
        }

        private static bool MatchesYaraRules(string filename, string source, byte[] hash, string hashstr/*, BundleFileInstance bfi*/)
        {
            var matches = new List<dnYara.ScanResult>();
            if (rules != null)
                matches.AddRange(scanner.ScanFile(filename, rules));
            if (userRules != null)
                matches.AddRange(scanner.ScanFile(filename, userRules));
            if (matches.Count == 0)
                return false;
            foreach (var scanResult in matches)
            {
                var id = scanResult.MatchingRule.Identifier;
                BundleBouncer.NotifyUserOfBlockedBundle(IOTool.SHA256File(filename), source + "/YARA:" + id);
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