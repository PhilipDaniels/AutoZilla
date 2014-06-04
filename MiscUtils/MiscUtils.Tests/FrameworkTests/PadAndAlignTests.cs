// Copyright 2013, 2014 Philip Daniels - http://www.philipdaniels.com/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using MiscUtils.Framework;
using System;
using NUnit.Framework;

namespace MiscUtils.Tests.FrameworkTests
{
    public class PadAndAlignTests
    {
        [Test]
        public void MinWidthCannotBeNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "".PadAndAlign(-1, 3, Alignment.Left, ' '));
        }

        [Test]
        public void MaxWidthCannotBeNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => "".PadAndAlign(2, -1, Alignment.Left, ' '));
        }

        [Test]
        public void MinWidthCanBeZero()
        {
            "".PadAndAlign(0, 3, Alignment.Left, ' ');
        }

        [Test]
        public void MaxWidthCanBeZero()
        {
            "".PadAndAlign(0, 0, Alignment.Left, ' ');
        }

        [Test]
        public void MinWidthMustBeLessThanMaxWidth()
        {
            string text = "";
            Assert.Throws<ArgumentOutOfRangeException>(() => text.PadAndAlign(5, 3, Alignment.Left, ' '));
        }

        [Test]
        public void NullAndEmptyInputStringsAreTreatedTheSame()
        {
            string text1 = null, text2 = "";
            string result1 = text1.PadAndAlign(3, 3, Alignment.Left, '*');
            string result2 = text2.PadAndAlign(3, 3, Alignment.Left, '*');
            Assert.AreEqual("***", result1);
            Assert.AreEqual("***", result2);
        }

        [Test]
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

        [Test]
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

        [Test]
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
