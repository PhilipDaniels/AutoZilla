using AutoZilla.Core.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoZilla.Core.Tests.Templates
{
    [TestClass]
    public class TemplateTests
    {
        static readonly string DUMMY_VALUE = "dummy value";

        [TestMethod]
        public void VariableReplacementViaEventsWorks()
        {
            var template = new Template("${DOMAINUSER}");
            template.OnVariableReplacement += VariableReplacementViaEventsWorks_OnVariableReplacement;
            string result = template.Process();
            Assert.AreEqual(DUMMY_VALUE, result);
        }

        void VariableReplacementViaEventsWorks_OnVariableReplacement(object sender, VariableReplacementEventArgs e)
        {
            e.Thing = DUMMY_VALUE;
        }

        [TestMethod]
        public void NoOpVariableReplacementViaEventsWorks()
        {
            // We should get something back because we do not set it in the event.
            var template = new Template("${DOMAINUSER}");
            template.OnVariableReplacement += NoOpVariableReplacementViaEventsWorks_OnVariableReplacement;
            string result = template.Process();
            Assert.AreNotEqual("", result);
        }

        void NoOpVariableReplacementViaEventsWorks_OnVariableReplacement(object sender, VariableReplacementEventArgs e)
        {
        }
    }
}
