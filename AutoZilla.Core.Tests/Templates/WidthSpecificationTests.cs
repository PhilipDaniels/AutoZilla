using AutoZilla.Core.Extensions;
using AutoZilla.Core.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZilla.Core.Tests.Templates
{
    [TestClass]
    public class WidthSpecificationTests
    {
        [TestMethod]
        public void NullAndEmptyStringAreEquivalentWidthSpecifications()
        {
            var a = new WidthSpecification();
            var b = new WidthSpecification("");
            var c = new WidthSpecification(" ");
            var d = new WidthSpecification("  ");
            Assert.AreEqual(a, b);
            Assert.AreEqual(b, c);
            Assert.AreEqual(c, d);

            // This essentially constrains the defaults.
            Assert.AreEqual(' ', a.PadChar);
            Assert.AreEqual(Alignment.Left, a.Alignment);
            Assert.AreEqual(0, a.MinWidth);
            Assert.AreEqual(Int32.MaxValue, a.MaxWidth);
        }

        [TestMethod]
        public void MissingPadCharDefaultsToSpace()
        {
            var ws = new WidthSpecification(null);
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification("");
            Assert.AreEqual(' ', ws.PadChar);
            
            ws = new WidthSpecification("<");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification(">");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification("^");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification("<5");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification(">5");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification("^5");
            Assert.AreEqual(' ', ws.PadChar);

            ws = new WidthSpecification("5");
            Assert.AreEqual(' ', ws.PadChar);
        }

        IEnumerable<Alignment> ValidAlignments
        {
            get
            {
                yield return Alignment.Left;
                yield return Alignment.Center;
                yield return Alignment.Right;
            }
        }

        [TestMethod]
        public void SingleCharacterInterpretedCorrectly()
        {
            // If the single char is an alignment character, check
            // that it is detected properly.
            WidthSpecification ws;

            foreach (var alignment in ValidAlignments)
            {
                var c = (char)alignment;
                ws = new WidthSpecification(c.ToString());
                Assert.AreEqual(' ', ws.PadChar);
                Assert.AreEqual(alignment, ws.Alignment);
                Assert.AreEqual(0, ws.MinWidth);
                Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);
            }

            // If the single char is a digit it is taken to be a width.
            for (int i = 0; i <= 9; i++)
            {
                var c = i.ToString();
                ws = new WidthSpecification(c);
                Assert.AreEqual(' ', ws.PadChar);
                Assert.AreEqual(Alignment.Left, ws.Alignment);
                Assert.AreEqual(i, ws.MinWidth);
                Assert.AreEqual(i, ws.MaxWidth);
            }

            // Case of single char being a space is handled in the
            // test NullAndEmptyStringAreEquivalentWidthSpecifications().

            // Some representative pad chars.
            foreach (var c in OnlyPadChars)
            {
                ws = new WidthSpecification(c.ToString());
                Assert.AreEqual(c, ws.PadChar);
                Assert.AreEqual(Alignment.Left, ws.Alignment);
                Assert.AreEqual(0, ws.MinWidth);
                Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);
            }
        }

        IEnumerable<char> OnlyAlignmentChars
        {
            get
            {
                foreach (var alignment in ValidAlignments)
                {
                    yield return (char)alignment;
                }
            }
        }

        IEnumerable<char> OnlyPadChars
        {
            get
            {
                foreach (var c in new char[] { '=', 'a', '*', 'X', '@', '\'', '!' })
                    yield return c;
            }
        }

        IEnumerable<char> OnlyDigitChars
        {
            get
            {
                foreach (var c in new char[] { '0', '1', '2', '9' })
                    yield return c;
            }
        }

        IEnumerable<char> AssortedChars
        {
            get
            {
                foreach (var c in OnlyAlignmentChars)
                    yield return c;
                foreach (var c in OnlyPadChars)
                    yield return c;
                foreach (var c in OnlyDigitChars)
                    yield return c;
            }
        }

        [TestMethod]
        public void PadCharFollowedByWidthInterpretedCorrectly()
        {
            WidthSpecification ws;

            foreach (var padChar in OnlyPadChars)
            {
                var s = padChar.ToString() + "20";
                ws = new WidthSpecification(s);
                Assert.AreEqual(ws.PadChar, padChar);
                Assert.AreEqual(Alignment.Left, ws.Alignment);
                Assert.AreEqual(20, ws.MinWidth);
                Assert.AreEqual(20, ws.MaxWidth);
            }
        }

        [TestMethod]
        public void PadCharFollowedByAlignmentInterpretedCorrectly()
        {
            PadCharFollowedByAlignmentInterpretedCorrectly(null);
            PadCharFollowedByAlignmentInterpretedCorrectly(0);
            PadCharFollowedByAlignmentInterpretedCorrectly(1);
            PadCharFollowedByAlignmentInterpretedCorrectly(5);
        }

        void PadCharFollowedByAlignmentInterpretedCorrectly(int? width)
        {
            WidthSpecification ws;

            foreach (var padChar in AssortedChars)
            {
                foreach (var alignmentChar in OnlyAlignmentChars)
                {
                    var s = padChar.ToString() + alignmentChar.ToString();
                    if (width != null)
                        s += width.ToString();

                    ws = new WidthSpecification(s);
                    Assert.AreEqual(ws.PadChar, padChar);
                    Assert.AreEqual(alignmentChar, (char)ws.Alignment);
                    if (width == null)
                    {
                        Assert.AreEqual(0, ws.MinWidth);
                        Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);
                    }
                    else
                    {
                        Assert.AreEqual(width.Value, ws.MinWidth);
                        Assert.AreEqual(width.Value, ws.MaxWidth);
                    }
                }
            }
        }

        [TestMethod]
        public void NumberOnlyInterpretedAsBothMinAndMax()
        {
            WidthSpecification ws;

            ws = new WidthSpecification("0");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(0, ws.MaxWidth);

            ws = new WidthSpecification("9");
            Assert.AreEqual(9, ws.MinWidth);
            Assert.AreEqual(9, ws.MaxWidth);

            ws = new WidthSpecification("20");
            Assert.AreEqual(20, ws.MinWidth);
            Assert.AreEqual(20, ws.MaxWidth);
        }

        [TestMethod]
        public void MissingMaxDefaultsCorrectly()
        {
            WidthSpecification ws;

            ws = new WidthSpecification("0.");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);

            ws = new WidthSpecification("9.");
            Assert.AreEqual(9, ws.MinWidth);
            Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);

            ws = new WidthSpecification("20.");
            Assert.AreEqual(20, ws.MinWidth);
            Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);
        }

        [TestMethod]
        public void LeadingDotIsInterpretedAsMinMaxDivider()
        {
            WidthSpecification ws;

            // This is ambiguous.
            // New rule: to specify a pad char of '.'
            // you must have an alignment.
            ws = new WidthSpecification(".0");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(0, ws.MaxWidth);

            ws = new WidthSpecification(".9");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(9, ws.MaxWidth);

            ws = new WidthSpecification(".20");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(20, ws.MaxWidth);
        }

        [TestMethod]
        public void MissingMinDefaultsCorrectly()
        {
            WidthSpecification ws;

            // This is ambiguous.
            // New rule: to specify a pad char of '.'
            // you must have an alignment.
            ws = new WidthSpecification("=.0");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(0, ws.MaxWidth);

            ws = new WidthSpecification("=.9");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(9, ws.MaxWidth);

            ws = new WidthSpecification("=.20");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(20, ws.MaxWidth);
        }

        [TestMethod]
        public void MissingMinAndMaxDefaultsCorrectly()
        {
            WidthSpecification ws;

            ws = new WidthSpecification(".");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(Int32.MaxValue, ws.MaxWidth);
        }



        [TestMethod]
        public void WhitespaceIsNotSignificant()
        {
            WidthSpecification ws;

            ws = new WidthSpecification("  .0  ");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(0, ws.MaxWidth);

            ws = new WidthSpecification("  .9 ");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(9, ws.MaxWidth);

            ws = new WidthSpecification("  .20  ");
            Assert.AreEqual(0, ws.MinWidth);
            Assert.AreEqual(20, ws.MaxWidth);
        }
    }
}
