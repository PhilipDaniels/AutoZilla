using System;
using System.Runtime.Serialization;

namespace AutoZilla.Core
{
    /// <summary>
    /// This exception type is thrown when you try and send Ctrl-V or other
    /// keychords including modifiers while the modifers are still down.
    /// (In other words, you can't send Ctrl-V if the Ctrl key is already down).
    /// </summary>
    [Serializable]
    public class BadModifierStateException : Exception
    {
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public BadModifierStateException()
        { 
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public BadModifierStateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="innerException">Inner exception.</param>
        public BadModifierStateException(string message, Exception innerException)
            : base(message, innerException)
        { 
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected BadModifierStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
