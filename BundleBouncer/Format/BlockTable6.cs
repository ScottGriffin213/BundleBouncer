using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Format
{
    internal class BlockTable6
    {
        internal byte[] checksum;
        internal BlockRow6[] blocks;

        internal BlockTable6()
        {

        }

        internal void ZeroOutChecksum()
        {
            this.checksum = new byte[16];
        }

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "blocktable6.checksum";
            this.checksum = vbr.GetBytes(fieldName, 16); // 8+8 (low+high)

            fieldName = "blocktable6.blocks.len";
            var nblocks = vbr.GetS32(fieldName, 0);
            if (nblocks > 100)
            {
                Logging.Warning($"{fieldName} == {nblocks}, may cause lag or OOMEs.");
            }

            blocks = new BlockRow6[nblocks];
            for (uint i = 0; i < nblocks; i++)
            {
                var curRow = new BlockRow6();
                curRow.index = i;
                curRow.Read(vbr);
                blocks[i] = curRow;
            }
        }
    }
}
