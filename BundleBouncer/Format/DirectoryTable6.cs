namespace BundleBouncer.Format
{
    internal class DirectoryTable6
    {
        internal DirectoryRow6[] rows;

        public DirectoryTable6()
        {
        }

        internal void Read(ValidatingBinaryReader vbr)
        {
            string fieldName = "directorytable6.rows.len";
            var nrows = vbr.GetS32(fieldName, 0);
            if (nrows > 100)
            {
                Logging.Warning($"{fieldName} == {nrows}, may cause lag or OOMEs.");
            }

            rows = new DirectoryRow6[nrows];
            for (uint i = 0; i < nrows; i++)
            {
                var curRow = new DirectoryRow6();
                curRow.index = i;
                curRow.Read(vbr);
                rows[i] = curRow;
            }
        }
    }
}