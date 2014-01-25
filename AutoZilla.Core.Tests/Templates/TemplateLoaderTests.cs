using AutoZilla.Core.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoZilla.Core.Tests.Templates
{
    [TestClass]
    public class TemplateLoaderTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullContentThrowsArgumentNullException()
        {
            Template t = TemplateLoader.LoadTemplateFromString(null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void EmptyContentThrowsArgumentOutOfRangeException()
        {
            Template t = TemplateLoader.LoadTemplateFromString("  ");
        }

        [ExpectedException(typeof(TemplateFormatException))]
        [TestMethod]
        public void EmptyBodyThrowsTemplateFormatException()
        {
            string body = @"[Config];;AZ;;";

            Template t = TemplateLoader.LoadTemplateFromString(body);
        }

        [ExpectedException(typeof(TemplateFormatException))]
        [TestMethod]
        public void EmptyConfigThrowsTemplateFormatException()
        {
            string body = @";;AZ;;body";

            Template t = TemplateLoader.LoadTemplateFromString(body);
        }

        [TestMethod]
        public void BlankConfigLoadsOk()
        {
            string body = @"[Config];;AZ;;body";

            Template t = TemplateLoader.LoadTemplateFromString(body);
            Assert.IsNull(t.Name);
            Assert.IsNull(t.Key);
            Assert.IsNull(t.Description);
            Assert.IsNull(t.Filename);
            Assert.IsNull(t.FilePath);
            Assert.AreEqual("body", t.OriginalText);
        }

        [ExpectedException(typeof(FileNotFoundException))]
        [TestMethod]
        public void NonExistentFileThrowsException()
        {
            Template t = TemplateLoader.LoadTemplate(@"I:\Definitely\Do\Not\Exist");
        }

        [ExpectedException(typeof(DirectoryNotFoundException))]
        [TestMethod]
        public void NonExistentDirectoryThrowsException()
        {
            IEnumerable<Template> templates = TemplateLoader.LoadTemplates(@"I:\Definitely\Do\Not\Exist");
        }

    }
}
