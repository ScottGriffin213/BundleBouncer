using System.Collections.Generic;

namespace BundleBouncer.Format
{
    internal class FormatHeader6
    {
        // These two were generated using devtools/gather.py, which collects data from cache.
        private static readonly HashSet<string> ALLOWED_MIN_PLAYER_VERSIONS = new HashSet<string>() { "5.x.x" };
        private static readonly HashSet<string> ALLOWED_CUR_PLAYER_VERSIONS = new HashSet<string>() { "2017.4.15f1", "2017.4.28f1", "2018.4.20f1", "2019.4.31f1" };

        public string minPlayerVersion;
        public string curPlayerVersion;
        public ulong  totalFileSize;
        public uint   compressedSize;
        public uint   decompressedSize;
        public uint   flags;

        public FormatHeader6()
        {
        }

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "format_header.min_player_version";
            minPlayerVersion = vbr.GetCString(fieldName, 4, 30);
            if (!ALLOWED_MIN_PLAYER_VERSIONS.Contains(minPlayerVersion))
            {
                throw new FailedValidation(fieldName, $"Unrecognized min player version: {minPlayerVersion}");
            }

            fieldName = "format_header.cur_player_version";
            curPlayerVersion = vbr.GetCString(fieldName, 8, 30);
            if (!ALLOWED_CUR_PLAYER_VERSIONS.Contains(curPlayerVersion))
            {
                throw new FailedValidation(fieldName, $"Unrecognized current player version: {curPlayerVersion}");
            }

            fieldName = "format_header.total_file_size";
            totalFileSize = vbr.GetU64(fieldName, 256);

            fieldName = "format_header.compressed_size";
            compressedSize = vbr.GetU32(fieldName, 100);

            fieldName = "format_header.decompressed_size";
            decompressedSize = vbr.GetU32(fieldName, 100);

            fieldName = "format_header.flags";
            flags = vbr.GetU32(fieldName);
        }
    }
}