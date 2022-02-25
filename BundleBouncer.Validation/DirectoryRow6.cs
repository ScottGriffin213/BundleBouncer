using System;

namespace BundleBouncer.Validation
{
    public class DirectoryRow6
    {
        public uint index;
        public long offset;
        public long decompressedSize;
        public uint flags;
        public string name;

        public void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "directoryrow6.offset";
            offset = vbr.GetS64(fieldName, 0);

            fieldName = "directoryrow6.decompressedSize";
            decompressedSize = vbr.GetS64(fieldName, 0);

            fieldName = "directoryrow6.flags";
            flags = vbr.GetU32(fieldName);

            fieldName = "directoryrow6.decompressedSize";
            name = vbr.GetCString(fieldName, 1UL, 255UL);
        }
    }
}