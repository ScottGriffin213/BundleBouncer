/**
 * BundleBouncer Unity Cache Tools
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

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace BundleBouncer
{
    internal class CacheTool
    {
        private const string DATA_FILE_NAME = "__data";
        private const string INFO_FILE_NAME = "__info";

        internal static string GetCachePathBase(string fileId, int fileVersion)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), GetCacheNameFromFileID(fileId), GetCacheVersionFolder(fileVersion));
        }
        internal static string GetRawCachePathBase(string cacheKey, int cacheVersion)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), cacheKey, GetCacheVersionFolder(cacheVersion));
        }

        private static Cache _GetCache()
        {
            return AssetBundleDownloadManager.field_Private_Static_AssetBundleDownloadManager_0.field_Private_Cache_0;
        }

        private static string GetCacheVersionFolder(int fileVersion)
        {
            return GetCacheVersionFolder(new Hash128(0, 0, 0, (uint)fileVersion));
            //return String.Concat((new int[] { 0, 0, 0, fileVersion }).Select(x => String.Concat(BitConverter.GetBytes(x).Select(y => y.ToString("X2")))));
        }

        private static string GetCacheVersionFolder(Hash128 hash)
        {
            return hash.ToString();
        }

        private static string GetCacheNameFromFileID(string fileId)
        {
            using (var sha256 = new SHA256Managed())
            {
                return string.Concat(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fileId)).Select(x => x.ToString("X2"))).Substring(0, 16);
            }
        }

        internal static string GetCacheDataPath(string fileId, int fileVersion)
        {
            return Path.Combine(GetCachePathBase(fileId, fileVersion), DATA_FILE_NAME);
        }

        internal static string GetRawCacheDataPath(string fileKey, int fileVersion)
        {
            return Path.Combine(GetRawCachePathBase(fileKey, fileVersion), DATA_FILE_NAME);
        }

        internal static string GetRawCacheDataPath(string fileKey, Hash128 fileHash)
        {
            return Path.Combine(GetRawCachePathBase(fileKey, fileHash), DATA_FILE_NAME);
        }

        private static string GetRawCachePathBase(string fileKey, Hash128 fileHash)
        {
            return Path.Combine(Path.GetFullPath(_GetCache().path), fileKey, fileHash.ToString());
        }

        // https://github.com/gompoc/VRChatMods/blob/f3e5440b715b7c337cfb521ccbf519dc55e74e74/WorldPredownload/Cache/CacheManager.cs#L98
        internal static void CreateCacheInfoFile(string fileKey, Hash128 fileHash)
        {
            var infoFilename = Path.Combine(GetRawCachePathBase(fileKey, fileHash), INFO_FILE_NAME);
            File.WriteAllText(infoFilename, $"-1\n{((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()}\n1\n{DATA_FILE_NAME}\n");
        }
    }
}