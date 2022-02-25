using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Validation
{
    public class BlockTable6
    {
        public byte[] checksum;
        public BlockRow6[] blocks;

        public BlockTable6()
        {

        }

        public void ZeroOutChecksum()
        {
            checksum = new byte[16];
        }

        public void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "blocktable6.checksum";
            checksum = vbr.GetBytes(fieldName, 16); // 8+8 (low+high)

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
