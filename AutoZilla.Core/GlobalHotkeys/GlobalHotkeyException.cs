using System;
using System.Runtime.Serialization;

namespace AutoZilla.Core.GlobalHotKeys
{
    /// <summary>
    /// Exception used when there is a problem registering or unregistering
    /// a <code>GlobalHotKey</code>.
    /// </summary>
    [Serializable]
    public class GlobalHotKeyException : Exception
    {
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public GlobalHotKeyException() : base()
        { 
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public GlobalHotKeyException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="inner">Inner exception.</param>
        public GlobalHotKeyException(string message, Exception inner) 
            : base(message, inner)
        {
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected GlobalHotKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
