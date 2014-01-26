using System;

namespace AutoZilla.Core
{
    /// <summary>
    /// This EventArgs type is used by the <code>Template</code> class when it is replacing
    /// variable values. Set <code>Thing</code> to a non-null value to have that value
    /// formatted and placed into the template. Setting <code>Thing</code> to <code>String.Empty</code>
    /// will "blank out" the variable in the template.
    /// </summary>
    public class VariableReplacementEventArgs : EventArgs
    {
        /// <summary>
        /// The name of the variable.
        /// </summary>
        public string Variable { get; private set; }

        /// <summary>
        /// The Thing that you want to set the variable to. If the variable is a built-in
        /// variable then it will be initialised to AutoZilla's default value for that variable.
        /// </summary>
        public object Thing { get; set; }

        /// <summary>
        /// Construct a new <code>VariableReplacementEventArgs</code> object.
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="thing">The value of the thing.</param>
        public VariableReplacementEventArgs(string variableName, object thing)
        {
            Variable = variableName;
            Thing = thing;
        }
    }
}
