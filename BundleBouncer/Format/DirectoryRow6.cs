using System;

namespace BundleBouncer.Format
{
    internal class DirectoryRow6
    {
        internal uint index;
        private long offset;
        private long decompressedSize;
        private uint flags;
        private string name;

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "directoryrow6.offset";
            this.offset = vbr.GetS64(fieldName, 0);

            fieldName = "directoryrow6.decompressedSize";
            this.decompressedSize = vbr.GetS64(fieldName, 0);
            
            fieldName = "directoryrow6.flags";
            this.flags = vbr.GetU32(fieldName);

            fieldName = "directoryrow6.decompressedSize";
            this.name = vbr.GetCString(fieldName, 1UL, 255UL);
        }
    }
}