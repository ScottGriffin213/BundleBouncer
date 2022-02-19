using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleBouncer.Format
{
    internal class AssetBundleBlockList6
    {
        private byte[] checksum;

        internal AssetBundleBlockList6()
        {

        }
        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "blocklist6.checksum";
            this.checksum = vbr.GetBytes(fieldName, 16); // 8+8 (low+high)

            fieldName = "blocklist6.blocks.len";
            var nblocks = vbr.GetS32(fieldName, 0);
            if(nblocks > 100)
            {
                Logging.Warning($"{fieldName} == {nblocks}, may cause lag or OOMEs.");
            }

        }
    }
}
