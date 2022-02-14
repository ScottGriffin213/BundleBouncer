/**
 * BundleBouncer Shitlist DLL Interface
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

/*************************************************************************
 * IMPORTANT IMPORTANT IMPORTANT IMPORTANT IMPORTANT IMPORTANT IMPORTANT *
 *************************************************************************
 * If you change this interface, you *must* change the code generation 
 * code for the shitlist DLL. Ask a core dev like Scott to do this for you.
 *************************************************************************/

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
