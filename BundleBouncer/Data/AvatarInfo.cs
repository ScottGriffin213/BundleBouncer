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

        internal void FromDynamic(dynamic avDict)
        {
            ID = avDict["id"].ToString();
            Name = avDict["name"].ToString();
            foreach(dynamic pkg in avDict["unityPackages"])
            {
                if (ApiFile.TryParseFileIdAndVersionFromFileAPIUrl(pkg["assetUrl"].ToString(), out string fileID, out int _))
                {
                    AssetIDs.Add(fileID);
                }
            }
        }
    }
}