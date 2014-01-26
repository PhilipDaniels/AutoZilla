using AutoZilla.Core.GlobalHotkeys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Core.Tests.GlobalHotKeys
{
    [TestClass]
    public class ModifiedKeyTests
    {
        [TestMethod]
        public void TestResolve()
        {
            Keys k1 = ModifiedKey.ResolveKey('a');
            Keys k2 = ModifiedKey.ResolveKey(';');
            Keys k3 = ModifiedKey.ResolveKey('5');
        }
    }
}
