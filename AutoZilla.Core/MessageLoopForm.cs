using AutoZilla.Core.GlobalHotKeys;
using System;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// Registering a global hot key requires a form to receive the WndProc messages.
    /// That is the purpose of this form. It is invisible and used only within the
    /// AutoZilla core.
    /// </summary>
    internal partial class MessageLoopForm : InvisibleForm
    {
        internal event EventHandler<HotKeyPressedEventArgs> HotKeyPressed;

        internal MessageLoopForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            var HotKeyInfo = ModifiedKey.GetFromMessage(m);
            if (HotKeyInfo != null)
            {
                HandleHotKey(HotKeyInfo);
            }
            base.WndProc(ref m);
        }

        void HandleHotKey(ModifiedKey HotKeyInfo)
        {
            var handler = HotKeyPressed;
            if (handler != null)
            {
                var args = new HotKeyPressedEventArgs(HotKeyInfo);
                handler(this, args);
            }
        }
    }
}
