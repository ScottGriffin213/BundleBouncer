using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BundleBouncer.Data
{
    public class AvatarShitList
    {
        // Loaded from UserData/BundleBouncer/My-Blocked-Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserAvatarShitList;

        // Loaded from UserData/BundleBouncer/My-Allowed-Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserAvatarAllowList;

        // Loaded from Dependencies/BundleBouncer.Shitlist.dll
        public static IShitListProvider shitListProvider;

        public static bool IsAvatarIDBlocked(string avID)
        {
            // No fun allowed.
            avID = avID.ToLowerInvariant();

            if (avID.StartsWith("local:"))
            {
                // Local test avatar, bypasses rules.
                return false;
            }
            
            // Check against global whitelist.
            if (shitListProvider.IsAvatarIDWhitelisted(avID))
                return false;

            // Check against user whitelist.
            if (UserAvatarAllowList.Contains(avID))
                return false;

            if (!avID.StartsWith("avtr_"))
            {
                // Handles kaj's weird invalid bullshit.
                // Will probably also trap some very old avs but IDGAF.
                return true;
            }

            // avIDs of crashers hashed so people can't just pull them from the binary.
            byte[] avhash = IOTool.SHA256String(avID);

            if (shitListProvider.IsAvatarIDHashBlacklisted(avhash))
                return true;

            // Check against user shitlist
            if (UserAvatarShitList.Contains(avID))
                return true;

            return false;
        }



        internal static bool IsAssetBundleHashBlocked(byte[] hash)
        {
            // Check against global whitelist.  Shouldn't be needed until we start checking shit with YARA/AssetUtils...
            if (shitListProvider.IsAssetBundleHashWhitelisted(hash))
                return false;

            return shitListProvider.IsAssetBundleHashBlacklisted(hash);
        }
    }
}