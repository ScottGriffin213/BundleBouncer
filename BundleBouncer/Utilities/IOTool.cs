using System;
using System.IO;
using System.Security.Cryptography;
using UnhollowerBaseLib;

namespace BundleBouncer
{
    internal class IOTool
    {
        internal static byte[] SHA256File(string bpath)
        {
            using(var stream = File.OpenRead(bpath))
            {
                using (var sha256 = new SHA256Managed())
                {
                    return sha256.ComputeHash(stream);
                }
            }
        }

        internal static byte[] SHA256Data(byte[] rcvdata)
        {
            using (var stream = new MemoryStream(rcvdata))
            {
                using (var sha256 = new SHA256Managed())
                {
                    return sha256.ComputeHash(stream);
                }
            }
        }
    }
}