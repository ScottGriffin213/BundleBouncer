﻿using BundleBouncer.Data;
using BundleBouncer.Utilities;
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;
using VRC;
using VRC.Core;
using VRChatUtilityKit.Utilities;

namespace BundleBouncer
{
    public class BundleBouncer : MelonMod
    {
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
        public string UserAvatarShitListFile { get { return Path.Combine(UserDataDir, "My-Blocked-Avatars.txt"); } }
        public string PlayerShitlistFile { get { return Path.Combine(UserDataDir, "Player-Blacklist.json"); } }

        internal Patches Patches { get; private set; }

        /// <summary>
        /// Previous users of exploitive assetbundles.
        /// </summary>
        public static HashSet<string> KnownSkiddies = new HashSet<string>();

        /// <summary>
        /// Current cache of known skiddies in the current scene, as VRCPlayers.
        /// </summary>
        public static HashSet<Player> DetectedSkiddies = new HashSet<Player>();

        //public static string AvatarOfShameURL = ""; // TODO: Make one

        // Red ESP pill
        static HighlightsFXStandalone shitterHighlighter;

        // Mostly for logging and datastream tracking.
        static Dictionary<string, AssetInfo> assetInfo = new Dictionary<string, AssetInfo>();
        private static Dictionary<string, AvatarInfo> Avatars = new Dictionary<string, AvatarInfo>();
        private static Dictionary<string, string> avIDsByFileSubURL = new Dictionary<string, string>();
        private static Dictionary<IntPtr, AssetBundleDownload> assetBundleDownloadToFactory = new Dictionary<IntPtr, AssetBundleDownload>();
        public static readonly string BLOCKED_AVTR_ID = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
        public static readonly string BLOCKED_FILE_URL = "https://0.0.0.0/blocked.dat"; // FIXME

        public override void OnApplicationStart()
        {
            Logging.LI = LoggerInstance;
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
                if (File.Exists(UserAvatarShitListFile))
                {
                    Logging.Warning($"Both the new ({UserAvatarShitListFile}) and old ({OldShitListFile}) avatar blacklists are present.  Merge them and get rid of the old one.");
                }
                else
                {
                    Logging.Warning($"Moving {OldShitListFile} to {UserAvatarShitListFile}...");
                    File.Move(OldShitListFile, UserAvatarShitListFile);
                }
            }

            if (!File.Exists(UserAvatarShitListFile))
            {
                File.WriteAllLines(UserAvatarShitListFile, new string[] { "# Add avatar IDs below this line, but without the #.", "" });
                Logging.Info($"Created {UserAvatarShitListFile}");
            }

            if (!File.Exists(PlayerShitlistFile))
            {
                File.WriteAllText(PlayerShitlistFile, "[]");
                Logging.Info($"Created {PlayerShitlistFile}");
            }

            LoadPlayerShitlist();
            if (KnownSkiddies.Count > 0)
            {
                Logging.Info($"Loaded {KnownSkiddies.Count} entries from {PlayerShitlistFile}");
            }

            AvatarShitList.UserShitList = File.ReadAllLines(UserAvatarShitListFile).Select(x => x.Trim().ToLowerInvariant()).Where(x => x != "" && !x.StartsWith("#")).ToHashSet();
            if (AvatarShitList.UserShitList.Count > 0)
            {
                Logging.Info($"Loaded {AvatarShitList.UserShitList.Count} entries from {UserAvatarShitListFile}");
            }

            this.Patches = new Patches(this);

            NetworkEvents.OnPlayerJoined += NetworkEvents_OnPlayerJoined;
            NetworkEvents.OnPlayerLeft += NetworkEvents_OnPlayerLeft;
            NetworkEvents.OnInstanceChanged += NetworkEvents_OnInstanceChanged;
        }
        private void NetworkEvents_OnInstanceChanged(ApiWorld arg1, ApiWorldInstance arg2)
        {
            // Clear asset tracking.
            assetInfo.Clear();
            DetectedSkiddies.Clear();
            if (shitterHighlighter != null && shitterHighlighter.field_Protected_HashSet_1_Renderer_0 != null)
                shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Clear();
        }

        private void NetworkEvents_OnPlayerLeft(Player player)
        {
            DoffAvatarOfShame(player);
        }

        private void NetworkEvents_OnPlayerJoined(Player player)
        {
            if (DetectedSkiddies.Contains(player))
            {
                DonAvatarOfShame(player);
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

            DetectedSkiddies.Add(match);
            KnownSkiddies.Add(usrID);
            DonAvatarOfShame(match);
            SavePlayerShitlist();
        }

        private static void SavePlayerShitlist()
        {
            File.WriteAllText(Instance.PlayerShitlistFile, JsonConvert.SerializeObject(KnownSkiddies));
        }

        private static void LoadPlayerShitlist()
        {
            KnownSkiddies = JsonConvert.DeserializeObject<HashSet<string>>(File.ReadAllText(Instance.PlayerShitlistFile));
        }

        internal static void NotifyUserOfBlockedAvatar(string avatarID, string source, Dictionary<string, string> extra = null)
        {
            AvatarInfo av = null;
            string wornby = "";
            string avstr = $"{avatarID}";
            string extrastr = "";
            string wornByCount = "";
            if(Avatars.TryGetValue(avatarID, out av))
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
                }
                else
                {
                    wornby += "(no one?)";
                }
                avstr = $"{av.ID} ({av.Name})";
            }
            if(extra != null)
            {
                extrastr = " ("+String.Join(", ", extra.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToArray())+")";
            }
            Logging.Gottem($"Crasher blocked: Avatar {avstr}{wornby}{extrastr} [{source}]");
            BBUI.NotifyUser($"Blocked Avatar{wornByCount}.");
        }

        private static void DonAvatarOfShame(Player player)
        {
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            /*
            if(s_BoothCat == null)
            {
                LoadAvatarOfShame();
            }
            */
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
        private static string GetFileIDFrom(string unityPackageUrl)
        {

            throw new NotImplementedException();
        }


        internal static void addAssetURL(string avURL, string avID, string user)
        {
            Uri parts = new Uri(avURL);
            string fileID = parts.AbsolutePath.Split('/').Where(x => x.StartsWith("file_")).First();
            avIDsByFileSubURL[$"file/{fileID}"] = avID;
            if (!assetInfo.ContainsKey(avURL))
                assetInfo[avURL] = new AssetInfo(avURL, avID, null);
            if (!assetInfo[avURL].UsedBy.Contains(user))
                assetInfo[avURL].UsedBy.Add(user);
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
