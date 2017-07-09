using System.Collections.Generic;
using System.IO;

namespace TextTableParser
{
    public class Table
    {
        public string[] Columns { get; set; }
        public IList<string[]> Rows { get; set; }

        public void SaveToCsv(string fileName)
        {
            using (var stream = new StreamWriter(fileName))
            {
                SaveToCsv(stream);
            }
        }
        public void SaveToCsv(StreamWriter stream)
        {
            stream.WriteLine(string.Join(",", Columns));
            foreach (var row in Rows)
            {
                stream.WriteLine(string.Join(",", row));
            }
        }
    }
}
