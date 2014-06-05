using MiscUtils.Framework;
using System;
using System.Windows.Forms;

namespace AutoZilla.Core.GlobalHotKeys
{
    /// <summary>
    /// Represents a Global Hot Key.
    /// </summary>
    internal sealed class GlobalHotKey : IDisposable
    {
        /// <summary>
        /// The modifiers that the <code>GlobalHotKey</code> was created with.
        /// </summary>
        public Modifiers Modifier { get; private set; }

        /// <summary>
        /// The key code that the <code>GlobalHotKey</code> was created with.
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Internal identifier of the <code>GlobalHotKey</code>.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Delegate to be called when the <code>GlobalHotKey</code> is pressed.
        /// </summary>
        public HotKeyCallback Callback { get; private set; }

        IntPtr hWnd;
        bool registered;

        /// <summary>
        /// Creates a GlobalHotKey object.
        /// </summary>
        /// <param name="modifier">HotKey modifier keys</param>
        /// <param name="key">HotKey</param>
        /// <param name="window">The Window that the HotKey should be registered to</param>
        /// <param name="callback">The delegate to call when the HotKey is pressed.</param>
        /// <param name="registerImmediately"> </param>
        public GlobalHotKey(Modifiers modifier, Keys key, IWin32Window window, HotKeyCallback callback, bool registerImmediately = false)
        {
            window.ThrowIfNull("window", "You must provide a form or window to register the HotKey against.");
            callback.ThrowIfNull("callback", "You must specify a callback in order to do some useful work when your HotKey is pressed.");

            Modifier = modifier;
            Key = (int)key;
            hWnd = window.Handle;
            Id = GetHashCode();
            Callback = callback;

            if (registerImmediately)
                Register();
        }

        /// <summary>
        /// Registers the current HotKey with Windows.
        /// Note! You must override the WndProc method in your window that registers the HotKey,
        /// or you will not receive any HotKey notifications.
        /// </summary>
        public void Register()
        {
            if (!NativeMethods.RegisterHotKey(hWnd, Id, (int)Modifier, Key))
                throw new GlobalHotKeyException("HotKey failed to register.");
            registered = true;
        }

        /// <summary>
        /// Unregisters the current HotKey with Windows.
        /// </summary>
        public void Unregister()
        {
            if (!registered) return;
            if (!NativeMethods.UnregisterHotKey(hWnd, Id))
                throw new GlobalHotKeyException("HotKey failed to unregister.");
            registered = false;
        }

        /// <summary>
        /// Disposes the hot key by unregistering it.
        /// </summary>
        public void Dispose()
        {
            Unregister();
            GC.SuppressFinalize(this);
        }

        ~GlobalHotKey()
        {
            Unregister();
        }

        /// <summary>
        /// Returns the hash code of the hot key.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override sealed int GetHashCode()
        {
            return (int)Modifier ^ Key ^ hWnd.ToInt32();
        }
    }
}
