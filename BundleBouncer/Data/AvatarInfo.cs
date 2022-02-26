/**
 * BundleBouncer Avatar Info
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

using System.Collections.Generic;
using VRC.Core;

namespace BundleBouncer.Data
{
    public class AvatarInfo
    {
        public string ID;
        public string Name;
        public HashSet<string> AssetIDs = new HashSet<string>();
        public HashSet<string> Users = new HashSet<string>();
        public HashSet<byte[]> Hashes = new HashSet<byte[]>();

        internal void FromModel(Model.Avatar avDict)
        {
            ID = avDict.Id;
            Name = avDict.Name;
            foreach (var pkg in avDict.UnityPackages)
            {
                if (ApiFile.TryParseFileIdAndVersionFromFileAPIUrl(pkg.AssetUrl, out string fileID, out int _))
                {
                    AssetIDs.Add(fileID);
                }
            }
        }
    }
}