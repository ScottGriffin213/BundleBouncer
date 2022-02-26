using System;

namespace BundleBouncer.Validation
{
    public class BlockRow6
    {
        public uint index = 0;
        public uint decompressedSize = 0;
        public uint compressedSize = 0;
        public ushort flags = 0;

        public byte compressionType { get; private set; }
        public bool isCompressed { get; private set; }

        public BlockRow6()
        {
            isCompressed = false;
            compressionType = 0x00;
        }

        public void Read(ValidatingBinaryReader vbr)
        {
            string fieldName;

            fieldName = $"blockrow6[{index}].decompressed-size";
            decompressedSize = vbr.GetU32(fieldName); // TODO: Get baseline

            fieldName = $"blockrow6[{index}].compressed-size";
            compressedSize = vbr.GetU32(fieldName); // TODO: Get baseline

            fieldName = $"blockrow6[{index}].flags";
            flags = vbr.GetU16(fieldName); // TODO: Get baseline

            compressionType = (byte)(flags & 0x3F);
            isCompressed = compressionType != 0;

            if (compressionType < 0 || compressionType > 3)
                throw new FailedValidation(fieldName, $"Invalid CompressionType {compressionType}");
        }
    }
}