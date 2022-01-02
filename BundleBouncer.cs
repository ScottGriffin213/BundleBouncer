using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleBouncer.Data;
using MelonLoader;
using VRC.Core;
using VRChatUtilityKit.Utilities;
using System.IO;
using System.Reflection;
using UnhollowerBaseLib;
using System.Runtime.InteropServices;

namespace BundleBouncer
{
    public class BundleBouncer: MelonMod
    {
        public BundleBouncer Instance { get; private set; }

        public string UserDataDir { get { return "UserData/BundleBouncer"; } }
        public string UserAvatarShitListFile { get { return Path.Combine(UserDataDir, "Avatars.txt"); } }

        public override void OnApplicationStart()
        {
            Logging.LI = LoggerInstance;
            Instance = this;

            if(!Directory.Exists(UserDataDir)) {
                Directory.CreateDirectory(UserDataDir);
                Logging.Info($"Created {UserDataDir}");
            }
            if(!File.Exists(UserAvatarShitListFile)) {
                File.WriteAllLines(UserAvatarShitListFile, new string[]{"# Add avatar IDs below this line, but without the #.", ""});
                Logging.Info($"Created {UserAvatarShitListFile}");
            }
            AvatarShitList.UserShitList = File.ReadAllLines(UserAvatarShitListFile).Select(x => x.Trim().ToLowerInvariant()).Where(x => x != "" && !x.StartsWith("#")).ToHashSet();
            if(AvatarShitList.UserShitList.Count > 0) {
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
                Logging.Info($"Hooked AssetBundleManager.");
            }
        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AttemptAvatarDownloadDelegate(IntPtr hiddenValueTypeReturn, IntPtr thisPtr, IntPtr apiAvatarPtr, IntPtr multicastDelegatePtr, bool idfk, IntPtr nativeMethodInfo);

        private static AttemptAvatarDownloadDelegate dgAttemptAvatarDownload;

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

        // I have no idea what I'm doing
        static unsafe IntPtr OnAttemptAvatarDownload(IntPtr hiddenStructReturn, IntPtr thisPtr, IntPtr pApiAvatar, IntPtr pMulticastDelegate, bool param_3, IntPtr nativeMethodInfo)
        {
            using (var ctx = new AttemptAvatarDownloadContext(pApiAvatar == IntPtr.Zero ? null : new ApiAvatar(pApiAvatar)))
            {
                var av = AttemptAvatarDownloadContext.apiAvatar;
                Logging.Info($"Attempting to download avatar {av.id} ({av.name})...");
                if (AvatarShitList.IsCrasher(av.id))
                {
                    Logging.Info($"Crasher blocked: {av.id} ({av.name}) -> {av.unityPackageUrl} (OnAttemptAvatarDownload)");
                    
                    av.assetUrl = "http://0.0.0.0/doesnt-exist.asset"; // TODO: Make configurable, or point to big blaring CRASHER avatar?
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
    }
}
