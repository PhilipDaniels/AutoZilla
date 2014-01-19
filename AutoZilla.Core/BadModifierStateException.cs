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
        public BadModifierStateException()
        { 
        }

        public BadModifierStateException(string message)
            : base(message)
        {
        }

        public BadModifierStateException(string message, Exception innerException)
            : base(message, innerException)
        { 
        }

        protected BadModifierStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
