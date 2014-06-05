using AutoZilla.Core.GlobalHotKeys;
using MiscUtils.Framework;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// This exception type is thrown when the application is asked to find a HotKey
    /// in its list of managed HotKeys, but can't. This typically indicates a program
    /// logic error.
    /// </summary>
    [Serializable]
    public class UnknownHotKeyException : Exception
    {
        public Modifiers Modifiers { get; private set; }
        public Keys Key { get; private set; }

        public UnknownHotKeyException()
        { 
        }

        public UnknownHotKeyException(string message)
            : base(message)
        {
        }

        public UnknownHotKeyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UnknownHotKeyException(string message, Modifiers modifiers, Keys key)
            : base(message)
        {
            Modifiers = modifiers;
            Key = key;
        }

        public UnknownHotKeyException(string message, Modifiers modifiers, Keys key, Exception innerException)
            : base(message, innerException)
        {
            Modifiers = modifiers;
            Key = key;
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected UnknownHotKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Modifiers = (Modifiers)info.GetValue("Modifiers", typeof(Modifiers));
            Key = (Keys)info.GetValue("Key", typeof(Keys));
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

            info.AddValue("Modifiers", Modifiers);
            info.AddValue("Key", Key);
            base.GetObjectData(info, context);
        }
    }
}
