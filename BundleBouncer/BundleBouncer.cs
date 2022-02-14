/**
 * BundleBouncer Mod Stuff
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

using BundleBouncer.Data;
using BundleBouncer.Utilities;
using MelonLoader;
using Newtonsoft.Json;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VRC;
using VRC.Core;
using VRChatUtilityKit.Utilities;

namespace BundleBouncer
{
    public class BundleBouncer : MelonMod
    {
        public Config Config { get; private set; }

        /// <summary>
        /// Singleton reference.
        /// </summary>
        public static BundleBouncer Instance { get; private set; }

        /// <summary>
        /// Where to put logs and data files
        /// </summary>
        public string UserDataDir { get { return Path.Combine("UserData", "BundleBouncer"); } }
        public string LogDir { get { return Path.Combine(UserDataDir, "Logs"); } }
        public string OldShitListFile { get { return Path.Combine(UserDataDir, "Avatars.txt"); } }
        public string UserAvatarBlockListFile { get { return Path.Combine(UserDataDir, "My-Blocked-Avatars.txt"); } }
        public string UserAvatarAllowListFile { get { return Path.Combine(UserDataDir, "My-Allowed-Avatars.txt"); } }
        public string UserAssetHashAllowListFile { get { return Path.Combine(UserDataDir, "My-Blocked-Asset-Hashes.txt"); } }
        public string UserAssetHashBlockListFile { get { return Path.Combine(UserDataDir, "My-Allowed-Asset-Hashes.txt"); } }
        public string PlayerShitlistFile { get { return Path.Combine(UserDataDir, "Player-Blacklist.json"); } }
        public string YaraCompiledRuleset { get { return Path.Combine(UserDataDir, "Global-YARA-Ruleset.bin"); } }
        public string YaraUserRulesDir { get { return Path.Combine(UserDataDir, "User-YARA-Rules"); } }
        public static string ShitlistDll { get; private set; }


        internal Patches Patches { get; private set; }

        /// <summary>
        /// Previous users of exploitive assetbundles.
        /// </summary>
        public static HashSet<string> KnownSkiddies = new HashSet<string>();

        /// <summary>
        /// Current cache of known skiddies in the current scene, as VRCPlayers.
        /// </summary>
        public static HashSet<Player> DetectedSkiddies = new HashSet<Player>();

        /// <summary>
        /// userID => avID
        /// </summary>
        private static Dictionary<string, AvatarInfo> Avatars = new Dictionary<string, AvatarInfo>();
        private static Dictionary<string, UserAvatars> AvatarUsers = new Dictionary<string, UserAvatars>();

        // Red ESP pill
        static HighlightsFXStandalone shitterHighlighter;

        private SemVersion MinimumMLVersion = new SemVersion(0, 5, 3);
        
        public const string LATEST_SHITLIST_URL = "https://github.com/ScottGriffin213/BundleBouncer/releases/download/LATEST_DEFINITIONS/BundleBouncer.Shitlist.dll";
        public const string LATEST_SHITLIST_CHECKSUM = "https://github.com/ScottGriffin213/BundleBouncer/releases/download/LATEST_DEFINITIONS/BundleBouncer.Shitlist.dll.sha256sum";
        public const string LATEST_YARA_URL = "https://github.com/ScottGriffin213/BundleBouncer/releases/download/LATEST_DEFINITIONS/Global-YARA-Ruleset.bin";
        public const string LATEST_YARA_CHECKSUM = "https://github.com/ScottGriffin213/BundleBouncer/releases/download/LATEST_DEFINITIONS/Global-YARA-Ruleset.bin.sha256sum";
        public static readonly string BLOCKED_AVTR_ID = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
        public static readonly string BLOCKED_FILE_URL = "https://0.0.0.0/blocked.dat"; // FIXME

        public override void OnApplicationStart()
        {
            Logging.LI = LoggerInstance;

            // Idiot checks
            var chunks = MelonLoader.BuildInfo.Version.Split('.').Select(x=>int.Parse(x)).ToArray();
            if (new Semver.SemVersion(chunks[0], chunks[1], chunks[2]) < MinimumMLVersion)
            {
                Logging.Error($"You are using MelonLoader {MelonLoader.BuildInfo.Version}, which has problems with BundleBouncer.  Please update.");
                return;
            }

            this.Config = new Config();

            Instance = this;

            if (!Directory.Exists(UserDataDir))
            {
                Directory.CreateDirectory(UserDataDir);
                Logging.Info($"Created {UserDataDir}");
            }

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
                Logging.Info($"Created {LogDir}");
            }

            if (File.Exists(OldShitListFile))
            {
                if (File.Exists(UserAvatarBlockListFile))
                {
                    Logging.Warning($"Both the new ({UserAvatarBlockListFile}) and old ({OldShitListFile}) avatar blacklists are present.  Merge them and get rid of the old one.");
                }
                else
                {
                    Logging.Warning($"Moving {OldShitListFile} to {UserAvatarBlockListFile}...");
                    File.Move(OldShitListFile, UserAvatarBlockListFile);
                }
            }

            SyncShitlistDLL();
            SyncYaraRuleset();

            LoadPlayerShitlists();
            if (KnownSkiddies.Count > 0)
            {
                Logging.Info($"Loaded {KnownSkiddies.Count} entries from {PlayerShitlistFile}");
            }

            AvatarShitList.UserAvatarBlockList = new UserAvatarSet(UserAvatarBlockListFile, "avatar ID", "avatar IDs", "user avatar blocklist", new string[]{
                    "User Blocked Avatar List", 
                    "Add one avatar ID per line.",
                    "Lines beginning with # are comments and will not be parsed."
                });
            AvatarShitList.UserAvatarAllowList = new UserAvatarSet(UserAvatarAllowListFile, "avatar ID", "avatar IDs", "user avatar allowlist", new string[]{
                    "User Allowed Avatar List", 
                    "Add one avatar ID per line.",
                    "Lines beginning with # are comments and will not be parsed."
                });

            AvatarShitList.UserAssetHashBlockList = new UserHashSet(UserAssetHashBlockListFile, "hash", "hashes", "user assetbundle hash blocklist", new string[]{
                    "User Blocked Asset Hashes", 
                    "Add one SHA256 checksum per line, in hexadecimal. (e.g. B478EDD1E837C83AE319A2788186A71751ECBBEBF6C8A8E57ED42349B863A5F1)",
                    "Lines beginning with # are comments and will not be parsed."
                });
            AvatarShitList.UserAssetHashAllowList = new UserHashSet(UserAssetHashAllowListFile, "hash", "hashes", "user assetbundle hash allowlist", new string[]{
                    "User Allowed Asset Hashes", 
                    "Add one SHA256 checksum per line, in hexadecimal. (e.g. B478EDD1E837C83AE319A2788186A71751ECBBEBF6C8A8E57ED42349B863A5F1)",
                    "Lines beginning with # are comments and will not be parsed."
                });

            // Fire up YARA.
            if(!Directory.Exists("Dependencies"))
                Directory.CreateDirectory("Dependencies");
            // Slight adaptation of knah's code in TSAC
            try 
            {
                using (var rsc = typeof(BundleBouncer).Assembly.GetManifestResourceStream(typeof(BundleBouncer), "libyara.dll"))
                {
                    using (var outfile = File.OpenWrite(Path.Combine("Dependencies", "libyara.dll")))
                    {
                        rsc.CopyTo(outfile);
                    }
                }
            }
            catch (IOException)
            {
                Logging.Warning("Could not write libyara.dll. If you're running multiple instances of VRChat, this is normal.");
            }
            AssetScanner.Init(this);

            this.Patches = new Patches(this);

            Logging.Startup();

            NetworkEvents.OnPlayerJoined += NetworkEvents_OnPlayerJoined;
            NetworkEvents.OnPlayerLeft += NetworkEvents_OnPlayerLeft;
            NetworkEvents.OnInstanceChanged += NetworkEvents_OnInstanceChanged;
        }

        public override void OnLateUpdate()
        {
            Patches.OnLateUpdate();
        }
        private void NetworkEvents_OnInstanceChanged(ApiWorld arg1, ApiWorldInstance arg2)
        {
            // Clear asset tracking.
            AvatarUsers.Clear();
            Avatars.Clear();
            DetectedSkiddies.Clear();
            if (shitterHighlighter != null && shitterHighlighter.field_Protected_HashSet_1_Renderer_0 != null)
                shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Clear();
        }

        private void NetworkEvents_OnPlayerLeft(Player player)
        {
            if (player == null)
            {
                Logging.Warning("Null player left!");
                return;
            }
            string userID = player.field_Private_APIUser_0.id;
            string avID = player.prop_ApiAvatar_0.id;
            if (AvatarUsers.ContainsKey(userID))
                AvatarUsers.Remove(userID);
            if (Avatars.ContainsKey(avID) && Avatars[avID].Users.Contains(userID))
                Avatars[avID].Users.Remove(userID);
            DoffAvatarOfShame(player);
        }

        private void NetworkEvents_OnPlayerJoined(Player player)
        {
            if (player == null)
            {
                Logging.Warning("Null player joined!");
                return;
            }
            if (KnownSkiddies.Contains(player.field_Private_APIUser_0.id))
            {
                DetectedSkiddies.Add(player);
                DonAvatarOfShame(player);
                Logging.Gottem($"Malicious user {player.field_Private_APIUser_0.displayName} has joined.");
                BBUI.NotifyUser($"Malicious user {player.field_Private_APIUser_0.displayName} has joined.");
            }
            else
            {
                DoffAvatarOfShame(player);
            }
        }


        public static void AddToSkiddieShitlist(string usrID)
        {
            // Try to find VRCPlayer
            var match = GetPlayers().Where(x => x.prop_APIUser_0.id == usrID).FirstOrDefault();
            if (match == null)
            {
                return;
            }

            // Currently in the room with us
            DetectedSkiddies.Add(match);

            // Add to our list of all known skiddies
            KnownSkiddies.Add(usrID);

            // Dunce hat
            DonAvatarOfShame(match);

            // Save
            SavePlayerShitlist();

            Logging.Info($"User {usrID} added to your user shitlist.");
        }

        private static void SavePlayerShitlist()
        {
            File.WriteAllText(Instance.PlayerShitlistFile, JsonConvert.SerializeObject(KnownSkiddies));
        }

        private static void LoadPlayerShitlists()
        {
            KnownSkiddies = JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(Instance.PlayerShitlistFile));
        }

        internal static void NotifyUserOfBlockedAvatar(string avatarID, string source, Dictionary<string, string> extra = null)
        {
            string wornby = "";
            string avstr = $"{avatarID}";
            string extrastr = "";
            string wornByCount = "";
            if (Avatars.TryGetValue(avatarID, out AvatarInfo av))
            {
                wornby = "Worn By: ";
                if (av.Users.Count > 0)
                {
                    wornby += String.Join(", ", av.Users);
                    wornByCount = $" worn by {av.Users.Count} user";
                    if (av.Users.Count > 1)
                    {
                        wornByCount = $" worn by {av.Users.Count} users";
                    }
                    foreach (var usrID in av.Users)
                    {
                        AddToSkiddieShitlist(usrID);
                    }
                }
                else
                {
                    wornby += "(no one?)";
                }
                avstr = $"{av.ID} ({av.Name})";
            }
            if (extra != null)
            {
                extrastr = " (" + String.Join(", ", extra.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToArray()) + ")";
            }
            Logging.Gottem($"Crasher blocked: Avatar {avstr}{wornby}{extrastr} [{source}]");
            BBUI.NotifyUser($"Blocked Avatar{wornByCount}.");
        }

        internal static void NotifyUserOfBlockedBundle(byte[] hash, string source)
        {
            var strhash = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Gottem($"Crasher blocked: AssetBundle {strhash} [{source}]");
            BBUI.NotifyUser($"Blocked AssetBundle.");
        }

        private static void DonAvatarOfShame(Player player)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            var selectRegion = player.transform.Find("SelectRegion");
            if (BundleBouncer.shitterHighlighter == null)
            {
                BundleBouncer.shitterHighlighter = HighlightsFX.field_Private_Static_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
                shitterHighlighter.highlightColor = Color.red;
                shitterHighlighter.enabled = true;
            }
            shitterHighlighter.field_Protected_HashSet_1_Renderer_0.AddIfNotPresent(selectRegion.GetComponent<Renderer>());
        }

        private static void DoffAvatarOfShame(Player player)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            if (player.transform == null)
                return; // ????
            var selectRegion = player.transform.Find("SelectRegion");
            if (selectRegion != null)
            {
                var renderer = selectRegion.GetComponent<Renderer>();
                if (shitterHighlighter != null)
                    if (shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Contains(renderer))
                        shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Remove(renderer);
            }
        }

        internal static Player[] GetPlayers()
        {
            var players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
            if (players == null)
                return new Player[0];
            else
            {
                lock (players)
                {
                    return players.ToArray();
                }
            }
        }


        internal void SyncShitlistDLL()
        {
            // Purposefully blocking.

            ShitlistDll = Path.Combine("Dependencies", "BundleBouncer.Shitlist.dll");
            var needsDL = !File.Exists(ShitlistDll);
            var www = new System.Net.WebClient();
            string remote_hash = www.DownloadString(LATEST_SHITLIST_CHECKSUM).Trim();
            string local_hash = needsDL ? "[Doesn't exist]" : String.Concat(IOTool.SHA256File(ShitlistDll).Select(x => x.ToString("X2")));
            //Logging.Info($"Definitions File Exists: {needsDL}");
            //Logging.Info($"Config - Sync Definitions: {Instance.Config.SyncDefinitions}");
            //Logging.Info($"Remote hash: {remote_hash}");
            //Logging.Info($"Local hash: {local_hash}");
            if (!needsDL && Instance.Config.SyncDefinitions)
            {
                if (local_hash != remote_hash)
                {
                    needsDL = true;
                }
            }
            if (needsDL)
            {
                Logging.Info($"Updating to shitlist of hash {remote_hash}...");
                www.DownloadFile(LATEST_SHITLIST_URL, ShitlistDll);
            }
            local_hash = String.Concat(IOTool.SHA256File(ShitlistDll).Select(x => x.ToString("X2")));
            Logging.Info($"Hash of {ShitlistDll} after update checks: {local_hash}");
            Logging.Info("Loading shitlist...");
            var asm = Assembly.LoadFrom(ShitlistDll);
            AvatarShitList.shitListProvider = (IShitListProvider)(asm.GetTypes().Where(x => x.Name == "ShitlistProvider").First().GetConstructor(new Type[0]).Invoke(null));
        }


        internal void SyncYaraRuleset()
        {
            // Purposefully blocking.
            var needsDL = !File.Exists(YaraCompiledRuleset);
            var www = new System.Net.WebClient();
            string remote_hash = www.DownloadString(LATEST_YARA_CHECKSUM).Trim();
            string local_hash = needsDL ? "[Doesn't exist]" : String.Concat(IOTool.SHA256File(YaraCompiledRuleset).Select(x => x.ToString("X2")));
            //Logging.Info($"Definitions File Exists: {needsDL}");
            //Logging.Info($"Config - Sync Definitions: {Instance.Config.SyncDefinitions}");
            //Logging.Info($"Remote hash: {remote_hash}");
            //Logging.Info($"Local hash: {local_hash}");
            if (!needsDL && Instance.Config.SyncDefinitions)
            {
                if (local_hash != remote_hash)
                {
                    needsDL = true;
                }
            }
            if (needsDL)
            {
                Logging.Info($"Updating to yara ruleset of hash {remote_hash}...");
                www.DownloadFile(LATEST_YARA_URL, YaraCompiledRuleset);
            }
            local_hash = String.Concat(IOTool.SHA256File(YaraCompiledRuleset).Select(x => x.ToString("X2")));
            Logging.Info($"Hash of {YaraCompiledRuleset} after update checks: {local_hash}");
        }

        internal static void SetUserAvatar(string userid, EAvatarType avType, dynamic avDict)
        {
            string avID = avDict["id"];
            if (!Avatars.ContainsKey(avID))
            {
                Avatars[avID] = new AvatarInfo();
            }
            Avatars[avID].FromDynamic(avDict);
            if (!AvatarUsers.ContainsKey(userid))
            {
                AvatarUsers[userid] = new UserAvatars();
            }
            AvatarUsers[userid].Set(avType, avID);
        }
    }

    internal class AssetInfo
    {
        public string URL;
        public string ID;
        public string Hash;
        public HashSet<string> UsedBy = new HashSet<string>();

        public AssetInfo(string avURL, string avID, string avHash)
        {
            this.URL = avURL;
            this.ID = avID;
            this.Hash = avHash;
        }


    }
}
