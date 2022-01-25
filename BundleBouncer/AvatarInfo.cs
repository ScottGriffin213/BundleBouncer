using System.Collections.Generic;

namespace BundleBouncer
{
    public class AvatarInfo
    {
        public string ID;
        public string Name;
        public HashSet<string> AssetIDs = new HashSet<string>();
        public HashSet<string> Users = new HashSet<string>();
    }
}