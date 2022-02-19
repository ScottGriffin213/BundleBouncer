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
            if (!allowedMagic.Contains(magic))
            {
                throw new FailedValidation(fieldName, $"Bad magic string: {magic}");
            }

            fieldName = "file_header.version";
            version = vbr.GetU32(fieldName);
            switch (version)
            {
                case 6:
                case 7:
                    this.formatHeader = new FormatHeader6();
                    break;
                default:
                    throw new FailedValidation(fieldName, $"Unrecognized version {version}");
            }
            this.formatHeader.Read(vbr);

            if(version >= 7)
            {
                vbr.AlignTo(16);
            }
        }

        // From AssetTools.NET
        public long GetBundleInfoOffset()
        {
            if ((formatHeader.flags & 0x80) != 0)
            {
                if (formatHeader.totalFileSize == 0)
                    return -1;
                return (long)(formatHeader.totalFileSize - formatHeader.compressedSize);
            }
            else
            {
                //if (!strcmp(this->signature, "UnityWeb") || !strcmp(this->signature, "UnityRaw"))
                //	return 9;
                long ret = formatHeader.minPlayerVersion.Length + formatHeader.curPlayerVersion.Length + 0x1A;
                if (this.version >= 7)
                {
                    if ((formatHeader.flags & 0x100) != 0)
                        return ((ret + 0x0A) + 15) >> 4 << 4;
                    else
                        return ((ret + this.magic.Length + 1) + 15) >> 4 << 4;
                }
                else
                {
                    if ((formatHeader.flags & 0x100) != 0)
                        return ret + 0x0A;
                    else
                        return ret + this.magic.Length + 1;
                }
            }
        }
    }
}
