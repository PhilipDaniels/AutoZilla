using System;

namespace AutoZilla.Core.GlobalHotKeys
{
    /// <summary>
    /// Represents possible keyboard modifiers such as Shift, Alt etc.
    /// This is a flags enum.
    /// </summary>
    [Flags]
    public enum Modifiers
    {
        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// The Alt key modifier.
        /// </summary>
        Alt = 0x0001,
        
        /// <summary>
        /// The Control key modifier.
        /// </summary>
        Ctrl = 0x0002,

        /// <summary>
        /// The shift key modifier.
        /// </summary>
        Shift = 0x0004,

        /// <summary>
        /// The Windows key modifier.
        /// </summary>
        Win = 0x0008
    }
}
