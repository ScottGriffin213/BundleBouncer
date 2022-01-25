using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Data
{
    public interface IShitListProvider
    {
        /// <summary>
        /// Returns true if digest of avatar ID is a known crasher.
        /// </summary>
        /// <param name="digestOfAvId">SHA256 sum of avatar ID (UTF-8 encoded)</param>
        /// <returns></returns>
        bool IsAvatarIDAnAssetBundleCrasher(byte[] digestOfAvId);
        /// <summary>
        /// Returns true if digest of assetbundle is a known crasher.
        /// </summary>
        /// <param name="digest">SHA256 sum of assetbundle</param>
        /// <returns></returns>
        bool IsAssetBundleAnAssetBundleCrasher(byte[] digest);
    }
}
