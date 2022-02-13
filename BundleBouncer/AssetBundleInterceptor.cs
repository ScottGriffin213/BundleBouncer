/**
 * BundleBouncer Asset Bundle Intereceptor
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace BundleBouncer
{
    internal class AssetBundleInterceptor
    {
        private string url;
        private string method;
        private string destfile;
        private FileStream file;
        private DownloadHandlerAssetBundle dhab;
        internal IntPtr ptr;
        private ulong bytesDownloaded;
        private string cacheKey;
        private Hash128 cacheHash;
        private uint crc;
        private bool done;
        private ulong contentLength = 0UL;

        private static HashSet<string> writing = new HashSet<string>();

        public AssetBundleInterceptor(string url, string method, string destfile, DownloadHandlerAssetBundle dhab, string cacheKey, Hash128 cacheHash, uint crc)
        {
            this.url = url;
            this.method = method;
            this.destfile = destfile;
            this.file = null;
            this.dhab = dhab;
            this.ptr = dhab.Pointer;
            this.bytesDownloaded = 0UL;
            this.cacheKey = cacheKey;
            this.cacheHash = cacheHash;
            this.crc = crc;
            this.done = false;
            Logging.Info($"ABI - Intercepting bytes sent to {dhab}...");
        }

        public bool IsDone => done;

        internal void OnCompleteContent()
        {
            done = true;
            file.Dispose();
            CacheTool.CreateCacheInfoFile(cacheKey, cacheHash);
            //dl = null;
            var hash = IOTool.SHA256File(destfile);
            var strhash = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"ABI - Done! Scanning file {destfile} ({strhash})...");
            // Scan local file with YARA and check hash against shitlist.
            if (AssetScanner.ScanFile(destfile, "AssetBundleInterceptor"))
            {
                // Faux download error.
                Patches.SendDelayedDHABSignals(ptr, new byte[0], 0UL);
            }
            else
            {
                Patches.SendDelayedDHABSignals(ptr, File.ReadAllBytes(destfile), contentLength);
            }
            lock (writing)
            {
                writing.Remove(destfile);
            }
        }

        internal void ProcessHeaders(ulong length, string type)
        {
            //Logging.Info($"Content-Length: {length}UL");
            this.contentLength = length;
        }

        internal long OnReceiveData(byte[] data, ulong len)
        {
            if (file == null)
            {
                if (writing.Contains(destfile))
                {
                    // Spin wheels while waiting for lock to release.
                    while (writing.Contains(destfile)) { Thread.Sleep(500); }
                }
                lock (writing)
                {
                    writing.Add(destfile);
                }
                Directory.CreateDirectory(Path.GetDirectoryName(destfile));
                this.file = File.OpenWrite(destfile);
            }
            this.bytesDownloaded += len;
            this.file.Write(data, 0, data.Length);
            return (long)len;
        }

        internal double GetProgress()
        {
            if (contentLength == 0UL)
                return 0f;
            return bytesDownloaded / contentLength;
        }

        internal ulong GetMemorySize()
        {
            return bytesDownloaded;
        }
    }
}