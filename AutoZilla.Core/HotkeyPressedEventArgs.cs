using AutoZilla.Core.GlobalHotKeys;
using System;

namespace AutoZilla.Core
{
    /// <summary>
    /// The EventArgs type used by the message loop form to tell the
    /// hot key manager that a hot key has been pressed.
    /// </summary>
    internal class HotKeyPressedEventArgs : EventArgs
    {
        /// <summary>
        /// The <code>ModifiedKey</code> that was pressed.
        /// </summary>
        public ModifiedKey ModifiedKey { get; private set; }

        /// <summary>
        /// Construct a new <code>HotKeyPressedEventArgs</code> object.
        /// </summary>
        /// <param name="modifiedKey">The <code>ModifiedKey</code> that was pressed.</param>
        public HotKeyPressedEventArgs(ModifiedKey modifiedKey)
        {
            ModifiedKey = modifiedKey;
        }
    }
}
