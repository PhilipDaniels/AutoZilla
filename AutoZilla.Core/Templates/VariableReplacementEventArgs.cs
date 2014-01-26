using AutoZilla.Core.GlobalHotkeys;
using AutoZilla.Core.Templates;
using System;

namespace AutoZilla.Core
{
    /// <summary>
    /// The EventArgs type used by the message loop form to tell the
    /// hot key manager that a hot key has been pressed.
    /// </summary>
    public class VariableReplacementEventArgs : EventArgs
    {
        public string Variable { get; private set; }
        public object Thing { get; set; }

        public VariableReplacementEventArgs(string variableName)
        {
            Variable = variableName;
            Thing = null;
        }
    }
}
