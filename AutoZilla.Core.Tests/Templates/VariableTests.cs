using AutoZilla.Core.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscUtils.Framework;
using System;

namespace AutoZilla.Core.Tests.Templates
{
    [TestClass]
    public class VariableTests
    {
        static string NAME1 = "VAR1";

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullSpecificationThrows()
        {
            new Variable(null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void EmptyStringThrows()
        {
            new Variable("");
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void WhitespaceThrows()
        {
            new Variable(" ");
        }

        [TestMethod]
        public void DelimitersAreOptional()
        {
            var v1 = new Variable(NAME1);
            Assert.AreEqual(NAME1, v1.Specification);
            Assert.AreEqual(NAME1, v1.Name);
            Assert.AreEqual("", v1.Pattern);
            Assert.IsNull(v1.Width);

            var v2 = new Variable("${" + NAME1 + "}");
            Assert.AreEqual(NAME1, v2.Specification);
            Assert.AreEqual(NAME1, v2.Name);
            Assert.AreEqual("", v2.Pattern);
            Assert.IsNull(v2.Width);
        }

        [TestMethod]
        public void NameOnlyIsOk()
        {
            var v = new Variable(NAME1);
            Assert.AreEqual(NAME1, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.IsNull(v.Width);

            v = new Variable(NAME1 + "!");
            Assert.AreEqual(NAME1 + "!", v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void NameAndSimpleWidth()
        {
            var spec = NAME1 + "!20";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.AreEqual(20, v.Width.MinWidth);
            Assert.AreEqual(20, v.Width.MaxWidth);
            Assert.AreEqual(' ', v.Width.PadChar);
            Assert.AreEqual(Alignment.Left, v.Width.Alignment);
        }

        [TestMethod]
        public void NameAndSimpleWidthWithPadChar()
        {
            var spec = NAME1 + "!:20";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.AreEqual(20, v.Width.MinWidth);
            Assert.AreEqual(20, v.Width.MaxWidth);
            Assert.AreEqual(':', v.Width.PadChar);
            Assert.AreEqual(Alignment.Left, v.Width.Alignment);

            spec = NAME1 + "!!20";
            v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.AreEqual(20, v.Width.MinWidth);
            Assert.AreEqual(20, v.Width.MaxWidth);
            Assert.AreEqual('!', v.Width.PadChar);
            Assert.AreEqual(Alignment.Left, v.Width.Alignment);
        }

        [TestMethod]
        public void NameAndPatternOnly()
        {
            var spec = NAME1 + ":yyyy";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("yyyy", v.Pattern);
            Assert.IsNull(v.Width);

            spec = NAME1 + ":";
            v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual("", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void PatternOnly()
        {
            var spec = ":yyyy";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual("", v.Name);
            Assert.AreEqual("yyyy", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void PatternCanContainWidthDelimiter()
        {
            var spec = ":yy!yy";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual("", v.Name);
            Assert.AreEqual("yy!yy", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void PatternCanContainPatternDelimiter()
        {
            var spec = ":yy:yy";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual("", v.Name);
            Assert.AreEqual("yy:yy", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void PatternCanContainLeftBracketDelimiter()
        {
            var spec = ":yy{yy";
            var v = new Variable(spec);
            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual("", v.Name);
            Assert.AreEqual("yy{yy", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void PatternCanContainRightBracketDelimiter()
        {
            var spec = "${:yy}yy}";
            var v = new Variable(spec);
            Assert.AreEqual(":yy}yy", v.Specification);
            Assert.AreEqual("", v.Name);
            Assert.AreEqual("yy}yy", v.Pattern);
            Assert.IsNull(v.Width);
        }

        [TestMethod]
        public void WholeThingWithSurroundingDelimiters()
        {
            WholeThing(true);
        }

        [TestMethod]
        public void WholeThingWithoutSurroundingDelimiters()
        {
            WholeThing(false);
        }

        void WholeThing(bool delimiters)
        {
            var WIDTH = "!^17.20";
            var PATTERN = @"yyyy\\=123!\:::}}\}oo";
            var spec = NAME1 + "!" + WIDTH + ":" + PATTERN;
            var fullspec = spec;
            if (delimiters)
                fullspec = "${" + spec + "}";

            var v = new Variable(fullspec);

            Assert.AreEqual(spec, v.Specification);
            Assert.AreEqual(NAME1, v.Name);
            Assert.AreEqual(PATTERN, v.Pattern);
            Assert.AreEqual('!', v.Width.PadChar);
            Assert.AreEqual(Alignment.Center, v.Width.Alignment);
            Assert.AreEqual(17, v.Width.MinWidth);
            Assert.AreEqual(20, v.Width.MaxWidth);
        }

//        static Variable ComplexVariable = new Variable(NAME1 + @"!!^17.20:yyyy\\=123!\:::}}\}oo");
        static Variable ComplexVariable = new Variable(NAME1 + @"!!^17.20:Author");
        static string LITERAL = "Mercy";
        static Variable LiteralVariable = new Variable(":" + LITERAL);

        [TestMethod]
        public void FormattingLiteralWithNullStringReturnsEmptyString()
        {
            string s = LiteralVariable.ApplyFormat(null);
            Assert.AreEqual("", s);
        }

        [TestMethod]
        public void FormattingLiteralWithEmptyStringReturnsPattern()
        {
            string s = LiteralVariable.ApplyFormat("");
            Assert.AreEqual(LITERAL, s);
        }

        [TestMethod]
        public void FormattingNullReturnsEmptyString()
        {
            var v = new Variable(":abcd");
            string s = v.ApplyFormat(null);
            Assert.AreEqual("", s);
        }

        [TestMethod]
        public void FormattingEmptyStringReturnsPatternAndPadding()
        {
            var v = new Variable("!=^10:abcd");
            string s = v.ApplyFormat("");
            Assert.AreEqual("===abcd===", s);
        }

        [TestMethod]
        public void PatternsAreApplied()
        {
            var now = DateTime.Now;
            var nowStr = now.ToString("yyyy-MM-dd");

            var v = new Variable("DATE:yyyy-MM-dd");
            var vs = v.ApplyFormat(now);

            Assert.AreEqual(nowStr, vs);
        }

        [TestMethod]
        public void BlankVariableIsOk()
        {
            // Ok this works, though it appears to be useless!
            // This is not really a requirement, simply documentation
            // that this doesn't cause an exception.
            var v = new Variable("!:");
            string s = v.ApplyFormat(null);
            Assert.AreEqual("", s);
        }

        // There aren't many tests for the ApplyFormat method because
        // most things are tested in the PadAndAlign tests.
    }
}
