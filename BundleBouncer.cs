using BundleBouncer.Data;
using BundleBouncer.Utilities;
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
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
        /// Previous users of exploitive assetbundles.
        /// </summary>
        public static HashSet<string> KnownSkiddies = new HashSet<string>();

        /// <summary>
        /// Current cache of known skiddies in the current scene, as VRCPlayers.
        /// </summary>
        public static HashSet<Player> DetectedSkiddies = new HashSet<Player>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AttemptAvatarDownloadDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, IntPtr multicastDelegatePtr, bool idfk, IntPtr nativeMethodInfo);
        private static AttemptAvatarDownloadDelegate dgAttemptAvatarDownload;

        /*
        //public static extern AssetBundle LoadFromFile(string path, [UnityEngine.Internal.DefaultValue("0")] uint crc, [UnityEngine.Internal.DefaultValue("0")] ulong offset);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr LoadFromFileDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, string file, int crc, ulong offset, IntPtr nativeMethodInfo);
        private static LoadFromFileDelegate dgLoadFromFile;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr LoadFromFileAsyncDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, string file, int crc, ulong offset, IntPtr nativeMethodInfo);
        private static LoadFromFileAsyncDelegate dgLoadFromFileAsync;
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr LoadFromFile1Delegate(IntPtr retPtr, IntPtr thisPtr, string file, IntPtr nativeMethodInfo);
        private static LoadFromFile1Delegate origLoadFromFile_1;

        //public static string AvatarOfShameURL = ""; // TODO: Make one

        static HighlightsFXStandalone shitterHighlighter;

        /// <summary>
        /// Mostly for logging and datastream tracking.
        /// </summary>
        static Dictionary<string, AssetInfo> assetInfo = new Dictionary<string, AssetInfo>();


        private static Dictionary<string, string> avIDsByFileSubURL = new Dictionary<string, string>();
        private static Dictionary<string, Tuple<IntPtr, IntPtr>> ourOriginalPointers = new Dictionary<string, Tuple<IntPtr, IntPtr>>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromFileAsync_InternalDelegate(IntPtr path, uint crc, ulong offset);
        private static LoadFromFileAsync_InternalDelegate origLoadFromFileAsync_Internal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromMemory_InternalDelegate(IntPtr binary, uint crc);
        private static LoadFromMemory_InternalDelegate origLoadFromMemory_Internal;


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
            // This is going to be dangerous as fuck, buckle up sweetheart.
            foreach (var method in typeof(AssetBundle).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                switch (method.Name)
                {
                    //public unsafe static AssetBundle LoadFromFile(string path)
                    case "LoadFromFile":
                        switch(method.GetParameters().Length)
                        {
                            case 1: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_1)); break;
                            //case 2: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_2)); break;
                            //case 3: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_3)); break;
                        }
                        break;
                    //public static AssetBundle LoadFromFile_Internal(string path, uint crc, ulong offset);
                    case "LoadFromFile_Internal":
                        StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_Internal));
                        break;
                }
            }
            /** Hard-crashes with implement-me message
             * TODO: Rattle cages in VRCMG
            foreach(var method in typeof(UnityWebRequestAssetBundle).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                switch (method.Name)
                {
                    case "GetAssetBundle":
                        var p = method.GetParameters();
                        //public static UnityWebRequest GetAssetBundle(string uri);
                        if (p.Length == 1 && p[0].ParameterType == typeof(string))
                            StaticHarmony(method, nameof(BundleBouncer.OnGetAssetBundle_1));
                        break;
                }
            }
            */

            StaticHarmony(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnEvent)), nameof(OnEvent));

            // AssetBundle.LoadFromFileAsync_InternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromFileAsync_InternalDelegate>("UnityEngine.AssetBundle::LoadFromFileAsync_Internal");
            PatchICall("UnityEngine.AssetBundle::LoadFromFileAsync_Internal", out origLoadFromFileAsync_Internal, "OnLoadFromFileAsync_Internal");

            // AssetBundle.LoadFromMemory_InternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromMemory_InternalDelegate>("UnityEngine.AssetBundle::LoadFromMemory_Internal");
            PatchICall("UnityEngine.AssetBundle::LoadFromMemory_Internal", out origLoadFromMemory_Internal, "OnLoadFromMemory_Internal");


            NetworkEvents.OnPlayerJoined += NetworkEvents_OnPlayerJoined;
            NetworkEvents.OnPlayerLeft += NetworkEvents_OnPlayerLeft;
            NetworkEvents.OnInstanceChanged += NetworkEvents_OnInstanceChanged;
        }

        private static IntPtr OnLoadFromMemory_Internal(IntPtr dataPtr, uint crc)
        {
            var data = (new Il2CppStructArray<byte>(dataPtr)).ToArray();
            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = new MemoryStream(data))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load memory-resident assetbundle (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromMemory_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: (memory-resident) (CRC: {crc}, SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromMemory_Internal(dataPtr, crc);
        }

        private static IntPtr OnLoadFromFileAsync_Internal(IntPtr pathPtr, uint crc, ulong offset)
        {
            var path = IL2CPP.Il2CppStringToManaged(pathPtr);

            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = File.OpenRead(path))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr}) via AssetBundle.LoadFromMemory_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromFileAsync_Internal(pathPtr, crc, offset);
        }

        private static bool OnGetAssetBundle_1(string uri, UnityWebRequest __result)
        {
            Logging.Info($"Unity.GetAssetBundle({uri})");
            return true;
        }

        private void StaticHarmony(MethodInfo hookee, string hooker)
        {
            string psig = String.Join(",", hookee.GetParameters().Select(x => x.ParameterType.ToString()));
            string sig = $"{hookee.DeclaringType}.{hookee.Name}({psig})";
            try
            {
                HarmonyInstance.Patch(hookee, typeof(BundleBouncer).GetMethod(hooker, BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod());
                Logging.Info($"Patched {sig} to {hooker} (static)");
            }
            catch (Exception e)
            {
                Logging.Error($"Unable to patch {sig} (static):");
                Logging.Error(e.ToString());
            }
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

        /**
         * From Knah's fabulous Finitizer.
         * https://github.com/knah/VRCMods/blob/9ad060a8aa05c1454696f2625ad6a857fec1fed6/Finitizer/FinitizerMod.cs#L106
         */
        private static unsafe void PatchICall<T>(string name, out T original, string patchName) where T : MulticastDelegate
        {
            var originalPointer = IL2CPP.il2cpp_resolve_icall(name);
            if (originalPointer == IntPtr.Zero)
            {
                Logging.Warning($"ICall {name} was not found, not patching");
                original = null;
                return;
            }

            var target = typeof(BundleBouncer).GetMethod(patchName, BindingFlags.Static | BindingFlags.NonPublic);
            var functionPointer = target.MethodHandle.GetFunctionPointer();

            MelonUtils.NativeHookAttach((IntPtr)(&originalPointer), functionPointer);
            ourOriginalPointers[name] = new Tuple<IntPtr, IntPtr>(originalPointer, functionPointer);
            original = Marshal.GetDelegateForFunctionPointer<T>(originalPointer);
        }

        /**
         * Stolen from Behemoth (with permission)
         */
        private static unsafe IntPtr Hook(MethodBase target, string detour)
        {
            string psig = String.Join(",", target.GetParameters().Select(x => x.ParameterType.ToString()));
            string sig = $"{target.DeclaringType}.{target.Name}({psig})";
            var originalMethodPointer = *(IntPtr*)UnhollowerSupport.MethodBaseToIl2CppMethodInfoPointer(target);
            var detourPointer = typeof(BundleBouncer).GetMethod(detour, BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
            MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), detourPointer);
            Logging.Info($"Hooked {target.Name} to {detour}");
            return originalMethodPointer;
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

        private static Player[] GetPlayers()
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

        private static bool OnLoadFromFile_1(string __0, ref AssetBundle __result)
        {
            string path = __0;

            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = File.OpenRead(path))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (SHA256: {hashstr}) via AssetBundle.LoadFromFile(string)...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                __result = null;
                return false;
            }
            return true;
        }

        private static bool OnLoadFromFile_2(string __0, uint __1, ref AssetBundle __result)
        {
            string path = __0;
            uint crc = __1;

            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = File.OpenRead(path))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromFile...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                __result = null;
                return false;
            }
            return true;
        }

        private static bool OnLoadFromFile_3(string __0, uint __1, ulong __2, ref AssetBundle __result)
        {
            string path = __0;
            uint crc = __1;
            ulong offset = __2;

            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = File.OpenRead(path))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr}) via AssetBundle.LoadFromFile...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                __result = null;
                return false;
            }
            return true;
        }

        private static bool OnLoadFromFile_Internal(string __0, int __1, ulong __2, ref AssetBundle __result)
        {
            string path = __0;
            int crc = __1;
            ulong offset = __2;

            byte[] hash;
            using (var sha256 = new SHA256Managed())
            {
                using (var stream = File.OpenRead(path))
                {
                    stream.Position = 0;
                    hash = sha256.ComputeHash(stream);
                }
            }
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr}) via AssetBundle.LoadFromFile_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr})");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                __result = null;
                return false;
            }
            return true;
        }

        // I have no idea what I'm doing
        static unsafe IntPtr OnAttemptAvatarDownload(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr pApiAvatar, IntPtr pMulticastDelegate, bool param_3, IntPtr nativeMethodInfo)
        {
            using (var ctx = new AttemptAvatarDownloadContext(pApiAvatar == IntPtr.Zero ? null : new ApiAvatar(pApiAvatar)))
            {
                var av = AttemptAvatarDownloadContext.apiAvatar;
                Logging.Info($"Attempting to download avatar {av.id} ({av.name}) via AssetBundleDownloadManager...");
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

        private static string GetFileIDFrom(string unityPackageUrl)
        {

            throw new NotImplementedException();
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

        private static bool OnEvent(ref EventData __0)
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
                                var path = Path.Combine("UserData", "BundleBouncer", $"{__0.Code}.json");
                                File.WriteAllText(path, customProps);
                                Logging.Info($"Captured event {__0.Code} that appears to have sent an avatar ID.  Please notify the author via email, and attach {path}.");
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
            foreach (dynamic up in avdata["unityPackages"])
            {
                addAssetURL(up["assetUrl"].ToString(), avID, user);
            }
            Logging.Info($"Attempting to download {fbstr} avatar {avID} ({avName} via E{code})...");
            if (AvatarShitList.IsCrasher(avID))
            {
                AddToSkiddieShitlist(user);
                var player = GetPlayers().Where(x => x.field_Private_APIUser_0.id == user).FirstOrDefault();
                if (player != null)
                {
                    Logging.Gottem($"Crasher from {player.field_Private_APIUser_0.displayName} ({user}) blocked: {avID} ({avName}) (via event {code})");
                    BBUI.NotifyUser($"Crasher from {player.field_Private_APIUser_0.displayName} blocked!");
                }
                else
                {
                    Logging.Gottem($"Crasher from {user} blocked: {avID} ({avName}) (via event {code})");
                    BBUI.NotifyUser($"Crasher from {user} blocked!");
                }
                return false;
            }
            return true;
        }

        private static void addAssetURL(string avURL, string avID, string user)
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
