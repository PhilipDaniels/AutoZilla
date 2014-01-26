using AutoZilla.Core;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZilla
{
    sealed class AutoTemplateManager
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string AutoTemplateFolder { get; set; }
        GlobalHotkeyManager HotKeyManager { get; set; }
        FileSystemWatcher Watcher { get; set; }
        List<Template> ActiveTemplates { get; set; }

        internal AutoTemplateManager()
        {
            HotKeyManager = new GlobalHotkeyManager();
            ActiveTemplates = new List<Template>();
            AutoTemplateFolder = Path.Combine(AutoZillaVariables.ExeFolder, "AutoTemplates");
        }

        internal void BeginWatching()
        {
            if (!Directory.Exists(AutoTemplateFolder))
            {
                log.ErrorFormat("The AutoTemplates folder, expected at {0}, does not exist. No AutoTemplates have been loaded.", AutoTemplateFolder);
                return;
            }

            log.DebugFormat("Watching for change events in folder {0}", AutoTemplateFolder);

            Watcher = new FileSystemWatcher(AutoTemplateFolder, "*." + TemplateLoader.TemplateExtension);
            Watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            Watcher.Changed += AutoTemplatesWatcher_SomethingChanged;
            Watcher.Deleted += AutoTemplatesWatcher_SomethingChanged;
            Watcher.Created += AutoTemplatesWatcher_SomethingChanged;
            Watcher.Renamed += AutoTemplatesWatcher_SomethingChanged;
            Watcher.EnableRaisingEvents = true;

            // Initial load.
            LoadTemplates();
        }

        /// <summary>
        /// Whatever happens, unload all existing templates and reregister. It's the only
        /// way to be sure we are mimicing what is in the folder, for example if somebody
        /// changes both a filename a key what would we do? Information has been destroyed,
        /// we cannot work out the minimal delta.
        /// </summary>
        /// <param name="sender">The FileSystemWatcher.</param>
        /// <param name="e"></param>
        void AutoTemplatesWatcher_SomethingChanged(object sender, FileSystemEventArgs e)
        {
            log.DebugFormat("Got AutoTemplatesWatcher event of type {0}", e.ChangeType);

            try
            {
                UnloadCurrentTemplates();
                LoadTemplates();
            }
            catch (Exception ex)
            {
                log.Error("Error while re-syncing AutoTemplates folder.", ex);
            }
        }

        void UnloadCurrentTemplates()
        {
            foreach (var template in ActiveTemplates)
            {
                HotKeyManager.Unregister(template);
            }

            ActiveTemplates = new List<Template>();
        }

        void LoadTemplates()
        {
            IEnumerable<string> templateFiles = TemplateLoader.GetTemplatesInFolder(AutoTemplateFolder);

            foreach (var file in templateFiles)
            {
                try
                {
                    var template = TemplateLoader.LoadTemplate(file);
                    if (template.Key != null && !String.IsNullOrEmpty(template.OriginalText))
                    {
                        HotKeyManager.Register(template);
                        ActiveTemplates.Add(template);
                    }
                    else
                    {
                        log.DebugFormat("The auto-template {0} does not have a key and/or a body - ignoring.", file);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while attempting to load template file " + file, ex);
                }
            }
        }
    }
}
