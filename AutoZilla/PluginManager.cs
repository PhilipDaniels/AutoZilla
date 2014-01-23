using AutoZilla.Core;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla
{
    /// <summary>
    /// Handles the loading and unloading of plugins from the plugin directory.
    /// </summary>
    class PluginManager
    {
        public GlobalHotkeyManager HotKeyManager { get; private set; }
        public IEnumerable<IAutoZillaPlugin> Plugins { get; private set; }

        public PluginManager()
        {
            HotKeyManager = new GlobalHotkeyManager();
        }

        public void LoadAllPlugins()
        {
            Plugins = LoadPlugins();
            InitialisePlugins();
        }

        void InitialisePlugins()
        {
            foreach (var plugin in Plugins)
            {
                plugin.InitialiseHotKeyManager(HotKeyManager);
            }
        }

        IEnumerable<IAutoZillaPlugin> LoadPlugins()
        {
            string[] pluginFiles = Directory.GetFiles(AutoZillaVariables.PluginsFolder, "*.dll", SearchOption.AllDirectories);

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
