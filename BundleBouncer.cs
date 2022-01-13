using BundleBouncer.Data;
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
        /// Only used when we find an event previously unknown to us transmitting avatar IDs.
        /// </summary>
        public static HashSet<byte> writtenSamples = new HashSet<byte>();

        /// <summary>
        /// Where to put logs and data files
        /// </summary>
        public string UserDataDir { get { return Path.Combine("UserData", "BundleBouncer"); } }
        public string LogDir { get { return Path.Combine(UserDataDir, "Logs"); } }
        public string OldShitListFile { get { return Path.Combine(UserDataDir, "Avatars.txt"); } }
        public string UserAvatarShitListFile { get { return Path.Combine(UserDataDir, "My-Blocked-Avatars.txt"); } }
        public string PlayerShitlistFile { get { return Path.Combine(UserDataDir, "Player-Blacklist.json"); } }

        /// <summary>
        /// Previous users of exploitave assetbundles.
        /// </summary>
        public static HashSet<string> KnownSkiddies = new HashSet<string>();

        /// <summary>
        /// Current cache of known skiddies in the current scene, as VRCPlayers.
        /// </summary>
        public static HashSet<Player> DetectedSkiddies = new HashSet<Player>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AttemptAvatarDownloadDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, IntPtr multicastDelegatePtr, bool idfk, IntPtr nativeMethodInfo);
        private static AttemptAvatarDownloadDelegate dgAttemptAvatarDownload;

        public static string AvatarOfShameURL = ""; // TODO: Make one

        static HighlightsFXStandalone shitterHighlighter;

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

            if(File.Exists(OldShitListFile))
            {
                if(File.Exists(UserAvatarShitListFile))
                {
                    Logging.Warning($"Both the new ({UserAvatarShitListFile}) and old ({OldShitListFile}) avatar blacklists are present.  Merge them and get rid of the old one.");
                } else {
                    Logging.Warning($"Moving {OldShitListFile} to {UserAvatarShitListFile}...");
                    File.Move(OldShitListFile, UserAvatarShitListFile);
                }
            }

            if (!File.Exists(UserAvatarShitListFile))
            {
                File.WriteAllLines(UserAvatarShitListFile, new string[] { "# Add avatar IDs below this line, but without the #.", "" });
                Logging.Info($"Created {UserAvatarShitListFile}");
            }

            if(!File.Exists(PlayerShitlistFile))
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

            unsafe
            {
                // God I hate pointers.
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AssetBundleDownloadManager).GetMethod(
                        nameof(AssetBundleDownloadManager.Method_Internal_UniTask_1_InterfacePublicAbstractIDisposableGaObGaUnique_ApiAvatar_MulticastDelegateNInternalSealedVoUnUnique_Boolean_0)))
                    .GetValue(null);

                MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), typeof(BundleBouncer).GetMethod(nameof(OnAttemptAvatarDownload), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

                dgAttemptAvatarDownload = Marshal.GetDelegateForFunctionPointer<AttemptAvatarDownloadDelegate>(originalMethodPointer);
                Logging.Info($"Hooked AssetBundleDownloadManager.");
            }

            HarmonyInstance.Patch(
                typeof(VRCNetworkingClient).GetMethod("OnEvent"),
                new HarmonyLib.HarmonyMethod(
                    typeof(BundleBouncer).GetMethod(nameof(Detour),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ), null, null, null, null);

            NetworkEvents.OnPlayerJoined += NetworkEvents_OnPlayerJoined;
            NetworkEvents.OnPlayerLeft += NetworkEvents_OnPlayerLeft;
        }

        private void NetworkEvents_OnPlayerLeft(Player player)
        {
            DoffAvatarOfShame(player);
        }

        private void NetworkEvents_OnPlayerJoined(Player player)
        {
            if(DetectedSkiddies.Contains(player))
            {
                DonAvatarOfShame(player);
            } else {
                DoffAvatarOfShame(player);
            }
        }

        /**
         * Stolen from Behemoth (with permission)
         * Not used right now.
         */
        private static unsafe void Hook(MethodBase target, string detour)
        {
            var originalMethodPointer = *(IntPtr*)UnhollowerSupport.MethodBaseToIl2CppMethodInfoPointer(target);
            var detourPointer = typeof(BundleBouncer).GetMethod(detour, BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
            MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), detourPointer);
            Logging.Info($"Hooked {target.Name} to {detour}");
        }

        public static void AddToSkiddieShitlist(string usrID) { 
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
            var shameTF = player.transform.Find("_BB_SHAME");
            if (shameTF == null)
            {
                var selectRegion = player.transform.Find("SelectRegion");
                if (BundleBouncer.shitterHighlighter == null) {
                    BundleBouncer.shitterHighlighter = HighlightsFX.field_Private_Static_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
                    shitterHighlighter.highlightColor = Color.red;
                    shitterHighlighter.enabled = true;
                }
                shitterHighlighter.field_Protected_HashSet_1_Renderer_0.AddIfNotPresent(selectRegion.GetComponent<Renderer>());
            }
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
                if(shitterHighlighter!=null)
                    if(shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Contains(renderer))
                        shitterHighlighter.field_Protected_HashSet_1_Renderer_0.Remove(renderer);
            }
        }

        private static Player[] GetPlayers()
        {
            var players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
            if (players == null)
                return new Player[0];
            else {
                lock (players) {
                    return players.ToArray();
                }
            }
        }

        private static bool HasProp(dynamic thing, string key)
        {
            if (thing == null)
            {
                return false;
            }
            else if (thing is IDictionary<string, object> dict)
            {
                return dict.ContainsKey(key);
            }
            return thing.GetType().GetProperty(key) != null;
        }

        // I have no idea what I'm doing
        static unsafe IntPtr OnAttemptAvatarDownload(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr pApiAvatar, IntPtr pMulticastDelegate, bool param_3, IntPtr nativeMethodInfo)
        {
            using (var ctx = new AttemptAvatarDownloadContext(pApiAvatar == IntPtr.Zero ? null : new ApiAvatar(pApiAvatar)))
            {
                var av = AttemptAvatarDownloadContext.apiAvatar;
                Logging.Info($"Attempting to download avatar {av.id} ({av.name})...");
                if (AvatarShitList.IsCrasher(av.id))
                {
                    Logging.Gottem($"Crasher blocked: {av.id} ({av.name}) -> {av.unityPackageUrl} (OnAttemptAvatarDownload)");

                    // TODO: Make configurable, or point to big blaring CRASHER avatar?
                    //av.assetUrl = "http://0.0.0.0/doesnt-exist.asset";
                    av.assetUrl = null; // This supposedly returns faster.
                    av.version = int.MaxValue; // Ensure this doesn't get overwritten. :3c
                }
                return dgAttemptAvatarDownload(hiddenStructReturn, thisPtr, pApiAvatar, pMulticastDelegate, param_3, nativeMethodInfo);
            }
        }

        private struct AttemptAvatarDownloadContext : IDisposable
        {
            internal static ApiAvatar apiAvatar;

            public AttemptAvatarDownloadContext(ApiAvatar iApiAvatar)
            {
                apiAvatar = iApiAvatar;
            }

            public void Dispose()
            {
                apiAvatar = null;
            }
        }

        private static bool Detour(ref EventData __0)
        {
            try
            {
                switch (__0.Code)
                {
                    case 255: // join
                        {
                            string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                            //Logging.Info(customProps);
                            dynamic playerHashtable = JsonConvert.DeserializeObject(customProps);
                            var avdata = playerHashtable["249"];
                            if (HasProp(avdata, "avatarDict"))
                            {
                                if (!CheckAvDict(avdata["avatarDict"], avdata["user"]["id"], __0.Code, false))
                                    return false;
                            }
                            if (HasProp(avdata, "favatarDict"))
                            {
                                if (!CheckAvDict(avdata["favatarDict"], avdata["user"]["id"], __0.Code, true))
                                    return false;
                            }
                        }
                        break;
                    case 253: // properties_changed
                        {
                            string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                            //Logging.Info(customProps);
                            dynamic playerHashtable = JsonConvert.DeserializeObject(customProps);
                            var avdata = playerHashtable["251"];
                            if (HasProp(avdata, "avatarDict"))
                            {
                                if (!CheckAvDict(avdata["avatarDict"], avdata["user"]["id"], __0.Code, false))
                                    return false;
                            }
                            if (HasProp(avdata, "favatarDict"))
                            {
                                if (!CheckAvDict(avdata["favatarDict"], avdata["user"]["id"], __0.Code, true))
                                    return false;
                            }
                        }
                        break;
                    default:
                        {
                            if (!writtenSamples.Contains(__0.Code))
                            {
                                string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                                if (!customProps.Contains("avtr_"))
                                    break;
                                
                                writtenSamples.Add(__0.Code);
                                File.WriteAllText(Path.Combine("UserData", "BundleBouncer", $"{__0.Code}.json"), customProps);
                                Logging.Info($"Captured event {__0.Code} that appears to have sent an avatar ID.  Please notify the author via email.");
                                //dynamic playerHashtable = JsonConvert.DeserializeObject(customProps);
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.Error(e);
            }
            return true;
        }

        private static bool CheckAvDict(dynamic avdata, string user, int code, bool is_fallback)
        {
            string avID = avdata["id"];
            string avName = avdata["name"];
            string fbstr = is_fallback ? "fallback" : "main";
            Logging.Info($"Attempting to download {fbstr} avatar {avID} ({avName} via E{code})...");
            if (AvatarShitList.IsCrasher(avID))
            {
                Logging.Gottem($"Crasher from {user} blocked: {avID} ({avName}) (E{code})");
                return false;
            }
            return true;
        }
    }
}
