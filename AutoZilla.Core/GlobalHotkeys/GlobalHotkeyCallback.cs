﻿
namespace AutoZilla.Core.GlobalHotkeys
{
    /// <summary>
    /// The delegate type for callbacks. They should return void
    /// and accept a <code>ModifiedKey</code> object.
    /// </summary>
    /// <param name="key">The information about your hotkey. Typically this is somewhat redundant
    /// because you will know which hot key/callback combination you are dealing with when you register it,
    /// however it is possible to register several hotkeys with the same callback, in which case
    /// this parameter is useful for distinguishing what was pressed.</param>
    public delegate void HotkeyCallback(ModifiedKey key);
}
