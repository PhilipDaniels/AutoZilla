using AutoZilla.Core.GlobalHotKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Core.WinForms
{
    /// <summary>
    /// When it is displayed this form memorizes which window was previously
    /// the foreground window and provides methods for restoring it
    /// and sending data to it. The memorization of the previous foreground
    /// window is done in the constructor, so unless you immediately show
    /// the window after constructing it it is likely to be unreliable.
    /// </summary>
    public class FocusRestoringForm : Form
    {
        IntPtr PreviousForegroundWindow;
        TextOutputter TOUT = new TextOutputter();

        public FocusRestoringForm()
            : base()
        {
            PreviousForegroundWindow = NativeMethods.GetForegroundWindow();
        }

        public void SendToLastWindow(string text)
        {
            NativeMethods.SetForegroundWindow(PreviousForegroundWindow);
            TOUT.PasteString(text);
        }
    }
}
