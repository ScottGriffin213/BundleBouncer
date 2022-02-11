﻿/**
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
using System;
using System.Linq;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;

namespace BundleBouncer
{
    internal class InterceptingAssetBundleDownloadHandler
    {
        private string url;
        private string method;
        private string destfile;
        private DownloadHandlerFile dl;
        private DownloadHandlerAssetBundle dhab;
        internal IntPtr ptr;
        private ulong contentLength;

        public InterceptingAssetBundleDownloadHandler(string url, string method, string destfile, DownloadHandlerAssetBundle dhab)
        {
            this.url = url;
            this.method = method;
            this.destfile = destfile;
            this.dl = new DownloadHandlerFile(destfile);
            this.dhab = dhab;
            this.ptr = dhab.Pointer;
            Logging.Info($"IDHAB - Intercepting bytes sent to {dhab}...");
        }


        public bool IsDone()
        {
            return dl.IsDone();
        }

        public bool ReceiveData(Il2CppStructArray<byte> data, int length)
        {
            return dl.ReceiveData(data, length);
        }

        internal void OnCompleteContent()
        {
            dl.CompleteContent();
            var hash = IOTool.SHA256File(destfile);
            var strhash = string.Concat(hash.Select(x => x.ToString("X2")));
            Logging.Info($"IDHAB - Done! Scanning file {destfile} ({strhash})...");
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
            {
                BundleBouncer.NotifyUserOfBlockedBundle(hash, "InterceptingDownloadHandlerAssetBundle");
                Patches.SendDelayedDHABSignals(ptr, new byte[0], 0UL);
            }
            else
            {
                var downloaded = dl.data;
                Patches.SendDelayedDHABSignals(ptr, downloaded, contentLength);
            }
        }

        internal void ProcessHeaders(ulong length)
        {
            Logging.Info($"Content-Length: {length}UL");
            this.contentLength = length;
            dl.ReceiveContentLengthHeader(length);
        }

        internal long OnReceiveData(byte[] data, ulong len)
        {
            dl.ReceiveData(data, (int)len);
            return (long)len; //?
        }

        internal double GetProgress()
        {
            return dl.GetProgress();
        }
    }
}