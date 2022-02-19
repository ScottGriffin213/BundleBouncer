using System.Collections.Generic;

namespace BundleBouncer.Format
{
    internal class FormatHeader6
    {
        private const int MIN_COMPRESSED_SIZE = 50;
        private const int MIN_DECOMPRESSED_SIZE = 50;
        private const int MIN_TOTAL_FILE_SIZE = 256;

        public string minPlayerVersion;
        public string curPlayerVersion;
        public ulong  totalFileSize;
        public uint   compressedSize;
        public uint   decompressedSize;
        public uint   flags;

        public FormatHeader6()
        {
        }

        public uint compressionType 
        { 
            get 
            { 
                return flags & 0x3F; 
            } 
        }

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "format_header.min_player_version";
            minPlayerVersion = vbr.GetCString(fieldName, Constants.MIN_PLAYER_VERSION_LENGTH_MIN, Constants.MIN_PLAYER_VERSION_LENGTH_MAX);
            if (!Constants.ASSETBUNDLE_HEADER_ALLOWED_MIN_PLAYER_VERSIONS.Contains(minPlayerVersion))
            {
                throw new FailedValidation(fieldName, $"Unrecognized min player version: {minPlayerVersion}");
            }

            fieldName = "format_header.cur_player_version";
            curPlayerVersion = vbr.GetCString(fieldName, Constants.CUR_PLAYER_VERSION_LENGTH_MIN, Constants.CUR_PLAYER_VERSION_LENGTH_MAX);
            if (!Constants.ASSETBUNDLE_HEADER_ALLOWED_CUR_PLAYER_VERSIONS.Contains(curPlayerVersion))
            {
                Logging.Warning($"{fieldName} - Unrecognized current player version: {curPlayerVersion}");
                //throw new FailedValidation(fieldName, $"Unrecognized current player version: {curPlayerVersion}");
            }

            fieldName = "format_header.total_file_size";
            totalFileSize = vbr.GetU64(fieldName, MIN_TOTAL_FILE_SIZE);

            fieldName = "format_header.compressed_size";
            compressedSize = vbr.GetU32(fieldName, MIN_COMPRESSED_SIZE);

            fieldName = "format_header.decompressed_size";
            decompressedSize = vbr.GetU32(fieldName, MIN_DECOMPRESSED_SIZE);

            fieldName = "format_header.flags";
            flags = vbr.GetU32(fieldName);
            if(compressionType > 3)
            {
                throw new FailedValidation(fieldName, $"Invalid compression type {compressionType}");
            }
        }
    }
}