using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TextTableParser.TextFileParser
{
    internal static class StreamReaderExtensions
    {
        public static IEnumerable<string> ReadFileAsLines(this StreamReader streamReader)
        {
            while (true)
            {
                var line = streamReader.ReadLine();
                if (line != null)
                {
                    yield return line;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
