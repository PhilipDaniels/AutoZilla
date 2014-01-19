using System;
using AutoZilla.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoZilla.Core.Templates;

namespace AutoZilla.Core.Tests.Extensions
{
    [TestClass]
    public class PadAndAlignTests
    {
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MinWidthCannotBeNegative()
        {
            "".PadAndAlign(-1, 3, Alignment.Left, ' ');
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MaxWidthCannotBeNegative()
        {
            "".PadAndAlign(2, -1, Alignment.Left, ' ');
        }

        [TestMethod]
        public void MinWidthCanBeZero()
        {
            "".PadAndAlign(0, 3, Alignment.Left, ' ');
        }

        [TestMethod]
        public void MaxWidthCanBeZero()
        {
            "".PadAndAlign(0, 0, Alignment.Left, ' ');
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MinWidthMustBeLessThanMaxWidth()
        {
            string text = "";
            text.PadAndAlign(5, 3, Alignment.Left, ' ');
        }

        [TestMethod]
        public void NullAndEmptyInputStringsAreTreatedTheSame()
        {
            string text1 = null, text2 = "";
            string result1 = text1.PadAndAlign(3, 3, Alignment.Left, '*');
            string result2 = text2.PadAndAlign(3, 3, Alignment.Left, '*');
            Assert.AreEqual("***", result1);
            Assert.AreEqual("***", result2);
        }

        [TestMethod]
        public void AlignLeftWorks()
        {
            AlignLeftTest(null, "***");
            AlignLeftTest("", "***");
            AlignLeftTest("a", "a**");
            AlignLeftTest("ab", "ab*");
            AlignLeftTest("abc", "abc");
            AlignLeftTest("abcd", "abcd");
            AlignLeftTest("abcde", "abcde");
            AlignLeftTest("abcdef", "abcde");
        }

        void AlignLeftTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, Alignment.Left, '*');
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AlignRightWorks()
        {
            AlignRightTest(null, "***");
            AlignRightTest("", "***");
            AlignRightTest("a", "**a");
            AlignRightTest("ab", "*ab");
            AlignRightTest("abc", "abc");
            AlignRightTest("abcd", "abcd");
            AlignRightTest("abcde", "abcde");
            AlignRightTest("abcdef", "bcdef");
        }

        void AlignRightTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, Alignment.Right, '*');
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AlignCenterWorks()
        {
            AlignCenterTest(null, "***");
            AlignCenterTest("", "***");
            AlignCenterTest("a", "*a*");
            AlignCenterTest("ab", "ab*");
            AlignCenterTest("abc", "abc");
            AlignCenterTest("abcd", "abcd");
            AlignCenterTest("abcde", "abcde");
            AlignCenterTest("abcdef", "abcde");
            // one more case here compared to left and right.
            AlignCenterTest("abcdefg", "bcdef");
        }

        void AlignCenterTest(string text, string expected)
        {
            string result = text.PadAndAlign(3, 5, Alignment.Center, '*');
            Assert.AreEqual(expected, result);
        }
    }
}
