using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TextTableParser.TextFileParser
{
    public class CSVParser
    {
        private bool _hasHeader;
        private char _separator;
        private char? _quotationChar;

        public CSVParser(bool hasHeader, char separator, char? quotationChar)
        {
            _hasHeader = hasHeader;
            _separator = separator;
            _quotationChar = quotationChar;
        }

        public Table CreateFromCsv(string fileName)
        {
            return CreateFromCsv(fileName, Encoding.UTF8);
        }


        public Table CreateFromCsv(string fileName, Encoding encoding)
        {
            using (var reader = new StreamReader(fileName, encoding))
            {
                return CreateFromCsv(reader.ReadFileAsLines());
            }
        }

        public Table CreateFromCsv(IEnumerable<string> lines)
        {
            var result = new Table();

            var linesIterator = lines;

            if (_hasHeader)
            {
                result.Columns = lines.Take(1)
                                      .First()
                                      .Split(_separator)
                                      .Select(s => RemoveQuotationNeeded(s, _quotationChar))
                                      .ToArray();
                linesIterator = lines.Skip(1);
            }
            result.Rows = linesIterator.Select(line => line.Split(_separator)
                                                           .Select(s => RemoveQuotationNeeded(s, _quotationChar))
                                                           .ToArray())
                                       .ToList();

            return result;
        }

        private static string RemoveQuotationNeeded(string item, char? quotationChar)
        {
            if (!quotationChar.HasValue) {
                return item;
            }
            if (string.IsNullOrWhiteSpace(item))
            {
                return item;
            }
            if (item.First() != quotationChar.Value && item.Last() != quotationChar && item.Length >= 2)
            {
                throw new ArgumentOutOfRangeException($"CSV parsing: item='{ item }' has wrong quotation char.");
            }
            return item.Substring(1, item.Length - 2);
        }
    }
}
