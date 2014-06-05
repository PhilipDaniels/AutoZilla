using MiscUtils.Framework;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// This exception type is thrown when there is a problem with a variable.
    /// This is usually a failure to parse the variable in the first place.
    /// </summary>
    [Serializable]
    public class VariableException : Exception
    {
        /// <summary>
        /// The variable specification, i.e. its string represenation.
        /// </summary>
        public string VariableSpecification { get; private set; }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public VariableException()
        { 
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public VariableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="innerException">Inner exception.</param>
        public VariableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="variableSpecification">Textual variable representation.</param>
        public VariableException(string message, string variableSpecification)
            : base(message)
        {
            VariableSpecification = variableSpecification;
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="variableSpecification">Textual variable representation.</param>
        /// <param name="innerException">Inner exception.</param>
        public VariableException(string message, string variableSpecification, Exception innerException)
            : base(message, innerException)
        {
            VariableSpecification = variableSpecification;
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected VariableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            VariableSpecification = info.GetString("VariableSpecification");
        }

        /// <summary>
        /// Override to include our own custom data.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.ThrowIfNull("info");

            info.AddValue("VariableSpecification", VariableSpecification);
            base.GetObjectData(info, context);
        }
    }
}
