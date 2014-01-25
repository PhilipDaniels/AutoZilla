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
    public class AutoZillaVariableTests
    {
        [TestMethod]
        public void GetByNameIsCaseInsensitive()
        {
            object value = AutoZillaVariables.GetByName("machinename");
            Assert.AreEqual(Environment.MachineName, value);
        }
    }
}
