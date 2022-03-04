/**
 * BundleBouncer Detours and Patches
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
using BundleBouncer.Model;
using BundleBouncer.Utilities;
using ExitGames.Client.Photon;
using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BundleBouncer
{
    class Patches
    {
        private BundleBouncer bb;

        /// <summary>
        /// Only used when we find an event previously unknown to us transmitting avatar IDs.
        /// </summary>
        public static HashSet<byte> writtenPhotonSamples = new HashSet<byte>();

        // Mostly for logging and datastream tracking.
        static Dictionary<string, AssetInfo> assetInfo = new Dictionary<string, AssetInfo>();
        private static Dictionary<string, string> avIDsByFileSubURL = new Dictionary<string, string>();

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

        private static Dictionary<string, DateTime> scannedGameObjects = new Dictionary<string, DateTime>();

        private static Dictionary<IntPtr, AssetBundleInterceptor> intercepts = new Dictionary<IntPtr, AssetBundleInterceptor>();
        private static Dictionary<IntPtr, AssetBundleInterceptor> uwrLookup = new Dictionary<IntPtr, AssetBundleInterceptor>();

        public Patches(BundleBouncer bundleBouncer)
        {
            bb = bundleBouncer;
            unsafe
            {
                // God I hate pointers.
                var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                    .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(typeof(AssetBundleDownloadManager).GetMethod(
                        nameof(AssetBundleDownloadManager.Method_Internal_UniTask_1_InterfacePublicAbstractIDisposableGaObGaUnique_ApiAvatar_MulticastDelegateNInternalSealedVoUnUnique_Boolean_0)))
                    .GetValue(null);

                MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), typeof(Patches).GetMethod(nameof(Patches.OnAttemptAvatarDownload), BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer());

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
                            case 1: HarmonyPatchMethod(method, nameof(OnLoadFromFile_1)); break;
                                //case 2: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_2)); break;
                                //case 3: StaticHarmony(method, nameof(BundleBouncer.OnLoadFromFile_3)); break;
                        }
                        break;
                    //public static AssetBundle LoadFromFile_Internal(string path, uint crc, ulong offset);
                    case "LoadFromFile_Internal":
                        HarmonyPatchMethod(method, nameof(OnLoadFromFile_Internal));
                        break;
                }
            }

            HarmonyPatchMethod(typeof(VRCNetworkingClient).GetMethod(nameof(VRCNetworkingClient.OnEvent)), nameof(OnEvent));

            // This is the right one, but ML doesn't handle ctors properly ;_;
            //HarmonyPatchMethod(typeof(UnityWebRequest).GetConstructor(new Type[] { typeof(string), typeof(string) }), nameof(OnUnityWebRequest_CtorStrStr));
            //HarmonyPatchMethod(typeof(UnityWebRequest).GetConstructor(new Type[] { typeof(string), typeof(string), typeof(DownloadHandler), typeof(UploadHandler) }), prefix: nameof(OnUnityWebRequest_CtorStrStrDHUH));

            // Both of these trigger a mono implement-me assertion.
            // var tUnityWebRequestAssetBundle = typeof(UnityWebRequestAssetBundle);
            // var nGetAssetBundle = nameof(UnityWebRequestAssetBundle.GetAssetBundle);
            // HarmonyPatchMethod(tUnityWebRequestAssetBundle.GetMethod(nGetAssetBundle, new Type[] { typeof(string) }), prefix: nameof(OnUnityWebRequestAssetBundle_Pre_GetAssetBundle_Str));
            // HarmonyPatchMethod(tUnityWebRequestAssetBundle.GetMethod(nGetAssetBundle, new Type[] { typeof(string), typeof(CachedAssetBundle), typeof(uint) }), prefix: nameof(OnUnityWebRequestAssetBundle_Pre_GetAssetBundle_StrCabUi));

            //HarmonyPatchMethod(typeof(UnityWebRequest).GetMethod(nameof(UnityWebRequest.GetDownloadProgress)), prefix: nameof(OnUnityWebRequest_GetDownloadProgress));
            HarmonyPatchMethod(typeof(UnityWebRequest).GetProperty(nameof(UnityWebRequest.downloadProgress)).GetMethod, prefix: nameof(OnUnityWebRequest_get_DownloadProgress));
            HarmonyPatchMethod(typeof(UnityWebRequest).GetProperty(nameof(UnityWebRequest.downloadedBytes)).GetMethod, prefix: nameof(OnUnityWebRequest_get_DownloadedBytes));

            HarmonyPatchMethod(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Private_Static_String_String_String_Int32_String_String_String_0)), nameof(OnCreateAssetBundleDownload));
            //PatchHarmony(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Private_Static_String_String_0), AccessTools.all), nameof(OnUnknownStringHook1_pre), nameof(OnUnknownStringHook1_post));
            HarmonyPatchMethod(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableAsObAsUnique_0)), nameof(OnGetAssetBundleGetter));
            HarmonyPatchMethod(typeof(AssetBundleDownload).GetMethod(nameof(AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0)), nameof(OnGetGameObjectGetter));

            HarmonyPatchMethod(typeof(ApiFile).GetMethod(nameof(ApiFile.DownloadFile)), prefix: nameof(OnAPIFileDownloadFile));

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

            var mods = Process.GetCurrentProcess().Modules.Cast<ProcessModule>();
            var modUnityPlayer = mods.Where(x => x.ModuleName.Equals("UnityPlayer.dll")).First();
            unsafe
            {
                UsingFunctionInModule(modUnityPlayer, Constants.Offsets.UnityPlayer.CORE_BASICSTRING_CHAR_CSTR, out UnityCoreUtils.origNATIVECoreBasicString_CStr);
                UsingFunctionInModule(modUnityPlayer, Constants.Offsets.UnityPlayer.CORE_STRINGSTORAGEDEFAULT_CHAR_ASSIGN, out UnityCoreUtils.origNATIVECoreStringStorageDefault_Char_Assign);
                UsingFunctionInModule(modUnityPlayer, Constants.Offsets.UnityPlayer.CORE_STRINGSTORAGEDEFAULT_WCHART_DEALLOCATE, out UnityCoreUtils.origNATIVECoreStringStorageDefault_WCharT_Deallocate);
                UsingFunctionInModule(modUnityPlayer, Constants.Offsets.UnityPlayer.HEADERMAP_FIND, out origNATIVEHeaderMap_find);

                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.LOADFROMFILE, OnUnityPlayer_LoadFromFile_NATIVE, out origNATIVELoadFromFile);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.LOADFROMFILEASYNC, OnUnityPlayer_LoadFromFileAsync_NATIVE, out origNATIVELoadFromFileAsync);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.LOADFROMMEMORY, OnUnityPlayer_LoadFromMemory_NATIVE, out origNATIVELoadFromMemory);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.ASSETBUNDLELOADFROMASYNCOPERATION_INITIALIZEASSETBUNDLESTORAGE_FSEULONGBOOL, OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_FSEUlongBool, out origNATIVEInitAssetBundleStorageFSEUlongBool);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.ASSETBUNDLELOADFROMASYNCOPERATION_INITIALIZEASSETBUNDLESTORAGE_STRULONG, OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_StrUlong, out origNATIVEInitAssetBundleStorageStrUlong);

                // ABI shit
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_CREATECACHED, OnDownloadHandlerAssetBundle_CreateCached, out origNATIVEDownloadHandlerAssetBundle_CreateCached);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_GETPROGRESS, OnDownloadHandlerAssetBundle_GetProgress, out origNATIVEDownloadHandlerAssetBundle_GetProgress);
                //PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_GETMEMORYSIZE, OnDownloadHandlerAssetBundle_GetMemorySize, out origNATIVEDownloadHandlerAssetBundle_GetMemorySize);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_ISDONE, OnDownloadHandlerAssetBundle_IsDone, out origNATIVEDownloadHandlerAssetBundle_IsDone);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_ONCOMPLETECONTENT, OnDownloadHandlerAssetBundle_OnCompleteContent, out origNATIVEDownloadHandlerAssetBundle_OnCompleteContent);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLERASSETBUNDLE_ONRECEIVEDATA, OnDownloadHandlerAssetBundle_OnReceiveData, out origNATIVEDownloadHandlerAssetBundle_OnReceiveData);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLER_PROCESSHEADERS, OnDownloadHandler_ProcessHeaders, out origNATIVEDownloadHandler_ProcessHeaders);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.DOWNLOADHANDLER_HASCONTENTLENGTH, OnDownloadHandler_HasContentLength, out origNATIVEDownloadHandler_HasContentLength);

                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.UNITYWEBREQUEST_BEGINWEBREQUEST, OnUnityWebRequest_BeginWebRequest, out origNATIVEUnityWebRequest_BeginWebRequest);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.UNITYWEBREQUEST_GETDOWNLOADEDBYTES, OnUnityWebRequest_GetDownloadedBytes, out origNATIVEUnityWebRequest_GetDownloadedBytes);
                PatchModule(modUnityPlayer, Constants.Offsets.UnityPlayer.UNITYWEBREQUEST_GETDOWNLOADPROGRESS, OnUnityWebRequest_GetDownloadProgress, out origNATIVEUnityWebRequest_GetDownloadProgress);
            }
        }

        #region Infrastructure
        private static unsafe void UsingFunctionInModule<T>(ProcessModule module, int offset, out T delegateField) where T : MulticastDelegate
        {
            Logging.Info($"Attaching to {module.ModuleName} + 0x{offset:X8}");
            delegateField = null;
            if (offset == 0)
                return;
            var ptr = module.BaseAddress + offset;
            delegateField = Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        // Roughly based off of Knah's code: https://github.com/knah/VRCMods/blob/9ad060a8aa05c1454696f2625ad6a857fec1fed6/AdvancedSafety/ReaderPatches.cs#L84
        private static unsafe void PatchModule<T>(ProcessModule module, int offset, T patchDelegate, out T delegateField) where T : MulticastDelegate
        {
            Logging.Info($"Patching {module.ModuleName} + 0x{offset:X8}");
            delegateField = null;
            if (offset == 0)
                return;
            var ptr = module.BaseAddress + offset;
            MelonUtils.NativeHookAttach((IntPtr)(&ptr), Marshal.GetFunctionPointerForDelegate(patchDelegate));
            delegateField = Marshal.GetDelegateForFunctionPointer<T>(ptr);
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
        private static unsafe IntPtr Il2CPPPatchMethod(MethodBase target, string detour)
        {
            string psig = String.Join(",", target.GetParameters().Select(x => x.ParameterType.ToString()));
            string sig = $"{target.DeclaringType}.{target.Name}({psig})";
            var originalMethodPointer = *(IntPtr*)UnhollowerSupport.MethodBaseToIl2CppMethodInfoPointer(target);
            var detourPointer = typeof(Patches).GetMethod(detour, BindingFlags.Static | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
            MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), detourPointer);
            Logging.Info($"Natively attached {target.Name} to {detour}");
            return originalMethodPointer;
        }

        private void HarmonyPatchMethod(MethodBase hookee, string prefix = null, string postfix = null)
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

        private string mkSigFromMethod(MethodBase method)
        {
            string psig = String.Join(",", method.GetParameters().Select(x => x.ParameterType.ToString()));
            return $"{method.DeclaringType}.{method.Name}({psig})";
        }
        #endregion

        #region UnityPlayer.dll Tomfoolery
        /*
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OnUnityPlayer_LoadFromFile_Delegate(IntPtr a1, IntPtr filenamePtr, int a3, uint crc);
        private static OnUnityPlayer_LoadFromFile_Delegate origNATIVELoadFromFile;
        private static unsafe void OnUnityPlayer_LoadFromFile_NATIVE(IntPtr a1, IntPtr filenamePtr, int a3, uint crc)
        {
            string filename = UnityCoreUtils.CoreBasicString2String(filenamePtr);
            Logging.Info($"UnityPlayer::LoadFromFile - {filename}");
            origNATIVELoadFromFile(a1, filenamePtr, a3, crc);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OnUnityPlayer_LoadFromFileAsync_Delegate(IntPtr a1, IntPtr filenamePtr, int a3, uint crc);
        private static OnUnityPlayer_LoadFromFileAsync_Delegate origNATIVELoadFromFileAsync;
        private static unsafe void OnUnityPlayer_LoadFromFileAsync_NATIVE(IntPtr a1, IntPtr filenamePtr, int a3, uint crc)
        {
            string filename = UnityCoreUtils.CoreBasicString2String(filenamePtr);
            Logging.Info($"UnityPlayer::LoadFromFileAsync - {filename}");
            origNATIVELoadFromFileAsync(a1, filenamePtr, a3, crc);
        }
        */

        /* Not really useful, but interesting.
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_FSEUlongBool_Delegate(IntPtr @this, IntPtr fse, ulong a2, bool a3);
        private static OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_FSEUlongBool_Delegate origNATIVEInitAssetBundleStorageFSEUlongBool;
        private static unsafe void OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_FSEUlongBool(IntPtr @this, IntPtr fse, ulong a2, bool a3)
        {
            Logging.Info($"UnityPlayer::AssetBundleLoadFromAsyncOperation::InitializeAssetBundleStorage(FSE, {a2}, {a3})");
            origNATIVEInitAssetBundleStorageFSEUlongBool(@this, fse, a2, a3);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_StrUlong_Delegate(IntPtr @this, IntPtr a1, bool a2);
        private static OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_StrUlong_Delegate origNATIVEInitAssetBundleStorageStrUlong;
        private static unsafe IntPtr OnAssetBundleLoadFromAsyncOperation_InitializeAssetBundleStorage_StrUlong(IntPtr @this, IntPtr a1, bool a2)
        {
            string filename = UnityCoreUtils.CoreBasicString2String(a1);
            Logging.Info($"UnityPlayer::AssetBundleLoadFromAsyncOperation::InitializeAssetBundleStorage({filename}, {a2})");
            return origNATIVEInitAssetBundleStorageStrUlong(@this, a1, a2);
        }
        */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr OnDownloadHandlerAssetBundle_CreateCached_Delegate(IntPtr scriptingObjectPtr, IntPtr urlPtr, IntPtr keyPtr, Hash128 hash, uint crc);
        private static OnDownloadHandlerAssetBundle_CreateCached_Delegate origNATIVEDownloadHandlerAssetBundle_CreateCached;
        private static unsafe IntPtr OnDownloadHandlerAssetBundle_CreateCached(IntPtr scriptingObjectPtr, IntPtr urlPtr, IntPtr keyPtr, Hash128 hash, uint crc)
        {
            string url = UnityCoreUtils.CoreBasicString2String(urlPtr);
            string key = UnityCoreUtils.CoreBasicString2String(keyPtr);
            Logging.Info($"UnityPlayer::DownloadHandlerAssetBundle::CreateCached(sop, {url}, {key}, {hash}, {crc})");
            var cachedObjPath = CacheTool.GetRawCacheDataPath(key, hash);
            if (!File.Exists(cachedObjPath))
            {
                Logging.Info("Downloading...");
                var dhab = new DownloadHandlerAssetBundle(scriptingObjectPtr);
                //Logging.Info("IDHAB created.");
                var idhab = new AssetBundleInterceptor(url, "GET", cachedObjPath, dhab, key, hash, crc);
                var o = origNATIVEDownloadHandlerAssetBundle_CreateCached(scriptingObjectPtr, urlPtr, keyPtr, hash, crc);
                idhab.ptr = o;
                Patches.intercepts[o] = idhab;
                //Logging.Info($"Assigned to [{o.ToInt64()}]");
                return o;
            }
            /*
            if (AssetScanner.ScanFile(cachedObjPath, "UnityPlayer::DownloadHandlerAssetBundle::CreateCached"))
            {
                return IntPtr.Zero;
            }
            */
            return origNATIVEDownloadHandlerAssetBundle_CreateCached(scriptingObjectPtr, urlPtr, keyPtr, hash, crc);
        }
        /*
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate double OnDownloadHandlerAssetBundle_GetProgress_Delegate(IntPtr @this);
        private static OnDownloadHandlerAssetBundle_GetProgress_Delegate origNATIVEDownloadHandlerAssetBundle_GetProgress;
        private static unsafe double OnDownloadHandlerAssetBundle_GetProgress(IntPtr @this)
        {
            //Logging.Info($"OnDownloadHandlerAssetBundle_GetProgress[{@this.ToInt64()}]");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                return idhab.GetProgress();
            }
            var o = origNATIVEDownloadHandlerAssetBundle_GetProgress(@this);
            //Logging.Info($"Native GetProgress: {o}");
            return o;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong OnDownloadHandlerAssetBundle_GetMemorySize_Delegate(IntPtr @this);
        private static OnDownloadHandlerAssetBundle_GetMemorySize_Delegate origNATIVEDownloadHandlerAssetBundle_GetMemorySize;
        private static unsafe ulong OnDownloadHandlerAssetBundle_GetMemorySize(IntPtr @this)
        {
            //Logging.Info($"OnDownloadHandlerAssetBundle_GetMemorySize[{@this.ToInt64()}]");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                return idhab.GetMemorySize();
            }
            return origNATIVEDownloadHandlerAssetBundle_GetMemorySize(@this);
        }
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate char OnDownloadHandlerAssetBundle_IsDone_Delegate(IntPtr @this);
        private static OnDownloadHandlerAssetBundle_IsDone_Delegate origNATIVEDownloadHandlerAssetBundle_IsDone;
        private static unsafe char OnDownloadHandlerAssetBundle_IsDone(IntPtr @this)
        {
            //Logging.Info($"OnDownloadHandlerAssetBundle_IsDone[{@this.ToInt64()}]");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                return (char)(idhab.IsDone ? 0x01 : 0x00);
            }
            return origNATIVEDownloadHandlerAssetBundle_IsDone(@this);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void OnDownloadHandlerAssetBundle_OnCompleteContent_Delegate(IntPtr @this);
        private static OnDownloadHandlerAssetBundle_OnCompleteContent_Delegate origNATIVEDownloadHandlerAssetBundle_OnCompleteContent;
        private static unsafe void OnDownloadHandlerAssetBundle_OnCompleteContent(IntPtr @this)
        {
            //Logging.Info($"OnDownloadHandlerAssetBundle_OnCompleteContent[{@this.ToInt64()}]");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                idhab.OnCompleteContent();
            }
            else
            {
                origNATIVEDownloadHandlerAssetBundle_OnCompleteContent(@this);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool OnDownloadHandler_HasContentLength_Delegate(IntPtr @this);
        private static OnDownloadHandler_HasContentLength_Delegate origNATIVEDownloadHandler_HasContentLength;
        private static unsafe bool OnDownloadHandler_HasContentLength(IntPtr @this)
        {
            //Logging.Info($"OnDownloadHandle_HasContentLength[{@this.ToInt64()}]");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                return idhab.HasContentLength();
            }
            else
            {
                return origNATIVEDownloadHandler_HasContentLength(@this);
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr OnHeaderMap_find_Delegate(IntPtr @this, IntPtr ustr);
        private static OnHeaderMap_find_Delegate origNATIVEHeaderMap_find;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long OnDownloadHandler_ProcessHeaders_Delegate(IntPtr @this, IntPtr hdrmap);
        private static OnDownloadHandler_ProcessHeaders_Delegate origNATIVEDownloadHandler_ProcessHeaders;
        private static unsafe long OnDownloadHandler_ProcessHeaders(IntPtr @this, IntPtr hdrmap)
        {
            //Logging.Info($"OnDownloadHandler_ProcessHeaders[{@this.ToInt64()}]");
            /*
            var arr1 = Marshal.PtrToStructure<IntPtr[]>(hdrmap);
            Logging.Info(JsonConvert.SerializeObject(arr1));
            var keys = Marshal.PtrToStructure<string[]>(arr1[0]);
            Logging.Info(JsonConvert.SerializeObject(keys));
            var values = Marshal.PtrToStructure<string[]>(arr1[2]);
            Logging.Info(JsonConvert.SerializeObject(values));
            */

            var o = origNATIVEDownloadHandler_ProcessHeaders(@this, hdrmap);
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                ulong clen = *(ulong*)(@this + 0x48);
                //Logging.Info($"  Got Content-Length: {clen}");
                var ctype = UnityCoreUtils.CoreBasicString2String(@this + 0x58);
                //Logging.Info($"  Got Content-Type: {ctype}");
                idhab.ProcessHeaders(clen, ctype);
                //Logging.Info($"ProcessHeaders: ret {o} I");
            }
            else
            {
                //Logging.Info($"ProcessHeaders: ret {o}");
            }
            // Passthru to original DH.
            return o;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate long OnDownloadHandlerAssetBundle_OnReceiveData_Delegate(IntPtr @this, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] data, ulong len);
        private static OnDownloadHandlerAssetBundle_OnReceiveData_Delegate origNATIVEDownloadHandlerAssetBundle_OnReceiveData;
        private static unsafe long OnDownloadHandlerAssetBundle_OnReceiveData(IntPtr @this, byte[] data, ulong len)
        {
            //Logging.Info($"OnDownloadHandlerAssetBundle_OnReceiveData[{@this.ToInt64()}]({data.Length}, {len})");
            if (intercepts.TryGetValue(@this, out AssetBundleInterceptor idhab))
            {
                return idhab.OnReceiveData(data, len);
            }
            else
            {
                return origNATIVEDownloadHandlerAssetBundle_OnReceiveData(@this, data, len);
            }
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate IntPtr UnityWebRequest_BeginWebRequestDelegate(IntPtr @this, IntPtr a2, IntPtr a3);
        private static UnityWebRequest_BeginWebRequestDelegate origNATIVEUnityWebRequest_BeginWebRequest;
        private static unsafe IntPtr OnUnityWebRequest_BeginWebRequest(IntPtr @this, IntPtr a2, IntPtr a3)
        {
            var dnuwr = new UnityWebRequest(a2);
            Logging.Info($"UnityWebRequest::BeginWebRequest(this=[{@this}], a2={dnuwr}, a3={a3})");
            if (dnuwr.downloadHandler != null && intercepts.ContainsKey(dnuwr.downloadHandler.Pointer))
            {
                var abi =  intercepts[dnuwr.downloadHandler.Pointer];
                abi.webRequest = dnuwr;
                abi.cpp_uwr = @this;
                abi.OnBeginWebRequest();
                uwrLookup[@this] = abi;
            }
            return origNATIVEUnityWebRequest_BeginWebRequest(@this, a2, a3);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate ulong UnityWebRequest_GetDownloadedBytesDelegate(IntPtr @this);
        private static UnityWebRequest_GetDownloadedBytesDelegate origNATIVEUnityWebRequest_GetDownloadedBytes;
        private static unsafe ulong OnUnityWebRequest_GetDownloadedBytes(IntPtr @this)
        {
            Logging.Info($"UnityWebRequest::GetDownloadedBytes(this=[{@this}])");
            if (uwrLookup.TryGetValue(@this, out AssetBundleInterceptor abi))
            {
                return abi.GetMemorySize();
            }
            return origNATIVEUnityWebRequest_GetDownloadedBytes(@this);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private unsafe delegate float UnityWebRequest_GetDownloadProgressDelegate(IntPtr @this);
        private static UnityWebRequest_GetDownloadProgressDelegate origNATIVEUnityWebRequest_GetDownloadProgress;
        private static unsafe float OnUnityWebRequest_GetDownloadProgress(IntPtr @this)
        {
            Logging.Info($"UnityWebRequest::GetDownloadProgress(this=[{@this}])");
            if (uwrLookup.TryGetValue(@this, out AssetBundleInterceptor abi))
            {
                return (float)abi.GetProgress();
            }
            return origNATIVEUnityWebRequest_GetDownloadProgress(@this);
        }
        #endregion

        internal static void SendDelayedDHABSignals(IntPtr dhab, byte[] data, ulong dataLen)
        {
            //Logging.Info($"SendDelayedDHABSignals[{dhab.ToInt64()}](data.length: {data.Length}, dataLen: {dataLen})");
            //dhab.ReceiveContentLengthHeader(0);

            //dhab.ReceiveData(new Il2CppStructArray<byte>(0), 0);
            origNATIVEDownloadHandlerAssetBundle_OnReceiveData(dhab, data, dataLen);

            //dhab.CompleteContent();
            origNATIVEDownloadHandlerAssetBundle_OnCompleteContent(dhab);
        }

        public void OnLateUpdate()
        {
            lock (scannedGameObjects)
            {
                foreach (var kvp in new Dictionary<string, DateTime>(scannedGameObjects))
                {
                    if (kvp.Value <= DateTime.Now)
                        scannedGameObjects.Remove(kvp.Key);
                }
            }
            lock (intercepts)
            {
                lock (uwrLookup)
                {
                    foreach (var intercept in new List<AssetBundleInterceptor>(intercepts.Values))
                    {
                        if (intercept.IsDone)
                        {
                            intercepts.Remove(intercept.ptr);
                            uwrLookup.Remove(intercept.cpp_uwr);
                        }
                    }
                }
            }
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
            if (__instance.field_Private_ContentType_0 == ContentType.Avatar && AvatarShitList.IsAvatarIDBlocked(__instance.field_Private_String_0))
            {
                BundleBouncer.NotifyUserOfBlockedAvatar(__instance.field_Private_String_0, "OnGetGameObjectGetter", new Dictionary<string, string>(){
                    {"URL", __instance.field_Private_String_1},
                });
                __instance.field_Private_String_0 = BundleBouncer.BLOCKED_AVTR_ID;
                __instance.field_Private_String_1 = BundleBouncer.BLOCKED_FILE_URL;
            }
            if (ApiFile.TryParseFileIdAndVersionFromFileAPIUrl(__instance.field_Private_String_1, out string fileId, out int fileVersion))
            {
                if (!scannedGameObjects.ContainsKey(__instance.field_Private_String_1))
                {
                    scannedGameObjects[__instance.field_Private_String_1] = DateTime.Now + new TimeSpan(0, 5, 0);
                    var bpath = CacheTool.GetCacheDataPath(fileId, fileVersion);
                    //Logging.Info($"AssetBundleDownload.Method_Public_InterfacePublicAbstractIDisposableGaObGaUnique_0(): fileID: {fileId}, fileVersion: {fileVersion}, bpath: {bpath}");
                    if (File.Exists(bpath) && AvatarShitList.IsAssetBundleHashBlocked(IOTool.SHA256File(bpath)))
                    {
                        BundleBouncer.NotifyUserOfBlockedAvatar(__instance.field_Private_String_0, "OnGetGameObjectGetter", new Dictionary<string, string>(){
                            {"URL", __instance.field_Private_String_1},
                            {"BPath", bpath},
                        });
                        __instance.field_Private_String_0 = BundleBouncer.BLOCKED_AVTR_ID;
                        __instance.field_Private_String_1 = BundleBouncer.BLOCKED_FILE_URL;
                    }
                }
            }
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
            if (__instance.field_Private_ContentType_0 == ContentType.Avatar && AvatarShitList.IsAvatarIDBlocked(__instance.field_Private_String_0))
            {
                BundleBouncer.NotifyUserOfBlockedAvatar(__instance.field_Private_String_0, "OnGetAssetBundleGetter", new Dictionary<string, string>(){
                    {"URL", __instance.field_Private_String_1},
                });
                __instance.field_Private_String_0 = BundleBouncer.BLOCKED_AVTR_ID;
                __instance.field_Private_String_1 = BundleBouncer.BLOCKED_FILE_URL;
            }
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
                if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
            if (ext == "vrca" && AvatarShitList.IsAvatarIDBlocked(itemid))
            {
                BundleBouncer.NotifyUserOfBlockedAvatar(itemid, "OnCreateAssetBundleDownload", new Dictionary<string, string>(){
                    {"URI", uri},
                    {"Version", version.ToString()},
                });
                __result = null;
                return false;
            }
            return true;
        }

        // Appears to only really be used in mods.
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
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
            byte[] hash = IOTool.SHA256File(path);
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr}) via AssetBundle.LoadFromMemory_Internal...");
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, Offset: {offset}, SHA256: {hashstr})");
                BBUI.NotifyUser($"Blocked crasher (see log)");
                // TODO - This is probably a bad idea. Swap with internal avatar, mayhaps?
                return IntPtr.Zero;
            }
            return origLoadFromFileAsync_Internal(pathPtr, crc, offset);
        }

        private static bool OnLoadFromFile_1(string __0, ref AssetBundle __result)
        {
            string path = __0;
            byte[] hash = IOTool.SHA256File(path);
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (SHA256: {hashstr}) via AssetBundle.LoadFromFile(string)...");
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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

            byte[] hash = IOTool.SHA256File(path);
            string hashstr = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"Attempting to load assetbundle {path} (CRC: {crc}, SHA256: {hashstr}) via AssetBundle.LoadFromFile...");
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
            {
                Logging.Gottem($"Crasher blocked: {path} (CRC: {crc}, SHA256: {hashstr})");
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
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
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
                if (AvatarShitList.IsAvatarIDBlocked(av.id))
                {
                    BundleBouncer.NotifyUserOfBlockedAvatar(av.id, "AvatarDownloadManager", new Dictionary<string, string> {
                        { "Avatar Name", av.name }
                    });
                    // TODO: Make configurable, or point to big blaring CRASHER avatar?
                    //av.assetUrl = "http://0.0.0.0/doesnt-exist.asset";
                    av.assetUrl = null; // This supposedly returns faster.
                    av.version = int.MaxValue; // Ensure this doesn't get overwritten. :3c
                }
                return dgAttemptAvatarDownload(hiddenStructReturn, thisPtr, pApiAvatar, pMulticastDelegate, param_3, nativeMethodInfo);
            }
        }

        static unsafe bool OnUnityWebRequest_get_DownloadProgress(UnityWebRequest __instance, ref float __result)
        {
            var uwr = __instance;
            if (intercepts.ContainsKey(uwr.downloadHandler.Pointer))
            {
                __result = (float)intercepts[uwr.downloadHandler.Pointer].GetProgress();
                return true;
            }
            __result = 0f;
            return false;
        }

        static unsafe bool OnUnityWebRequest_get_DownloadedBytes(UnityWebRequest __instance, ref ulong __result)
        {
            var uwr = __instance;
            if (intercepts.ContainsKey(uwr.downloadHandler.Pointer))
            {
                __result = intercepts[uwr.downloadHandler.Pointer].GetMemorySize();
                return true;
            }
            __result = 0UL;
            return false;
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

        internal static string Params2JSON(ParameterDictionary paramDict)
        {
            var p = new Dictionary<byte, object>();
            foreach (var kvp in paramDict)
            {
                p[kvp.Key] = Serialize.FromIL2CPPToManaged<object>(kvp.Value);
            }
            return JsonConvert.SerializeObject(p);
        }
        internal static dynamic Params2Dynamic(ParameterDictionary paramDict) => JsonConvert.DeserializeObject(Params2JSON(paramDict));

        internal static bool OnEvent(ref EventData __0)
        {
            try
            {
#if USING_PHOTON_SAMPLER
                PhotonSampler.Sample(__0);
#endif
                switch (__0.Code)
                {
                    case 255: // join
                        {
                            // Fuck it, made a schema.
                            var ev = JsonConvert.DeserializeObject<Photon255>(Params2JSON(__0.Parameters));
                            var avdata = ev._249;
                            var userid = avdata.User.Id; // Beware: In certain situations, this can be spoofed.  Because this is coming from an otherwise authorative source, we have to assume it's the actual user state.

                            if (avdata.AvatarDict != null)
                            {
                                BundleBouncer.SetUserAvatar(userid, EAvatarType.MAIN, avdata.AvatarDict);
                                if (!CheckAvDict(avdata.AvatarDict, userid, __0.Code, false))
                                    return false;
                            }
                            if (avdata.FavatarDict != null)
                            {
                                BundleBouncer.SetUserAvatar(userid, EAvatarType.FALLBACK, avdata.FavatarDict);
                                if (!CheckAvDict(avdata.FavatarDict, userid, __0.Code, true))
                                    return false;
                            }
                        }
                        break;
                    case 253: // properties_changed
                        {
                            var ev = JsonConvert.DeserializeObject<Photon253>(Params2JSON(__0.Parameters));
                            var avdata = ev._251;
                            var userid = avdata.User.Id; // Beware: In certain situations, this can be spoofed.  Because this is coming from an otherwise authorative source, we have to assume it's the actual user state.

                            if (avdata.AvatarDict != null)
                            {
                                BundleBouncer.SetUserAvatar(userid, EAvatarType.MAIN, avdata.AvatarDict);
                                if (!CheckAvDict(avdata.AvatarDict, userid, __0.Code, false))
                                    return false;
                            }
                            if (avdata.FavatarDict != null)
                            {
                                BundleBouncer.SetUserAvatar(userid, EAvatarType.FALLBACK, avdata.FavatarDict);
                                if (!CheckAvDict(avdata.FavatarDict, userid, __0.Code, true))
                                    return false;
                            }
                        }
                        break;
                    default:
                        {
                            if (!writtenPhotonSamples.Contains(__0.Code))
                            {
                                var customProps = Params2JSON(__0.Parameters);
                                if (!customProps.Contains("avtr_"))
                                    break;

                                writtenPhotonSamples.Add(__0.Code);
                                var path = Path.Combine("UserData", "BundleBouncer", $"{__0.Code}.json");
                                File.WriteAllText(path, customProps);
                                path = Path.GetFullPath(path);
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

        private static bool CheckAvDict(Model.Avatar avdata, string user, int code, bool is_fallback)
        {
            string avID = avdata.Id;
            string avName = avdata.Name;
            string fbstr = is_fallback ? "fallback" : "main";
            Logging.Info($"User {user} changed {fbstr} avatar to {avID} ({avName}; via E{code})...");
            if (AvatarShitList.IsAvatarIDBlocked(avID))
            {
                BundleBouncer.NotifyUserOfBlockedAvatar(avID, $"Photon Event {code}", new Dictionary<string, string> {
                    {"Avatar Name", avName },
                    {"Is Fallback", is_fallback ? "yes" : "no" },
                });
                return false;
            }
            return true;
        }
    }


}
