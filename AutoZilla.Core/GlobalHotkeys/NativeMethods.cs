using System;
using System.Runtime.InteropServices;

namespace AutoZilla.Core.GlobalHotKeys
{
    internal static class NativeMethods
    {
        internal const int WM_HotKey_MSG_ID = 0x0312;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        internal static extern short VkKeyScan(char ch);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

        /// <summary>
        /// Returns a handle to the foreground window.
        /// </summary>
        /// <returns>hWnd of the foreground window.</returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Sets the current foreground window (application).
        /// </summary>
        /// <remarks>
        /// For Windows Mobile, replace user32.dll with coredll.dll 
        /// </remarks>
        /// <param name="hWnd">Handle of the window to bring to the foreground.</param>
        /// <returns>True if the operation succeeds.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
    }
}
