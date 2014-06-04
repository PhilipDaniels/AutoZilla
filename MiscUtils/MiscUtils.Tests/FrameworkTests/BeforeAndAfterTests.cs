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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MiscUtils.Tests.FrameworkTests
{
    public class BeforeAndAfterTests
    {
        [Test]
        public void BeforeOnNullReturnsNull()
        {
            string s = null;
            string result = s.Before("foo");
            Assert.Null(result);
        }

        [Test]
        public void AfterOnNullReturnsNull()
        {
            string s = null;
            string result = s.After("foo");
            Assert.Null(result);
        }

        [Test]
        public void BeforeNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.Throws<ArgumentNullException>(() => s.Before(null));
        }

        [Test]
        public void AfterNullThrowsArgumentNullException()
        {
            string s = "foo";
            Assert.Throws<ArgumentNullException>(() => s.After(null));
        }

        [Test]
        public void BeforeNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.Before("bar");
            Assert.Null(result);
        }

        [Test]
        public void AfterNotFoundStringReturnsNull()
        {
            string s = "foo";
            string result = s.After("bar");
            Assert.Null(result);
        }

        [Test]
        public void BeforeCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("foo", result);
        }

        [Test]
        public void AfterCaseSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCulture);
            Assert.AreEqual("FOO", result);
        }

        [Test]
        public void BeforeCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.Before("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("", result);
        }

        [Test]
        public void AfterCaseInSensitive()
        {
            string s = "fooFooFOO";
            string result = s.After("Foo", StringComparison.InvariantCultureIgnoreCase);
            Assert.AreEqual("FooFOO", result);
        }

        [Test]
        public void BeforeBeginningOfString()
        {
            string s = "abc";
            string result = s.Before("a");
            Assert.AreEqual("", result);
        }

        [Test]
        public void AfterBeginningOfString()
        {
            string s = "abc";
            string result = s.After("a");
            Assert.AreEqual("bc", result);
        }

        [Test]
        public void BeforeEndOfString()
        {
            string s = "abc";
            string result = s.Before("c");
            Assert.AreEqual("ab", result);
        }

        [Test]
        public void AfterEndOfString()
        {
            string s = "abc";
            string result = s.After("c");
            Assert.AreEqual("", result);
        }

        [Test]
        public void BeforeAndAfter()
        {
            string s = "abc;;AZ;;def";
            string before, after;
            s.BeforeAndAfter(";;AZ;;", StringComparison.InvariantCultureIgnoreCase, out before, out after);
            Assert.AreEqual("abc", before);
            Assert.AreEqual("def", after);
        }
    }
}
