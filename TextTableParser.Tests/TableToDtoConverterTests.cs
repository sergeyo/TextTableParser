using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TextTableParser.Tests.TestData;

namespace TextTableParser.Tests
{
    [TestFixture(Category = "Unit")]
    public class TableToDtoConverterTests
    {
        private const double PI = 3.1415926538;
        private const float PIf = 3.141592f;

        [Test]
        public void Convert_WhenCorrectTableAndDtoTypePassed_ShouldReturnDtos()
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
                new [] { "doubleParam", (-PI).ToString(CultureInfo.InvariantCulture), PI.ToString(CultureInfo.InvariantCulture)  },
                new [] { "doubleNullableParam", "0", ""  },
                new [] { "shortParam", "-32767", "32767"  },
                new [] { "shortNullableParam", "0", ""  },
                new [] { "intParam", "-2000000000", "2000000000"  },
                new [] { "intNullableParam", "0", ""  },
                new [] { "longParam", "-123456789", "123456789"  },
                new [] { "longNullableParam", "0", ""  },
                new [] { "sbyteParam", "-127", "127"  },
                new [] { "sbyteNullableParam", "0", ""  },
                new [] { "floatParam", (-PIf).ToString(CultureInfo.InvariantCulture), PIf.ToString(CultureInfo.InvariantCulture)  },
                new [] { "floatNullableParam", "0", ""  },
                new [] { "ushortParam", "123", "65535"  },
                new [] { "ushortNullableParam", "0", ""  },
                new [] { "uintParam", "123456789", "978654321"  },
                new [] { "uintNullableParam", "0", ""  },
                new [] { "ulongParam", "123456789123", "987654321987"  },
                new [] { "ulongNullableParam", "0", ""  }
            };

            var table = new Table()
            {
                Columns = testData.Select(item => item[0]).ToArray(),
                Rows = new List<string[]>() {
                    testData.Select(item => item[1]).ToArray(),
                    testData.Select(item => item[2]).ToArray(),
                }
            };

            var converter = new TableToDtoConverterFactory().GetParser<TestDtoWithAllSupportedTypes>();

            var dtos = converter.Convert(table).ToArray();

            Assert.That(dtos[0].stringParam, Is.EqualTo("stringParam1"));
            Assert.That(dtos[1].stringParam, Is.EqualTo("stringParam2"));
            Assert.That(dtos[0].boolParam, Is.EqualTo(true));
            Assert.That(dtos[1].boolParam, Is.EqualTo(false));
            Assert.That(dtos[0].boolNullableParam, Is.EqualTo(false));
            Assert.That(dtos[1].boolNullableParam, Is.EqualTo((bool?)null));
            Assert.That(dtos[0].byteParam, Is.EqualTo(0));
            Assert.That(dtos[1].byteParam, Is.EqualTo(255));
            Assert.That(dtos[0].byteNullableParam, Is.EqualTo(128));
            Assert.That(dtos[1].byteNullableParam, Is.EqualTo((byte?)null));
            Assert.That(dtos[0].charParam, Is.EqualTo('A'));
            Assert.That(dtos[1].charParam, Is.EqualTo('Z'));
            Assert.That(dtos[0].charNullableParam, Is.EqualTo('$'));
            Assert.That(dtos[1].byteNullableParam, Is.EqualTo((char?)null));
            Assert.That(dtos[0].DateTimeParam, Is.EqualTo(new DateTime(2017, 1, 1)));
            Assert.That(dtos[1].DateTimeParam, Is.EqualTo(new DateTime(2015, 12, 31)));
            Assert.That(dtos[0].DateTimeNullableParam, Is.EqualTo(new DateTime(1999, 1, 1)));
            Assert.That(dtos[1].DateTimeNullableParam, Is.EqualTo((DateTime?)null));
            Assert.That(dtos[0].decimalParam, Is.EqualTo(-100000m));
            Assert.That(dtos[1].decimalParam, Is.EqualTo(10000000m));
            Assert.That(dtos[0].decimalNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].decimalNullableParam, Is.EqualTo((decimal?)null));
            Assert.That(dtos[0].doubleParam, Is.EqualTo(-3.1415926538));
            Assert.That(dtos[1].doubleParam, Is.EqualTo(3.1415926538));
            Assert.That(dtos[0].doubleNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].doubleNullableParam, Is.EqualTo((double?)null));
            Assert.That(dtos[0].shortParam, Is.EqualTo(-32767));
            Assert.That(dtos[1].shortParam, Is.EqualTo(32767));
            Assert.That(dtos[0].shortNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].shortNullableParam, Is.EqualTo((short?)null));
            Assert.That(dtos[0].intParam, Is.EqualTo(-2000000000));
            Assert.That(dtos[1].intParam, Is.EqualTo(2000000000));
            Assert.That(dtos[0].intNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].intNullableParam, Is.EqualTo((int?)null));
            Assert.That(dtos[0].longParam, Is.EqualTo(-123456789));
            Assert.That(dtos[1].longParam, Is.EqualTo(123456789));
            Assert.That(dtos[0].longNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].longNullableParam, Is.EqualTo((long?)null));
            Assert.That(dtos[0].sbyteParam, Is.EqualTo(-127));
            Assert.That(dtos[1].sbyteParam, Is.EqualTo(127));
            Assert.That(dtos[0].sbyteNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].sbyteNullableParam, Is.EqualTo((sbyte?)null));
            Assert.That(dtos[0].floatParam, Is.EqualTo(-3.141592f));
            Assert.That(dtos[1].floatParam, Is.EqualTo(3.141592f));
            Assert.That(dtos[0].floatNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].floatNullableParam, Is.EqualTo((float?)null));
            Assert.That(dtos[0].ushortParam, Is.EqualTo(123));
            Assert.That(dtos[1].ushortParam, Is.EqualTo(65535));
            Assert.That(dtos[0].ushortNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].ushortNullableParam, Is.EqualTo((ushort?)null));
            Assert.That(dtos[0].uintParam, Is.EqualTo(123456789));
            Assert.That(dtos[1].uintParam, Is.EqualTo(978654321));
            Assert.That(dtos[0].uintNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].uintNullableParam, Is.EqualTo((uint?)null));
            Assert.That(dtos[0].ulongParam, Is.EqualTo(123456789123));
            Assert.That(dtos[1].ulongParam, Is.EqualTo(987654321987));
            Assert.That(dtos[0].ulongNullableParam, Is.EqualTo(0));
            Assert.That(dtos[1].ulongNullableParam, Is.EqualTo((ulong?)null));
        }
    }
}
