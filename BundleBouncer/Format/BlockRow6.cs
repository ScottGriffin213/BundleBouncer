using System;

namespace BundleBouncer.Format
{
    internal class BlockRow6
    {
        internal uint index = 0;
        internal uint decompressedSize = 0;
        internal uint compressedSize = 0;
        internal ushort flags = 0;

        internal byte compressionType
        {
            get
            {
                return (byte)(flags & 0x3F);
            }
        }

        public BlockRow6()
        {
        }

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName;

            fieldName = $"blockrow6[{index}].decompressed-size";
            decompressedSize = vbr.GetU32(fieldName); // TODO: Get baseline

            fieldName = $"blockrow6[{index}].compressed-size";
            compressedSize = vbr.GetU32(fieldName); // TODO: Get baseline

            fieldName = $"blockrow6[{index}].flags";
            flags = vbr.GetU16(fieldName); // TODO: Get baseline
        }
    }
}