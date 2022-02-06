using System.IO;
using System.Security.Cryptography;
using System.Text;

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

        internal static byte[] SHA256String(string input)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            }
        }
    }
}