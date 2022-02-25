using AssetsTools.NET.Extra;
using AssetsTools.NET.Extra.Decompressors.LZ4;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.IO;

namespace BundleBouncer.Validation
{
    public class AssetBundleFile
    {
        private readonly HashSet<string> allowedMagic = new HashSet<string>(){
            "UnityFS"
        };

        public string magic;
        public uint version;
        public FormatHeader6 formatHeader;
        public BlockTable6 blockTable;
        public DirectoryTable6 dirTable;

        public Action<BlockRow6, byte[]> OnBlockRead = null;
        public Action<DirectoryRow6, byte[]> OnDirectoryRead = null;

        public ulong fileSize;

        public AssetBundleFile()
        {

        }

        public void Read(ValidatingBinaryReader vbr, ulong fileSize)
        {
            this.fileSize = fileSize;

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

            using (var blockstream = new ValidatingBinaryReader(SetupDecompressionStream("blocks", vbr, GetBundleInfoOffset(), (byte)this.formatHeader.compressionType)))
                ReadBlockInfo(blockstream);

            vbr.BaseStream.Position = GetFileDataOffset();
            var blockStart = (ulong)vbr.BaseStream.Position;
            ulong blockEnd;
            foreach (var block in this.blockTable.blocks)
            {
                Logging.Info($"Block {block.index} @ {blockStart}: flags: 0x{block.flags:X4}, uncompressed: {block.decompressedSize}B, compressed: {block.compressedSize}B, compression-type: {block.compressionType}");
                fieldName = $"blocks[{block.index}].start";
                if (blockStart > fileSize)
                {
                    throw new FailedValidation(fieldName, "Block start is beyond the end of the file");
                }

                if (block.isCompressed)
                {
                    fieldName = $"blocks[{block.index}].compressed-size";
                    if (block.compressedSize > fileSize)
                        throw new FailedValidation(fieldName, $"Block size exceeds file size ({block.compressedSize}B > {fileSize}B)");
                    blockEnd = blockStart + block.compressedSize;

                    fieldName = $"blocks[{block.index}].end";
                    if (blockEnd > fileSize)
                        throw new FailedValidation(fieldName, $"Block end is beyond the end of the file (0x{blockEnd:X16} > 0x{fileSize:X16})");

                    fieldName = $"blocks[{block.index}].data";
                }
                else
                {
                    fieldName = $"blocks[{block.index}].decompressed-size";
                    if (block.decompressedSize > fileSize)
                        throw new FailedValidation(fieldName, $"Block size exceeds file size ({block.decompressedSize}B > {fileSize}B)");
                    blockEnd = blockStart + block.decompressedSize;

                    fieldName = $"blocks[{block.index}].end";
                    if (blockEnd > fileSize)
                        throw new FailedValidation(fieldName, $"Block end is beyond the end of the file (0x{blockEnd:X16} > 0x{fileSize:X16})");
                }

                using (var ms = new MemoryStream(new byte[block.decompressedSize]))
                {
                    /*
                    using (var stream = SetupDecompressionStream(fieldName, vbr, block.compressedSize, block.compressionType))
                    {
                        stream.Position = 0;
                        var data = new byte[block.decompressedSize];
                        stream.Read(data, 0, (int)block.decompressedSize);
                        OnBlockRead(block, data);
                    }
                    */
                    switch (block.compressionType)
                    {
                        case 0:
                            vbr.BaseStream.CopyToCompat(ms, block.compressedSize);
                            break;
                        case 1:
                            SevenZipHelper.StreamDecompress(vbr.BaseStream, ms, block.compressedSize, block.decompressedSize);
                            break;
                        case 2:
                        case 3:
                            using (MemoryStream tempMs = new MemoryStream())
                            {
                                vbr.BaseStream.CopyToCompat(tempMs, block.compressedSize);
                                tempMs.Position = 0;

                                using (Lz4DecoderStream decoder = new Lz4DecoderStream(tempMs))
                                {
                                    decoder.CopyToCompat(ms, block.decompressedSize);
                                }
                            }
                            break;
                    }
                    OnBlockRead?.Invoke(block, ms.ToArray());
                }

            }
        }

        private void ReadBlockInfo(ValidatingBinaryReader vbr)
        {
            vbr.BaseStream.Position = 0;
            this.blockTable = new BlockTable6();
            this.blockTable.Read(vbr);
            this.dirTable = new DirectoryTable6();
            this.dirTable.Read(vbr);
        }

        private Stream SetupDecompressionStream(string fieldName, ValidatingBinaryReader vbr, long position, byte compressionType)
        {
            var stream = vbr.BaseStream;
            stream.Position = position;
            switch (compressionType)
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
            throw new FailedValidation(fieldName, $"Invalid compression scheme 0x{formatHeader.compressionType:X2}");
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

        // Modified from AssetTools.NET
        public long GetFileDataOffset()
        {
            long ret = this.formatHeader.minPlayerVersion.Length + this.formatHeader.curPlayerVersion.Length + 0x1A;
            if ((this.formatHeader.flags & 0x100) != 0)
                ret += 0x0A;
            else
                ret += this.magic.Length + 1;

            if (this.version >= 7)
                ret = (ret + 15) >> 4 << 4;
            if ((this.formatHeader.flags & 0x80) == 0)
                ret += this.formatHeader.compressedSize;
            return ret;
        }
    }
}
