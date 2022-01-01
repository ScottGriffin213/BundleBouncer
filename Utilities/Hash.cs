using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BundleBouncer.Utilities {
    public class Hash {
        public static string SHA256String(string value) {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}