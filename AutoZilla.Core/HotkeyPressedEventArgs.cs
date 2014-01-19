using AutoZilla.Core.GlobalHotkeys;
using System;

namespace AutoZilla.Core
{
    /// <summary>
    /// The EventArgs type used by the message loop form to tell the
    /// hot key manager that a hot key has been pressed.
    /// </summary>
    internal class HotkeyPressedEventArgs : EventArgs
    {
        public readonly ModifiedKey HotkeyInfo;

        public HotkeyPressedEventArgs(ModifiedKey hotkeyInfo)
        {
            HotkeyInfo = hotkeyInfo;
        }
    }
}
