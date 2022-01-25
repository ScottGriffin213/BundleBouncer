using BundleBouncer.Data;
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
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;
using VRC;
using VRC.Core;

namespace BundleBouncer
{
    class Patches
    {
        private BundleBouncer bb;

        /// <summary>
        /// Only used when we find an event previously unknown to us transmitting avatar IDs.
        /// </summary>
        public static HashSet<byte> writtenPhotonSamples = new HashSet<byte>();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromFileAsync_InternalDelegate(IntPtr path, uint crc, ulong offset);
        private static LoadFromFileAsync_InternalDelegate origLoadFromFileAsync_Internal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr AttemptAvatarDownloadDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, IntPtr multicastDelegatePtr, bool idfk, IntPtr nativeMethodInfo);
        private static AttemptAvatarDownloadDelegate dgAttemptAvatarDownload;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromStream_InternalDelegate(IntPtr stream, uint crc, uint managedReadBufferSize);
        private static LoadFromStream_InternalDelegate origLoadFromStream_Internal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromStreamAsync_InternalDelegate(IntPtr stream, uint crc, uint managedReadBufferSize);
        private static LoadFromStreamAsync_InternalDelegate origLoadFromStreamAsync_Internal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromMemory_InternalDelegate(IntPtr binary, uint crc);
        private static LoadFromMemory_InternalDelegate origLoadFromMemory_Internal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr LoadFromMemoryAsync_InternalDelegate(IntPtr binary, uint crc);
        private static LoadFromMemoryAsync_InternalDelegate origLoadFromMemoryAsync_Internal;

        public Patches(BundleBouncer bundleBouncer)
        {
            bb = bundleBouncer;

            unsafe
            {
                Logging.Info("1");
                // God I hate pointers.
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AssetBundleDownloadManager).GetMethod(
                        nameof(AssetBundleDownloadManager.Method_Internal_UniTask_1_InterfacePublicAbstractIDisposableGaObGaUnique_ApiAvatar_MulticastDelegateNInternalSealedVoUnUnique_Boolean_0)))
                    .GetValue(null);

