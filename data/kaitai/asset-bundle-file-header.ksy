meta:
  id: asset_bundle_file_header
  title: 'Unity3D AssetBundle File'
  endian: be
  license: 'MIT'
seq:
  - id: file_header
    type: file_header
  - id: format_header
    type: 
      switch-on: file_header.format_version
      cases:
        3: format_header_v3
        6: format_header_v6
        7: format_header_v6
types:
  file_header:
    seq:
      - id: magic
        type: strz
        encoding: 'utf-8'
      - id: format_version
        type: u4
        doc: 'Generally 6 or 7'
  format_header_v3:
    seq:
      - id: min_player_version
        type: strz
        encoding: 'utf-8'
      - id: current_player_version
        type: strz
        encoding: 'utf-8'
      - id: min_streamed_bytes
        type: u4
      - id: data_offset
        type: u4
      - id: num_assets
        type: u4
      - id: num_levels
        type: u4
      - id: level_list
        type: offset_pair_v3
        repeat: expr
        repeat-expr: num_levels
      - id: file_size
        type: u4
      - id: unknown
        size: 5
      - id: bundle_count
        type: u4
  offset_pair_v3:
    seq:
      - id: compressed
        type: u4
      - id: uncompressed
        type: u4
  format_header_v6:
    seq:
      - id: min_player_version
        type: strz
        encoding: 'utf-8'
      - id: current_player_version
        type: strz
        encoding: 'utf-8'
      - id: file_size
        type: s8 # sus
      - id: compressed_size
        type: u4
      - id: decompressed_size
        type: u4
      - id: flags
        type: u4
      # Align16
      - if: _root.file_header.format_version >= 7
        size: (16 - _io.pos) % 16 # https://github.com/kaitai-io/kaitai_struct/issues/12#issuecomment-277757932
        #id: padding # Comment out if you don't need it
