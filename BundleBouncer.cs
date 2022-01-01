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

            // Thanks be to the skiddies, from whomst I have "borrowed" this from.
            foreach (var methodInfo in typeof(AssetBundleDownloadManager).GetMethods().Where(p => p.GetParameters().Length == 1 && p.GetParameters().First().ParameterType == typeof(ApiAvatar) && p.ReturnType == typeof(void)))
            {
                HarmonyInstance.Patch(methodInfo, MelonUtils.ToNewHarmonyMethod(typeof(BundleBouncer).GetMethod(nameof(OnAvatarAssetBundleDownloadAttempt), System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Static)));
            }

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
        }

        static bool OnAvatarAssetBundleDownloadAttempt(ApiAvatar __0)
        {
            if(__0==null)
            {
                Logging.Warning("ApiAvatar was null...?!");
                return true; // Apparently worked before...
            }
            Logging.Info($"Attempting to download avatar {__0.id} ({__0.name})...");
            if (AvatarShitList.IsCrasher(__0.id))
            {
                Logging.Info($"Crasher blocked: {__0.id} ({__0.name}) -> {__0.unityPackageUrl} (via nullroute)");
                __0.unityPackageUrl = "https://0.0.0.0/badavi.unityasset"; // nullroute, should trigger an error avatar.
            }
            return true;
        }
    }
}
