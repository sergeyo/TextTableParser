using NUnit.Framework;
using System.Linq;
using TextTableParser.TextFileParser;

namespace TextTableParser.Tests
{
    [TestFixture(Category = "Unit")]
    public class CSVParserTests
    {
        [Test]
        public void Parse_WhenCalledWithCorrectData_ShouldReturnParsedTable()
        {
            var testData = new[] {
                new [] { "stringParam", "stringParam1", "stringParam2"  },
                new [] { "boolParam", "true", "false"  },
                new [] { "boolNullableParam", "false", ""  },
                new [] { "byteParam", "0", "255"  },
                new [] { "byteNullableParam", "128", ""  },
                new [] { "charParam", "A", "Z"  },
                new [] { "charNullableParam", "$", ""  },
                new [] { "DateTimeParam", "2017-01-01", "2015-12-31" },
                new [] { "DateTimeNullableParam", "1999-01-01", ""  },
                new [] { "decimalParam", "-100000", "10000000"  },
                new [] { "decimalNullableParam", "0", ""  },
                new [] { "doubleParam", "-3,1415926538", "3,1415926538"  },
                new [] { "doubleNullableParam", "0", ""  },
                new [] { "shortParam", "-32767", "32767"  },
                new [] { "shortNullableParam", "0", ""  },
                new [] { "intParam", "-2000000000", "2000000000"  },
                new [] { "intNullableParam", "0", ""  },
                new [] { "longParam", "-123456789", "123456789"  },
                new [] { "longNullableParam", "0", ""  },
                new [] { "sbyteParam", "-127", "127"  },
                new [] { "sbyteNullableParam", "0", ""  },
                new [] { "floatParam", "-3,141592", "3,141592"  },
                new [] { "floatNullableParam", "0", ""  },
                new [] { "ushortParam", "123", "65535"  },
                new [] { "ushortNullableParam", "0", ""  },
                new [] { "uintParam", "123456789", "978654321"  },
                new [] { "uintNullableParam", "0", ""  },
                new [] { "ulongParam", "123456789123", "987654321987"  },
                new [] { "ulongNullableParam", "0", ""  }
            };

            var fileContent = new[] {
                string.Join("\t", testData.Select(item => item[0])),
                string.Join("\t", testData.Select(item => item[1])),
                string.Join("\t", testData.Select(item => item[2])),
            };

            var parser = new CSVParser(true, '\t', null);

            var table = parser.CreateFromCsv(fileContent);

            Assert.That(table.Columns, Is.EquivalentTo(testData.Select(item => item[0])));
            Assert.That(table.Rows[0], Is.EquivalentTo(testData.Select(item => item[1])));
            Assert.That(table.Rows[1], Is.EquivalentTo(testData.Select(item => item[2])));
        }
    }
}
