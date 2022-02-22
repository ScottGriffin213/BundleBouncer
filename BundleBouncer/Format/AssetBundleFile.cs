using AssetsTools.NET.Extra.Decompressors.LZ4;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (version >= 7)
            {
                vbr.AlignTo(16);
            }

            using (var blockstream = new ValidatingBinaryReader(SetupDecompressionStream("blocks", vbr)))
                ReadBlockInfo(blockstream);
        }

        private void ReadBlockInfo(ValidatingBinaryReader vbr)
        {
            // TODO
        }

        private Stream SetupDecompressionStream(string fieldName, ValidatingBinaryReader vbr)
        {
            var stream = vbr.BaseStream;
            stream.Position = GetBundleInfoOffset();
            switch (this.formatHeader.compressionType)
            {
                case 0:
                    Logging.Info("No compression");
                    return stream;
                case 1: // Basic LZMA
                    Logging.Info("Decompressing LZMA...");
                    using (var ms = new MemoryStream(vbr.GetBytes($"{fieldName}(LZMA).size", (int)formatHeader.compressedSize)))
                    {
                        return SevenZipHelper.StreamDecompress(ms);
                    }
                case 2:
                case 3:
                    Logging.Info("Decompressing LZ4...");
                    var uncompressedBytes = new byte[formatHeader.decompressedSize];
                    using (var ms = new MemoryStream(vbr.GetBytes($"{fieldName}(LZ4).size", (int)formatHeader.compressedSize)))
                    {
                        using (var lz4 = new Lz4DecoderStream(ms))
                        lz4.Read(uncompressedBytes, 0, (int)formatHeader.decompressedSize);
                    }
                    return new MemoryStream(uncompressedBytes);
            }
            throw new FailedValidation(fieldName, $"Invalid compression scheme {formatHeader.compressionType}");
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
