using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace BundleBouncer
{
    internal class CacheTool
    {
        internal static string GetCachePathBase(string fileId, int fileVersion)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), GetCacheNameOfFileID(fileId), GetCacheVersionFolder(fileVersion));
        }
        internal static string GetRawCachePathBase(string cacheKey, int cacheVersion)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), cacheKey, GetCacheVersionFolder(cacheVersion));
        }

        private static Cache _GetCache()
        {
            return AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Cache_0;
        }

        private static readonly byte[] INT_ZERO = new byte[] {0, 0, 0, 0};

        private static string GetCacheVersionFolder(int fileVersion)
        {
            return String.Concat((new int[] { 0, 0, 0, fileVersion }).Select(x=>String.Concat(BitConverter.GetBytes(x).Select(y=>y.ToString("X2")))));
        }

        private static string GetCacheNameOfFileID(string fileId)
        {
            using(var sha256 = new SHA256Managed())
            {
                return string.Concat(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fileId)).Select(x => x.ToString("X2"))).Substring(0, 16);
            }
        }

        internal static string GetCacheDataPath(string fileId, int fileVersion)
        {
            return Path.Combine(GetCachePathBase(fileId, fileVersion), "__data");
        }

        internal static string GetRawCacheDataPath(string fileKey, int fileVersion)
        {
            return Path.Combine(GetRawCachePathBase(fileKey, fileVersion), "__data");
        }

        internal static string GetRawCacheDataPath(string fileKey, Hash128 fileHash)
        {
            return Path.Combine(GetRawCachePathBase(fileKey, fileHash), "__data");
        }

        private static string GetRawCachePathBase(string fileKey, Hash128 fileHash)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), fileKey, fileHash.ToString());
        }

        // https://github.com/gompoc/VRChatMods/blob/f3e5440b715b7c337cfb521ccbf519dc55e74e74/WorldPredownload/Cache/CacheManager.cs#L98
        internal static void CreateCacheInfoFile(string fileKey, Hash128 fileHash)
        {
            var infoFilename = Path.Combine(GetRawCachePathBase(fileKey, fileHash), "__info");
            File.WriteAllText(infoFilename, $"-1\n{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}\n1\n__data\n");
        }
    }
}