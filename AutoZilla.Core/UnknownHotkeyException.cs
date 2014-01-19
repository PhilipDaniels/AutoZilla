using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// This exception type is thrown when the application is asked to find a hotkey
    /// in its list of managed hotkeys, but can't. This typically indicates a program
    /// logic error.
    /// </summary>
    [Serializable]
    public class UnknownHotkeyException : Exception
    {
        public readonly Modifiers Modifiers;
        public readonly Keys Key;

        public UnknownHotkeyException()
        { 
        }

        public UnknownHotkeyException(string message, Modifiers modifiers, Keys key)
            : base(message)
        {
            Modifiers = modifiers;
            Key = key;
        }

        public UnknownHotkeyException(string message, Modifiers modifiers, Keys key, Exception innerException)
            : base(message, innerException)
        {
            Modifiers = modifiers;
            Key = key;
        }

        protected UnknownHotkeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