                Logging.Info("2");
                MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), typeof(Patches).GetMethod(nameof(Patches.OnAttemptAvatarDownload), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

                Logging.Info("3");
                dgAttemptAvatarDownload = Marshal.GetDelegateForFunctionPointer<AttemptAvatarDownloadDelegate>(originalMethodPointer);
                Logging.Info($"Hooked AssetBundleDownloadManager.");
            }

            foreach (var method in typeof(AssetBundle).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                switch (method.Name)
                {
                    //public unsafe static AssetBundle LoadFromFile(string path)
                    case "LoadFromFile":
                        switch (method.GetParameters().Length)
                        {
                            case 1: PatchHarmony(method, nameof(OnLoadFromFile_1)); break;
                                //case 2: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_2)); break;
                                //case 3: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_3)); break;
                        }
                        break;
                    //public static AssetBundle LoadFromFile_Internal(string path, uint crc, ulong offset);
                    case "LoadFromFile_Internal":
                        PatchHarmony(method, nameof(OnLoadFromFile_Internal));
                        break;
                }
            }
            foreach (var method in typeof(AssetBundle).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                switch (method.Name)
                {
                    //public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, uint crc)
                    case "LoadFromCacheOrDownload":
                        if (HarmonyUtils.MatchParameters(method, new Type[] { typeof(string), typeof(Hash128), typeof(uint) }))
                        {
                            PatchHarmony(method, nameof(OnLoadFromCacheOrDownload));
                        }
                        break;
                }
            }

            PatchHarmony(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnEvent)), nameof(OnEvent));

            PatchHarmony(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Private_Static_String_String_String_Int32_String_String_String_0)), nameof(OnCreateAssetBundleDownload));

            PatchHarmony(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0)), nameof(OnGetAssetBundleGetter));
            PatchHarmony(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0)), nameof(OnGetGameObjectGetter));

            PatchHarmony(typeof(InterfacePublicAbstractIDisposableAsObAsUnique).GetProperty("prop_AssetBundle_0").GetGetMethod(), postfix: nameof(OnIIDisposableAssetBundleContainer_GetAssetBundle));

            PatchHarmony(typeof(ApiFile).GetMethod(nameof(ApiFile.DownloadFile)), prefix: nameof(OnAPIFileDownloadFile));

            //private void InternalCreateAssetBundleCached(string url, string name, Hash128 hash, uint crc)
            PatchHarmony(typeof(DownloadHandlerAssetBundle).GetMethod(nameof(DownloadHandlerAssetBundle.InternalCreateAssetBundleCached), BindingFlags.Public | BindingFlags.Instance),
                prefix: nameof(OnDownloadHandlerAssetBundle_InternalCreateAssetBundleCached));

            // AssetBundle.LoadFromFileAsync_InternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromFileAsync_InternalDelegate>("UnityEngine.AssetBundle::LoadFromFileAsync_Internal");
            PatchICall("UnityEngine.AssetBundle::LoadFromFileAsync_Internal", out origLoadFromFileAsync_Internal, nameof(OnLoadFromFileAsync_Internal));

            // AssetBundle.LoadFromMemory_InternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromMemory_InternalDelegate>("UnityEngine.AssetBundle::LoadFromMemory_Internal");
            PatchICall("UnityEngine.AssetBundle::LoadFromMemory_Internal", out origLoadFromMemory_Internal, nameof(OnLoadFromMemory_Internal));

            // AssetBundle.LoadFromMemoryAsync_InternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromMemoryAsync_InternalDelegate>("UnityEngine.AssetBundle::LoadFromMemoryAsync_Internal");
            PatchICall("UnityEngine.AssetBundle::LoadFromMemoryAsync_Internal", out origLoadFromMemoryAsync_Internal, nameof(OnLoadFromMemoryAsync_Internal));

            // AssetBundle.LoadFromStreamInternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromStreamInternalDelegate>("UnityEngine.AssetBundle::LoadFromStreamInternal");
            PatchICall("UnityEngine.AssetBundle::LoadFromStreamInternal", out origLoadFromStream_Internal, nameof(OnLoadFromStream_Internal));

            // AssetBundle.LoadFromStreamAsyncInternalDelegateField = IL2CPP.ResolveICall<AssetBundle.LoadFromStreamAsyncInternalDelegate>("UnityEngine.AssetBundle::LoadFromStreamAsyncInternal");
            PatchICall("UnityEngine.AssetBundle::LoadFromStreamAsyncInternal", out origLoadFromStreamAsync_Internal, nameof(OnLoadFromStreamAsync_Internal));
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

            var target = typeof(Patches).GetMethod(patchName, BindingFlags.Static | BindingFlags.NonPublic);
            var functionPointer = target.MethodHandle.GetFunctionPointer();

            MelonUtils.NativeHookAttach((IntPtr)(&originalPointer), functionPointer);
            //ourOriginalPointers[name] = new Tuple<IntPtr, IntPtr>(originalPointer, functionPointer);
            original = Marshal.GetDelegateForFunctionPointer<T>(originalPointer);
            Logging.Info($"Patched icall {name}");
        }

        /**
         * Stolen from Behemoth (with permission)
         */
        private static unsafe IntPtr Hook(MethodBase target, string detour)
        {
            string psig = String.Join(",", target.GetParameters().Select(x => x.ParameterType.ToString()));
            string sig = $"{target.DeclaringType}.{target.Name}({psig})";
            var originalMethodPointer = *(IntPtr*)UnhollowerSupport.MethodBaseToIl2CppMethodInfoPointer(target);
            var detourPointer = typeof(Patches).GetMethod(detour, BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
            MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), detourPointer);
            Logging.Info($"Hooked {target.Name} to {detour}");
            return originalMethodPointer;
        }



        //private void InternalCreateAssetBundleCached(string url, string name, Hash128 hash, uint crc)
        private static bool OnDownloadHandlerAssetBundle_InternalCreateAssetBundleCached(string __0, string __1, Hash128 __2, uint __3)
        {
            // Not seen in use yet.
            Logging.Info($"DownloadHandlerAssetBundle.InternalCreateAssetBundleCached(url: {__0}, name: {__1}, hash: {__2}, CRC: {__3})");
            return true;
        }

        // 
        // public unsafe InterfacePublicAbstractIDisposableGaObGaUnique Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0()
        private static bool OnGetGameObjectGetter(AssetBundleDownload __instance)
        {
            //HarmonyUtils.ShowDStack();
            /* This is triggered a lot and spams the log, resulting in lag spikes.
            Logging.Info($"AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0(): ct: {__instance.field_Private_ContentType_0}, 0: {__instance.field_Private_String_0}, 1: {__instance.field_Private_String_1}, 2: {__instance.field_Private_String_2}");
            if (ApiFile.TryParseFileIdAndVersionFromFileAPIUrl(__instance.field_Private_String_1, out string fileId, out int fileVersion))
            {
                Logging.Info($"AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0(): fileID: {fileId}, fileVersion: {fileVersion}");
            }
            */
            if (__instance.field_Private_ContentType_0 == ContentType.Avatar && AvatarShitList.IsCrasher(__instance.field_Private_String_0))
            {
                Logging.Gottem($"Crasher blocked: AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0 (avatar ID: {__instance.field_Private_String_0}, URL: {__instance.field_Private_String_1})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                __instance.field_Private_String_0 = BundleBouncer.BLOCKED_AVTR_ID;
                __instance.field_Private_String_1 = BundleBouncer.BLOCKED_FILE_URL;
            }
            return true;
        }

        private static bool OnIIDisposableAssetBundleContainer_GetAssetBundle()
        {
            // TODO?
            return true;
        }

        // public unsafe InterfacePublicAbstractIDisposableAsObAsUnique Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0()
        private static bool OnGetAssetBundleGetter(AssetBundleDownload __instance)
        {
            /* This is triggered a lot and spams the log, resulting in lag spikes.
            Logging.Info($"AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0(): ct: {__instance.field_Private_ContentType_0}, 0: {__instance.field_Private_String_0}, 1: {__instance.field_Private_String_1}, 2: {__instance.field_Private_String_2}");
            if (ApiFile.TryParseFileIdAndVersionFromFileAPIUrl(__instance.field_Private_String_1, out string fileId, out int fileVersion))
            {
                Logging.Info($"AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0(): fileID: {fileId}, fileVersion: {fileVersion}");
            }
            */
            if (__instance.field_Private_ContentType_0 == ContentType.Avatar && AvatarShitList.IsCrasher(__instance.field_Private_String_0))
            {
                Logging.Gottem($"Crasher blocked: AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0 (avatar ID: {__instance.field_Private_String_0}, URL: {__instance.field_Private_String_1})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                __instance.field_Private_String_0 = BundleBouncer.BLOCKED_AVTR_ID;
                __instance.field_Private_String_1 = BundleBouncer.BLOCKED_FILE_URL;
            }
            return true;
        }

        private static bool OnCheckIfAssetBundleFileTooLarge(ContentType __0, string __1, ref bool __result)
        {
            var contentType = __0;
            var fileName = __1;
            //var fileSize = __2;
            Logging.Info($"ValidationHelpers.CheckIfAssetBundleFileTooLarge({contentType}, {fileName})");
            return true;
        }

        //public static void DownloadFile(string url, Il2CppSystem.Action<Il2CppStructArray<byte>> onSuccess, Il2CppSystem.Action<string> onError, Il2CppSystem.Action<long, long> onProgress);
        private static bool OnAPIFileDownloadFile(string __0, ref Il2CppSystem.Action<Il2CppStructArray<byte>> __1, Il2CppSystem.Action<string> __2, Il2CppSystem.Action<long, long> __3)
        {
            var url = __0;
            var onSuccess = __1;
            var orig_onSuccess = __1;
            var onError = __2;
            var onProgress = __3;
            Action<Il2CppStructArray<byte>> p = (data) =>
            {
                byte[] hash;
                using (var sha256 = new SHA256Managed())
                {
                    using (var stream = new MemoryStream(data.ToArray()))
                    {
                        stream.Position = 0;
                        hash = sha256.ComputeHash(stream);
                    }
                }
                string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
                Logging.Info($"Attempting to process ApiFile.DownloadFile onSuccess: (URL: {url}, Hash: {hashstr})");
                if (AvatarShitList.IsBundleACrasher(hash))
                {
                    Logging.Gottem($"Crasher blocked: ApiFile.DownloadFile onSuccess (URL: {url}, Hash: {hashstr})");
                    BBUI.NotifyUser($"Blocked crasher (see log)");
                    onError.Invoke("crasher blocked");
                }
                else
                {
                    orig_onSuccess.Invoke(data);
                }
            };
            onSuccess = p;
            return true;
        }

        //public unsafe static string Method_Private_Static_String_String_String_Int32_String_String_String_0(string param_0, string param_1, int param_2, string param_3, string param_4, string param_5)

        private static bool OnCreateAssetBundleDownload(string __0, string __1, int __2, string __3, string __4, string __5, ref string __result)
        {
            string uri = __0;
            string itemid = __1;
            int version = __2;
            string unityversion = __3;
            string category = __4;
            string ext = __5;
            //Logging.Info($"AssetBundleDownload.Method_Private_Static_String_String_String_Int32_String_String_String_0({uri}, {itemid}, {version}, {unityversion}, {category}, {ext})");
            Logging.Info($"Attempting to download assetbundle (URI: {uri}, Item ID: {itemid}, Version: {version}, Unity: {unityversion}, Category: {category}, Extension: {ext})");
            if (ext == "vrca" && AvatarShitList.IsCrasher(itemid))
            {
                Logging.Gottem($"Crasher blocked: OnCreateAssetBundleDownload (URI: {uri}, ItemID: {itemid}, version: {version})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                __result = null;
                return false;
            }
            return true;
        }

        private static bool OnLoadFromCacheOrDownload(string url, Hash128 hash, uint crc, WWW __result)
        {
            //Logging.Info($"WWW.LoadFromCacheOrDownload: {url} (Hash: <{hash}>, CRC: {crc}) [TODO]");
            return true;
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromMemory_Internal(dataPtr, crc);
        }

        private static IntPtr OnLoadFromMemoryAsync_Internal(IntPtr dataPtr, uint crc)
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
            Logging.Info($"Attempting to load memory-resident assetbundle (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromMemoryAsync_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: (memory-resident) (CRC: {crc}, SHA256: {hashstr})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromMemoryAsync_Internal(dataPtr, crc);
        }

        private static IntPtr OnLoadFromStream_Internal(IntPtr streamPtr, uint crc, uint managedBufferSize)
        {
            var stream = new Il2CppSystem.IO.Stream(streamPtr);
            var prevPos = stream.Position;
            stream.Position = 0;
            byte[] hash;
            var sha256 = new Il2CppSystem.Security.Cryptography.SHA256Managed();
            try
            {
                hash = sha256.ComputeHash(stream).ToArray();
            }
            finally
            {
                sha256.Dispose();
            }
            stream.Position = prevPos;
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle from stream (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromStream_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: (stream) (CRC: {crc}, SHA256: {hashstr})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromStream_Internal(streamPtr, crc, managedBufferSize);
        }

        private static IntPtr OnLoadFromStreamAsync_Internal(IntPtr streamPtr, uint crc, uint managedBufferSize)
        {
            var stream = new Il2CppSystem.IO.Stream(streamPtr);
            var prevPos = stream.Position;
            stream.Position = 0;
            byte[] hash;
            var sha256 = new Il2CppSystem.Security.Cryptography.SHA256Managed();
            try
            {
                hash = sha256.ComputeHash(stream).ToArray();
            }
            finally
            {
                sha256.Dispose();
            }
            stream.Position = prevPos;
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle from stream (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromStreamAsync_Internal...");
            if (AvatarShitList.IsBundleACrasher(hash))
            {
                Logging.Gottem($"Crasher blocked: (stream) (CRC: {crc}, SHA256: {hashstr})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromStream_Internal(streamPtr, crc, managedBufferSize);
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromFileAsync_Internal(pathPtr, crc, offset);
        }

        private void PatchHarmony(MethodInfo hookee, string prefix = null, string postfix = null)
        {
            string sig = mkSigFromMethod(hookee);
            if (prefix != null)
            {
                try
                {
                    BundleBouncer.Instance.HarmonyInstance.Patch(hookee,
                        prefix: prefix == null ? null : typeof(Patches).GetMethod(prefix, BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod(),
                        postfix: postfix == null ? null : typeof(Patches).GetMethod(postfix, BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod());
                    if (prefix != null)
                        Logging.Info($"Patched {sig} to {prefix} (prefix)");
                    if (postfix != null)
                        Logging.Info($"Patched {sig} to {postfix} (postfix)");
                }
                catch (Exception e)
                {
                    Logging.Error($"Unable to patch {sig}:");
                    Logging.Error(e.ToString());
                }
            }
        }

        private void CtorHarmony(MethodInfo hookee, string hooker_postfix)
        {
            string sig = mkSigFromMethod(hookee);
            try
            {
                BundleBouncer.Instance.HarmonyInstance.Patch(hookee, null, typeof(Patches).GetMethod(hooker_postfix, BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod());
                Logging.Info($"Patched {sig} to {hooker_postfix} (.ctor postfix)");
            }
            catch (Exception e)
            {
                Logging.Error($"Unable to patch {sig} (.ctor):");
                Logging.Error(e.ToString());
            }
        }

        private string mkSigFromMethod(MethodInfo method)
        {
            string psig = String.Join(",", method.GetParameters().Select(x => x.ParameterType.ToString()));
            return $"{method.DeclaringType}.{method.Name}({psig})";
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
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
                BBUI.NotifyUser($"Blocked crasher (see log)");
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
                    BBUI.NotifyUser($"Blocked crasher: {av.id} ({av.name})");

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

        internal static bool OnEvent(ref EventData __0)
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
                            if (HarmonyUtils.HasProp(avdata, "avatarDict"))
                            {
                                if (!CheckAvDict(avdata["avatarDict"], avdata["user"]["id"], __0.Code, false))
                                    return false;
                            }
                            if (HarmonyUtils.HasProp(avdata, "favatarDict"))
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
                            if (HarmonyUtils.HasProp(avdata, "avatarDict"))
                            {
                                if (!CheckAvDict(avdata["avatarDict"], avdata["user"]["id"], __0.Code, false))
                                    return false;
                            }
                            if (HarmonyUtils.HasProp(avdata, "favatarDict"))
                            {
                                if (!CheckAvDict(avdata["favatarDict"], avdata["user"]["id"], __0.Code, true))
                                    return false;
                            }
                        }
                        break;
                    default:
                        {
                            if (!writtenPhotonSamples.Contains(__0.Code))
                            {
                                string customProps = JsonConvert.SerializeObject(Serialize.FromIL2CPPToManaged<object>(__0.Parameters));
                                if (!customProps.Contains("avtr_"))
                                    break;

                                writtenPhotonSamples.Add(__0.Code);
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
                BundleBouncer.addAssetURL(up["assetUrl"].ToString(), avID, user);
            }
            Logging.Info($"Attempting to download {fbstr} avatar {avID} ({avName} via E{code})...");
            if (AvatarShitList.IsCrasher(avID))
            {
                BundleBouncer.AddToSkiddieShitlist(user);
                var player = BundleBouncer.GetPlayers().Where(x => x.field_Private_APIUser_0.id == user).FirstOrDefault();
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
    }
}
