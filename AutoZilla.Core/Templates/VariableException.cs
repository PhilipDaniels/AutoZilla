using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// This exception type is thrown when there is a problem with a variable.
    /// This is usually a failure to parse the variable in the first place.
    /// </summary>
    [Serializable]
    public class VariableException : Exception
    {
        public readonly string VariableSpecification;

        public VariableException()
        { 
        }

        public VariableException(string message, string variableSpecification)
            : base(message)
        {
            VariableSpecification = variableSpecification;
        }

        public VariableException(string message, string variableSpecification, Exception innerException)
            : base(message, innerException)
        {
            VariableSpecification = variableSpecification;
        }

        protected VariableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
