using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BundleBouncer.Data
{
    public class AvatarShitList
    {
        // Loaded from UserData/BundleBouncer/Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserShitList;

        // Loaded from Dependencies/BundleBouncer.Shitlist.dll
        public static IShitListProvider shitListProvider;

        public static bool IsCrasher(string avID)
        {
            // No fun allowed.
            avID = avID.ToLowerInvariant();

            if (avID.StartsWith("local:"))
            {
                // Local test avatar, bypasses rules.
                return false;
            }

            if (shitListProvider.IsAvatarIDWhitelisted(avID))
                return false;

            if (!avID.StartsWith("avtr_"))
            {
                // Handles kaj's weird invalid bullshit.
                // Will probably also trap some very old avs but IDGAF.
                return true;
            }

            // avIDs of crashers hashed so people can't just pull them from the binary.
            byte[] avhash = IOTool.SHA256String(avID);

            if (shitListProvider.IsAvatarIDAnAssetBundleCrasher(avhash))
                return true;

            // Check against user shitlist
            if (UserShitList.Contains(avID))
                return true;

            return false;
        }



        internal static bool IsBundleACrasher(byte[] hash)
        {
            return shitListProvider.IsAssetBundleAnAssetBundleCrasher(hash);
        }
    }
}