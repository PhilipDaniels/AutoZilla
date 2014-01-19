using AutoZilla.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoZilla
{
    static class Program
    {
        static IEnumerable<IAutoZillaPlugin> Plugins;
        static GlobalHotkeyManager HotKeyManager;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Plugins = LoadPlugins();
            HotKeyManager = new GlobalHotkeyManager();
            InitialisePlugins(Plugins, HotKeyManager);
            Application.Run(new ApplicationContextWithNotifyIcon());
        }

        static void InitialisePlugins(IEnumerable<IAutoZillaPlugin> plugins, GlobalHotkeyManager hotKeyManager)
        {
            foreach (var plugin in plugins)
            {
                plugin.InitialiseHotKeyManager(hotKeyManager);
            }
        }

        static IEnumerable<IAutoZillaPlugin> LoadPlugins()
        {
            string path = Application.StartupPath;
            path = Path.Combine(path, "Plugins");
            string[] pluginFiles = Directory.GetFiles(path, "*.dll");

            var plugins =
                (
                from file in pluginFiles
                let asm = Assembly.LoadFile(file)
                from type in asm.GetExportedTypes()
                where typeof(IAutoZillaPlugin).IsAssignableFrom(type)
                select (IAutoZillaPlugin)Activator.CreateInstance(type)
                );

            return plugins;
        }
    }
}
