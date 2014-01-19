using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// Registering a global hot key requires a form to receive the WndProc messages.
    /// That is the purpose of this form. It is invisible.
    /// </summary>
    internal partial class MessageLoopForm : InvisibleForm
    {
        internal event EventHandler<HotkeyPressedEventArgs> HotkeyPressed;

        internal MessageLoopForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            var hotkeyInfo = ModifiedKey.GetFromMessage(m);
            if (hotkeyInfo != null)
            {
                HandleHotKey(hotkeyInfo);
            }
            base.WndProc(ref m);
        }

        void HandleHotKey(ModifiedKey hotkeyInfo)
        {
            var handler = HotkeyPressed;
            if (handler != null)
            {
                var args = new HotkeyPressedEventArgs(hotkeyInfo);
                handler(this, args);
            }
        }
    }
}
