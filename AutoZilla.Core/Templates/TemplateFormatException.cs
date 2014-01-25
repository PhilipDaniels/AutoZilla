using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// This exception type is thrown when there is a problem with the format
    /// of a template.
    /// </summary>
    [Serializable]
    public class TemplateFormatException : Exception
    {
        public string TemplateText { get; private set; }

        public TemplateFormatException()
        { 
        }

        public TemplateFormatException(string message, string templateText)
            : base(message)
        {
            TemplateText = templateText;
        }

        public TemplateFormatException(string message, string templateText, Exception innerException)
            : base(message, innerException)
        {
            TemplateText = templateText;
        }

        protected TemplateFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
