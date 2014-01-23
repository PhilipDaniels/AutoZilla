using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoZilla.Core.GlobalHotkeys;
using AutoZilla.Core.Validation;
using System.Reflection;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// Provides methods to load templates from files.
    /// The convention is that the templates for your plugin will be
    /// in the folder <code>YourPluginNameTemplates</code>. This class
    /// is designed to load templates that have assigned hot-keys and
    /// descriptions.
    /// </summary>
    public static class TemplateLoader
    {
        static readonly string AUTOZILLA_PREFIX = ";;AZ;;";
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the full path of the default template folder for an assembly.
        /// This is defined as a folder named "Templates" in the same folder
        /// as the dll.
        /// </summary>
        /// <param name="assembly">The assembly you want the template folder for. Typically
        /// this will be a plugin.</param>
        /// <returns>Full path of the Template folder.</returns>
        public static string GetDefaultTemplateFolder(Assembly assembly)
        {
            assembly.ThrowIfNull("assembly");
            
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "Templates");
            return path;
        }

        /// <summary>
        /// Initialises a new <code>TemplateLoader</code> and loads all
        /// the templates in <paramref name="templateFolder"/>.
        /// </summary>
        /// <param name="templateFolder">The folder to load templates from.</param>
        public static IEnumerable<Template> LoadTemplates(string templateFolder)
        {
            templateFolder.ThrowIfNullOrWhiteSpace("templateFolder");
            if (!Directory.Exists(templateFolder))
                throw new ArgumentOutOfRangeException("templateFolder", "templateFolder does not exist.");

            string[] templateFiles = Directory.GetFiles(templateFolder, "*.txt");
            var templates = new List<Template>();
            foreach (var f in templateFiles)
            {
                templates.Add(LoadTemplate(f));
            }

            return templates;
        }

        public static Template LoadTemplate(string templateFilePath)
        {
            try
            {
                string contents = File.ReadAllText(templateFilePath).TrimStart();
                if (!contents.StartsWith(AUTOZILLA_PREFIX, StringComparison.InvariantCultureIgnoreCase))
                {
                    log.DebugFormat
                        (
                        "The template at {0} does not contain an AutoZilla prefix of {1} on its first line - ignoring.",
                        templateFilePath, AUTOZILLA_PREFIX
                        );
                    return null;
                }

                string[] parts = contents.Split(new string[] { Environment.NewLine }, 2, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    string msg = String.Format("The template file {0} has an invalid format. No body was found.", templateFilePath);
                    throw new Exception(msg);
                }

                string templateBody = parts[1];
                string metaDataLine = parts[0].Remove(0, AUTOZILLA_PREFIX.Length).Trim();

                // If a semi-colon appears we expect "CSAW-Key; Description."
                // if one does not appear then assume we just have a description.
                string hotkey;
                string description;

                int indexOfSemi = metaDataLine.IndexOf(';');
                if (indexOfSemi == -1)
                {
                    hotkey = null;
                    description = metaDataLine.Trim();
                }
                else if (metaDataLine[indexOfSemi - 1] == '-' && metaDataLine[indexOfSemi + 1] == ';')
                {
                    // User wants to setup a hotkey for ';' i.e. we have CSA-;;
                    hotkey = metaDataLine.Substring(0, indexOfSemi +1);
                    description = metaDataLine.Substring(indexOfSemi + 2).Trim();
                }
                else
                {
                    // User wants to setup a hotkey for something else. CSA-K;
                    hotkey = metaDataLine.Substring(0, indexOfSemi);
                    description = metaDataLine.Substring(indexOfSemi + 1).Trim();
                }

                if (String.IsNullOrWhiteSpace(description))
                    description = Path.GetFileName(templateFilePath);

                ModifiedKey modifiedKey = null;
                if (!String.IsNullOrWhiteSpace(hotkey))
                {
                    if (!ModifiedKey.TryParse(hotkey, out modifiedKey))
                    {
                        var msg = String.Format("The modified key specification {0} in template {1} is invalid.", hotkey, templateFilePath);
                        throw new Exception(msg);
                    }
                }

                var t = new Template(modifiedKey, description, templateFilePath, templateBody);
                return t;
            }
            catch (Exception ex)
            {
                log.Error("Error while loading template file " + templateFilePath + " - ignoring.", ex);
                return null;
            }
        }
    }
}
