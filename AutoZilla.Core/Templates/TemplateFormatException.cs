using MiscUtils.Framework;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// This exception type is thrown when there is a problem with the format
    /// of a template.
    /// </summary>
    [Serializable]
    public class TemplateFormatException : Exception
    {
        /// <summary>
        /// The text of the template. Can be null.
        /// </summary>
        public string TemplateText { get; private set; }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public TemplateFormatException()
        { 
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public TemplateFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="templateText">The text of the template.</param>
        public TemplateFormatException(string message, string templateText)
            : base(message)
        {
            TemplateText = templateText;
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="inner">Inner exception.</param>
        public TemplateFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="templateText">The text of the template.</param>
        /// <param name="inner">Inner exception.</param>
        public TemplateFormatException(string message, string templateText, Exception innerException)
            : base(message, innerException)
        {
            TemplateText = templateText;
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected TemplateFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            TemplateText = info.GetString("TemplateText");
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

            info.AddValue("TemplateText", TemplateText);
            base.GetObjectData(info, context);
        }
    }
}
