namespace BundleBouncer.Data
{
    public interface IShitListProvider
    {
        /// <summary>
        /// Returns true if digest of avatar ID is a known crasher.
        /// </summary>
        /// <param name="digest">SHA256 sum of avatar ID (UTF-8 encoded)</param>
        /// <returns></returns>
        bool IsAvatarIDHashBlacklisted(byte[] digest);

        /// <summary>
        /// Returns true if digest of assetbundle is a known crasher.
        /// </summary>
        /// <param name="digest">SHA256 sum of assetbundle</param>
        /// <returns></returns>
        bool IsAssetBundleHashBlacklisted(byte[] digest);

        /// <summary>
        /// Is the given avatar ID in a whitelist?
        /// </summary>
        /// <param name="avID"></param>
        /// <returns></returns>
        bool IsAvatarIDWhitelisted(string avID);

        /// <summary>
        /// Returns true if a hash is whitelisted.
        /// </summary>
        /// <param name="digest">SHA256 sum of assetbundle</param>
        /// <returns></returns>
        bool IsAssetBundleHashWhitelisted(byte[] digest);
    }
}
