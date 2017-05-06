using System;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Should;

namespace Microsoft.Qwiq
{
    [TestClass]
    public class when_parsing_a_value_type_to_nullable_value_type : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = (int?)1;
            Actual = Parser.Parse<int?>(1L);
        }

        [TestMethod]
        public void value_is_converted()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_enum_value : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = Formatting.Indented;
            Actual = Parser.Parse("Indented", Formatting.None);
        }

        [TestMethod]
        public void enum_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_to_nonnullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 11d;
            Actual = Parser.Parse<double>(null, 11);
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_invalid_string_to_nullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = null;
            Actual = Parser.Parse(typeof(double?), (object)"blarg");
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_invalid_string_to_nonnullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 0d;
            Actual = Parser.Parse(typeof(double), (object)"blarg");
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_int64_string_to_int32 : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 0;
            Actual = Parser.Parse(typeof(int), (object)"0x7FFFFFFFFFFFFFFF");
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_null_to_nonnullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 0d;
            Actual = Parser.Parse(typeof(double), null);
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_DateTime_to_nonnullable_DateTime : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = new DateTime(2014, 1, 1);
            Actual = Parser.Parse(typeof(DateTime), null, Expected);
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_an_empty_string_nonnullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 11d;
            Actual = Parser.Parse<double>("", 11);
        }

        [TestMethod]
        public void default_value_is_returned()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nonnullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 7d;
            Actual = Parser.Parse<double>("7");
        }

        [TestMethod]
        public void value_is_parsed_as_double()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_nullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = null;
            Actual = Parser.Parse<double?>(null);
        }

        [TestMethod]
        public void value_is_null()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 7d;
            Actual = Parser.Parse<double?>("7");
        }

        [TestMethod]
        public void value_is_parsed_as_double()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_generic_nullable_double : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 7d;
            Actual = Parser.Parse<double?>("7");
        }

        [TestMethod]
        public void value_is_parsed_as_double()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_nullable_int : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = null;
            Actual = Parser.Parse<int?>(null);
        }

        [TestMethod]
        public void value_is_null()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nullable_int : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 7;
            Actual = Parser.Parse<int?>("7");
        }

        [TestMethod]
        public void value_is_parsed_as_int()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_nullable_date : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = null;
            Actual = Parser.Parse<DateTime?>(null);
        }

        [TestMethod]
        public void value_is_null()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nullable_date : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = DateTime.Today;
            Actual = Parser.Parse<DateTime?>(Expected.ToString());
        }

        [TestMethod]
        public void value_is_parsed_as_date()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_nonnullable_string : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = "";
            Actual = Parser.Parse<string>(null, "");
        }

        [TestMethod]
        public void value_is_empty_string()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nonnullable_string : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = "test string";
            Actual = Parser.Parse<string>("test string");
        }

        [TestMethod]
        public void value_is_parsed_as_string()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_null_nonnullable_int : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 14;
            Actual = Parser.Parse<int>(null, 14);
        }

        [TestMethod]
        public void value_is_default()
        {
            Actual.ShouldEqual(Expected);
        }
    }

    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class when_parsing_a_valid_nonnullable_int : TypeParserTestsContext
    {
        public override void When()
        {
            Expected = 7;
            Actual = Parser.Parse<int>(7);
        }

        [TestMethod]
        public void value_is_parsed_as_int()
        {
            Actual.ShouldEqual(Expected);
        }
    }
}
