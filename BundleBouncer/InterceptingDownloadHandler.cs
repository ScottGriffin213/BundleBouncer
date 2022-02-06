using BundleBouncer.Data;
using UnhollowerBaseLib;
using UnityEngine.Networking;

namespace BundleBouncer
{
    /// <summary>
    /// Wrapper around DownloadHandler.
    /// </summary>
    internal class InterceptingDownloadHandler : DownloadHandler
    {
        private DownloadHandler downloadHandler;
        private string url;
        private string method;

        public InterceptingDownloadHandler(DownloadHandler downloadHandler, string url, string method)
        {
            this.downloadHandler = downloadHandler;
            this.url = url;
            this.method = method;
        }

        public override void CompleteContent() => downloadHandler.CompleteContent();
        public override float GetProgress() => downloadHandler.GetProgress();

        public new Il2CppStructArray<byte> data
        {
            get
            {
                var rcvdata = downloadHandler.data;
                var hash = IOTool.SHA256Data(rcvdata);
                Logging.Info($"Intercepted DownloadHandler.data call. Result: {rcvdata.Length}B");
                if (AvatarShitList.IsAssetBundleHashBlocked(hash))
                {
                    BundleBouncer.NotifyUserOfBlockedBundle(hash, "InterceptingDownloadHandler");
                    return null;
                }
                return rcvdata;
            }
        }

        public new Il2CppStructArray<byte> GetData()
        {                
            var rcvdata = downloadHandler.GetData();
            var hash = IOTool.SHA256Data(rcvdata);
            Logging.Info($"Intercepted DownloadHandler.data call. Result: {rcvdata.Length}B");
            if (AvatarShitList.IsAssetBundleHashBlocked(hash))
            {
                BundleBouncer.NotifyUserOfBlockedBundle(hash, "InterceptingDownloadHandler");
                return null;
            }
            return rcvdata;
        }

        public override bool ReceiveData(Il2CppStructArray<byte> data, int dataLength) => downloadHandler.ReceiveData(data, dataLength);
        public new string ToString() => downloadHandler.ToString();
        public new void Dispose() => downloadHandler.Dispose();
#pragma warning disable CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
        public new void Finalize() => downloadHandler.Finalize();
#pragma warning restore CS0465 // Introducing a 'Finalize' method can interfere with destructor invocation
        public new string GetContentType() => downloadHandler.GetContentType();

        public new bool isDone
        {
            get
            {
                return downloadHandler.isDone;
            }
        }

        public new string text
        {
            get
            {
                return downloadHandler.text;
            }
        }

        public new string GetText() => downloadHandler.GetText();
        public new Il2CppSystem.Text.Encoding GetTextEncoder() => downloadHandler.GetTextEncoder();
        public new bool IsDone() => downloadHandler.IsDone();
        public new void ReceiveContentLength(int contentLength) => downloadHandler.ReceiveContentLength(contentLength);
        public new void ReceiveContentLengthHeader(ulong contentLength) => downloadHandler.ReceiveContentLengthHeader(contentLength);
        public new void Release() => downloadHandler.Release();
    }
}