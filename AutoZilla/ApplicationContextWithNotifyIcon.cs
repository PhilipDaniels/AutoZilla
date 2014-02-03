using AutoZilla.Properties;
using System;
using System.Windows.Forms;

namespace AutoZilla
{
    /// <summary>
    /// This is a custom <code>ApplicationContext</code>, used in the Run method.
    /// It avoids us having to create and then hide a form in order to get a
    /// message loop started.
    /// </summary>
    sealed class ApplicationContextWithNotifyIcon : ApplicationContext
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        NotifyIcon TrayIcon;
        MainForm TheMainForm;
        AutoTemplateManager AutoTemplateManager;
        PluginManager PluginManager;

        public ApplicationContextWithNotifyIcon()
        {
            TrayIcon = new NotifyIcon()
            {
                Icon = Resources.AutoZillaTrayIcon,
                ContextMenu = GetContextMenu(),
                Visible = true,
                Text = Resources.AppName
            };

            TrayIcon.DoubleClick += TrayIcon_DoubleClick;

            TheMainForm = new MainForm();
            AutoTemplateManager = new AutoTemplateManager();
            AutoTemplateManager.BeginWatching();
            PluginManager = new PluginManager();
            PluginManager.LoadAllPlugins();
        }

        ContextMenu GetContextMenu()
        {
            var cm = new ContextMenu();
            cm.MenuItems.Add(new MenuItem("&" + Resources.AppName, MenuShowMainForm));
            cm.MenuItems.Add(new MenuItem("-"));
            cm.MenuItems.Add(new MenuItem("E&xit", MenuExit));
            
            return cm;
        }

        void MenuShowMainForm(object sender, EventArgs e)
        {
            if (TheMainForm.Visible)
            {
                TheMainForm.Activate();
            }
            else
            {
                TheMainForm.ShowDialog();
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
