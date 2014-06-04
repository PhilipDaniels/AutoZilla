using MiscUtils.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscUtils.Tests.FrameworkTests
{
    /*
    [TestClass]
    public class IndentingStringBuilderTests
    {
        [TestMethod]
        public void test_empty_sb()
        {
            var sb = new IndentingStringBuilder();
            var s = sb.ToString();
            Assert.AreEqual(s, "");
        }

        [TestMethod]
        public void test_one_line_with_no_indentation()
        {
            var sb = new IndentingStringBuilder();
            sb.Append("Hello world");
            var s = sb.ToString();
            Assert.AreEqual(s, "Hello world");
        }

        [TestMethod]
        public void test_one_line_with_indentation()
        {
            var sb = new IndentingStringBuilder();
            sb.IndentationLevel++;
            sb.Append("Hello world");
            var s = sb.ToString();
            Assert.AreEqual(s, "    Hello world");
        }

        [TestMethod]
        public void test_two_lines_with_indentation()
        {
            var sb = new IndentingStringBuilder();
            sb.IndentationLevel++;
            sb.AppendLine("Hello");
            sb.Append("world");
            var s = sb.ToString();
            Assert.AreEqual(s, "    Hello" + Environment.NewLine + "    world");
        }

        [TestMethod]
        public void test_in_and_out()
        {
            var sb = new IndentingStringBuilder();
            sb.AppendLine("LEVEL1").Indent();
            sb.AppendLine("LEVEL2");
            sb.AppendLine("LEVEL2").Indent();
            sb.AppendLine("LEVEL3");
            sb.AppendLine("LEVEL3").OutDent();
            sb.AppendLine("LEVEL2").OutDent();
            sb.AppendLine("LEVEL1");

            var s = sb.ToString();
            var expected =
                "LEVEL1" + Environment.NewLine +
                "    LEVEL2" + Environment.NewLine +
                "    LEVEL2" + Environment.NewLine +
                "        LEVEL3" + Environment.NewLine +
                "        LEVEL3" + Environment.NewLine +
                "    LEVEL2" + Environment.NewLine +
                "LEVEL1" + Environment.NewLine;

            Assert.AreEqual(s, expected);
        }
    }
    */
}
