// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;
using System.Collections.Generic;

namespace BundleBouncer.Format.Structures
{
    public partial class AssetBundleFile : KaitaiStruct
    {
        public static AssetBundleFile FromFile(string fileName)
        {
            return new AssetBundleFile(new KaitaiStream(fileName));
        }

        public AssetBundleFile(KaitaiStream p__io, KaitaiStruct p__parent = null, AssetBundleFile p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _fileHeader = new FileHeader(m_io, this, m_root);
            switch (FileHeader.FormatVersion) {
            case 3: {
                _formatHeader = new FormatHeaderV3(m_io, this, m_root);
                break;
            }
            case 6: {
                _formatHeader = new FormatHeaderV6(m_io, this, m_root);
                break;
            }
            case 7: {
                _formatHeader = new FormatHeaderV6(m_io, this, m_root);
                break;
            }
            }
        }
        public partial class FileHeader : KaitaiStruct
        {
            public static FileHeader FromFile(string fileName)
            {
                return new FileHeader(new KaitaiStream(fileName));
            }

            public FileHeader(KaitaiStream p__io, AssetBundleFile p__parent = null, AssetBundleFile p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _magic = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _formatVersion = m_io.ReadU4be();
            }
            private string _magic;
            private uint _formatVersion;
            private AssetBundleFile m_root;
            private AssetBundleFile m_parent;
            public string Magic { get { return _magic; } }

            /// <summary>
            /// Generally 6 or 7
            /// </summary>
            public uint FormatVersion { get { return _formatVersion; } }
            public AssetBundleFile M_Root { get { return m_root; } }
            public AssetBundleFile M_Parent { get { return m_parent; } }
        }
        public partial class FormatHeaderV3 : KaitaiStruct
        {
            public static FormatHeaderV3 FromFile(string fileName)
            {
                return new FormatHeaderV3(new KaitaiStream(fileName));
            }

            public FormatHeaderV3(KaitaiStream p__io, AssetBundleFile p__parent = null, AssetBundleFile p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _minPlayerVersion = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _currentPlayerVersion = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _minStreamedBytes = m_io.ReadU4be();
                _dataOffset = m_io.ReadU4be();
                _numAssets = m_io.ReadU4be();
                _numLevels = m_io.ReadU4be();
                _levelList = new List<OffsetPairV3>((int) (NumLevels));
                for (var i = 0; i < NumLevels; i++)
                {
                    _levelList.Add(new OffsetPairV3(m_io, this, m_root));
                }
                _fileSize = m_io.ReadU4be();
                _unknown = m_io.ReadBytes(5);
                _bundleCount = m_io.ReadU4be();
            }
            private string _minPlayerVersion;
            private string _currentPlayerVersion;
            private uint _minStreamedBytes;
            private uint _dataOffset;
            private uint _numAssets;
            private uint _numLevels;
            private List<OffsetPairV3> _levelList;
            private uint _fileSize;
            private byte[] _unknown;
            private uint _bundleCount;
            private AssetBundleFile m_root;
            private AssetBundleFile m_parent;
            public string MinPlayerVersion { get { return _minPlayerVersion; } }
            public string CurrentPlayerVersion { get { return _currentPlayerVersion; } }
            public uint MinStreamedBytes { get { return _minStreamedBytes; } }
            public uint DataOffset { get { return _dataOffset; } }
            public uint NumAssets { get { return _numAssets; } }
            public uint NumLevels { get { return _numLevels; } }
            public List<OffsetPairV3> LevelList { get { return _levelList; } }
            public uint FileSize { get { return _fileSize; } }
            public byte[] Unknown { get { return _unknown; } }
            public uint BundleCount { get { return _bundleCount; } }
            public AssetBundleFile M_Root { get { return m_root; } }
            public AssetBundleFile M_Parent { get { return m_parent; } }
        }
        public partial class OffsetPairV3 : KaitaiStruct
        {
            public static OffsetPairV3 FromFile(string fileName)
            {
                return new OffsetPairV3(new KaitaiStream(fileName));
            }

            public OffsetPairV3(KaitaiStream p__io, AssetBundleFile.FormatHeaderV3 p__parent = null, AssetBundleFile p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _compressed = m_io.ReadU4be();
                _uncompressed = m_io.ReadU4be();
            }
            private uint _compressed;
            private uint _uncompressed;
            private AssetBundleFile m_root;
            private AssetBundleFile.FormatHeaderV3 m_parent;
            public uint Compressed { get { return _compressed; } }
            public uint Uncompressed { get { return _uncompressed; } }
            public AssetBundleFile M_Root { get { return m_root; } }
            public AssetBundleFile.FormatHeaderV3 M_Parent { get { return m_parent; } }
        }
        public partial class FormatHeaderV6 : KaitaiStruct
        {
            public static FormatHeaderV6 FromFile(string fileName)
            {
                return new FormatHeaderV6(new KaitaiStream(fileName));
            }

            public FormatHeaderV6(KaitaiStream p__io, AssetBundleFile p__parent = null, AssetBundleFile p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _minPlayerVersion = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _currentPlayerVersion = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _fileSize = m_io.ReadS8be();
                _compressedSize = m_io.ReadU4be();
                _decompressedSize = m_io.ReadU4be();
                _flags = m_io.ReadU4be();
                if (M_Root.FileHeader.FormatVersion >= 7) {
                    __unnamed6 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
                }
            }
            private string _minPlayerVersion;
            private string _currentPlayerVersion;
            private long _fileSize;
            private uint _compressedSize;
            private uint _decompressedSize;
            private uint _flags;
            private byte[] __unnamed6;
            private AssetBundleFile m_root;
            private AssetBundleFile m_parent;
            public string MinPlayerVersion { get { return _minPlayerVersion; } }
            public string CurrentPlayerVersion { get { return _currentPlayerVersion; } }
            public long FileSize { get { return _fileSize; } }
            public uint CompressedSize { get { return _compressedSize; } }
            public uint DecompressedSize { get { return _decompressedSize; } }
            public uint Flags { get { return _flags; } }
            public byte[] Unnamed_6 { get { return __unnamed6; } }
            public AssetBundleFile M_Root { get { return m_root; } }
            public AssetBundleFile M_Parent { get { return m_parent; } }
        }
        private FileHeader _fileHeader;
        private KaitaiStruct _formatHeader;
        private AssetBundleFile m_root;
        private KaitaiStruct m_parent;
        public FileHeader FileHeader { get { return _fileHeader; } }
        public KaitaiStruct FormatHeader { get { return _formatHeader; } }
        public AssetBundleFile M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
