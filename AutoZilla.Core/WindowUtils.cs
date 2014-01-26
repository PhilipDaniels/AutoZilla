using System;
using System.Runtime.InteropServices;

namespace AutoZilla.Core
{
    static class WindowUtils
    {
        /// <summary>
        /// Returns a handle to the foreground window.
        /// </summary>
        /// <returns>hWnd of the foreground window.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

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
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
