using AutoZilla.Core;
using AutoZilla.Core.GlobalHotKeys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Demo1
{
    static class Program
    {
        static TextOutputter TO = new TextOutputter();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This is pretty much the simplest possible example of how to install a HotKey and do something.
            // It is not an AutoZilla plugin, it is just a self contained program.
            // Form1 has had no modifications, so it is visible which is not what you want in most cases.

            // n.b. You should create a Windows Forms application, not a console application, because the
            // hot key hooking will not work with console apps (they don't have a message loop).

            // n.b. This simple demo will throw an exception if some other application has registered CA-L as
            // a HotKey.
            using (GlobalHotKeyManager HotKeyManager = new GlobalHotKeyManager())
            {
                HotKeyManager.Register(Modifiers.Ctrl | Modifiers.Alt, Keys.L, MyCallback);

                Application.Run(new Form1());

                // Try this if you want a no-GUI program.
                //Application.Run(new InvisibleForm());
            }
        }

        static void MyCallback(ModifiedKey HotKeyInfo)
        {
            // Slow
            //TO.WaitForModifiersUp();
            //TO.PasteLine("Hello world1");
            //TO.PasteLine("Hello world2");
            //TO.PasteLine("Hello world3");
            //TO.Move(2, 0);
            //TO.SelectNextWord();

            // Quick because of one paste.
            // Alsow shows fluent chaining syntax.
            TO.WaitForModifiersUp().
                PasteLine("Hello world1\r\nHello world2\r\nHello world3").
                Move(-2, 0).
                SelectNextWord();
        }
    }
}
