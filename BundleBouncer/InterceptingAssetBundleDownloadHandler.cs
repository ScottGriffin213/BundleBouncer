/**
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
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Networking;

namespace BundleBouncer
{
    internal class InterceptingAssetBundleDownloadHandler : DownloadHandler
    {
        private string url;
        private string method;
        private string destfile;
        private DownloadHandlerFile dl;
        private DownloadHandlerAssetBundle dhab;
        private ulong contentLength;

        public InterceptingAssetBundleDownloadHandler(string url, string method, string destfile, DownloadHandlerAssetBundle dhab)
        {
            this.url = url;
            this.method = method;
            this.destfile = destfile;
            this.dl = new DownloadHandlerFile(destfile);
            this.dhab = dhab;
        }

        public override void CompleteContent()
        {
            dl.CompleteContent();
            dhab.CompleteContent();
        }
        public new void Dispose()
        {
            dl?.Dispose();
            dhab?.Dispose();
        }
#pragma warning disable CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
        public new void Finalize()
#pragma warning restore CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
        {
            dl?.Finalize();
            dhab?.Finalize();
        }
        public new string GetContentType()
        {
            return dhab.GetContentType();
        }
        public new Il2CppStructArray<byte> GetData()
        {
            return dl.GetData();
        }
        public new float GetProgress()
        {
            return dl.GetProgress();
        }
        public new string GetText()
        {
            return null; // lolno
        }
        public new Encoding GetTextEncoder()
        {
            return null;
        }
        public new bool IsDone() {
            return dl.IsDone();
        }
        public new void ReceiveContentLength(int length)
        {
            this.contentLength = (ulong)length;
            dl.ReceiveContentLength(length);
        }
        public new void ReceiveContentLengthHeader(ulong length)
        {
            this.contentLength = length;
            dl.ReceiveContentLengthHeader(length);
        }
        public new bool ReceiveData(Il2CppStructArray<byte> data, int length)
        {
            var o = dl.ReceiveData(data, length);
            if (isDone)
            {
                var hash = IOTool.SHA256File(destfile);
                if (AvatarShitList.IsAssetBundleHashBlocked(hash))
                {
                    BundleBouncer.NotifyUserOfBlockedBundle(hash, "InterceptingDownloadHandlerAssetBundle");
                    dhab.ReceiveContentLengthHeader(0);
                    dhab.ReceiveData(new Il2CppStructArray<byte>(0), 0);
                    dhab.CompleteContent();
                }
                else
                {
                    dhab.ReceiveContentLengthHeader(this.contentLength);
                    var downloaded = dl.data;
                    dhab.ReceiveData(downloaded, downloaded.Length);
                    dhab.CompleteContent();
                }
            }
            return o;
        }
        public new Il2CppStructArray<byte> data {get{return GetData(); } }
        public new bool isDone { get { return IsDone(); } }
        public new string text { get { return GetText(); } }
        public AssetBundle assetBundle { get { return dhab.assetBundle; } }
    }
}