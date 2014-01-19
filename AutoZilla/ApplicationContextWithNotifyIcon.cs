using AutoZilla.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla
{
    class ApplicationContextWithNotifyIcon : ApplicationContext
    {
        NotifyIcon TrayIcon;
        MainForm MainForm = new MainForm();

        public ApplicationContextWithNotifyIcon()
        {
            TrayIcon = new NotifyIcon()
            {
                Icon = Resources.AutoZillaTrayIcon,
                ContextMenu = GetContextMenu(),
                Visible = true
            };

            TrayIcon.DoubleClick += TrayIcon_DoubleClick;
        }


        ContextMenu GetContextMenu()
        {
            var cm = new ContextMenu();
            cm.MenuItems.Add(new MenuItem("&AutoZilla", MenuShowMainForm));
            cm.MenuItems.Add(new MenuItem("-"));
            cm.MenuItems.Add(new MenuItem("E&xit", MenuExit));
            
            return cm;
        }

        void MenuShowMainForm(object sender, EventArgs e)
        {
            if (MainForm.Visible)
            {
                MainForm.Activate();
            }
            else
            {
                MainForm.ShowDialog();
            }
        }

        void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            MenuShowMainForm(sender, e);
        }

        void MenuExit(object sender, EventArgs e)
        {
            if (TrayIcon != null)
            {
                TrayIcon.Visible = false;
                TrayIcon.Dispose();
            }

            Application.Exit();
        }
    }
}
