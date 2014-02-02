using AutoZilla.Core;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoZilla
{
    /// <summary>
    /// Handles the loading and unloading of plugins from the plugin directory.
    /// </summary>
    sealed class PluginManager
    {
        GlobalHotKeyManager HotKeyManager;
        IEnumerable<IAutoZillaPlugin> Plugins;

        public PluginManager()
        {
            HotKeyManager = new GlobalHotKeyManager();
        }

        /// <summary>
        /// Loads all the plugins from the plugins folder.
        /// </summary>
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

        static IEnumerable<IAutoZillaPlugin> LoadPlugins()
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
