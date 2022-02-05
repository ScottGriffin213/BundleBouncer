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
    }
}