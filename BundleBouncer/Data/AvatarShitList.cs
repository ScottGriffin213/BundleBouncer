/**
 * BundleBouncer Avatar Shitlists and Whitelists
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

using System.Collections.Generic;

namespace BundleBouncer.Data
{
    public class AvatarShitList
    {
        // Loaded from UserData/BundleBouncer/My-Blocked-Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserAvatarShitList;

        // Loaded from UserData/BundleBouncer/My-Allowed-Avatars.txt in BundleBouncer.OnApplicationStart()
        public static HashSet<string> UserAvatarAllowList;

        public static HashSet<byte[]> UserAssetBundleWhitelist;
        public static HashSet<byte[]> UserAssetBundleShitlist;

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

        internal static bool IsAssetBundleHashWhitelisted(byte[] hash)
        {
            // Check against global whitelist.  Shouldn't be needed until we start checking shit with YARA/AssetUtils...
            if (shitListProvider.IsAssetBundleHashWhitelisted(hash))
                return true;

            if(UserAssetBundleWhitelist.Contains(hash))
                return true;

            return false;
        }

        internal static bool IsAssetBundleHashBlocked(byte[] hash)
        {
            if(shitListProvider.IsAssetBundleHashBlacklisted(hash))
                return true;
            if(UserAssetBundleShitlist.Contains(hash))
                return true;
            return false;
        }
    }
}