using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Format
{
    internal class AssetBundleFile
    {
        private readonly HashSet<string> allowedMagic = new HashSet<string>(){
            "UnityFS"    
        };
        
        private string magic;
        private uint version;
        private FormatHeader6 formatHeader;

        public AssetBundleFile()
        {

        }

        public void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "file_header.magic";
            magic = vbr.GetCString(fieldName);
            if(!allowedMagic.Contains(magic))
            {
                throw new FailedValidation(fieldName, $"Bad magic string: {magic}");
            }

            fieldName = "file_header.version";
            version = vbr.GetU32(fieldName, min: 6, max: 7);
            switch(version)
            {
                case 6:
                    case 7:
                    this.formatHeader = new FormatHeader6();
                    break;
            }
            this.formatHeader.Read(vbr);
        }
    }
}
