using AutoZilla.Core.Extensions;
using AutoZilla.Core.GlobalHotKeys;
using Nini.Config;
using Nini.Ini;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// Provides methods to load templates from files or string. This class
    /// expects the templates to have a [Config] section. If you just want
    /// to load a raw template, use the constructors on the <code>Template</code>
    /// class itself.
    /// </summary>
    public static class TemplateLoader
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string TemplateExtension = "azt";
        const string AutoZillaSeparator = ";;AZ;;";

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
            Uri uri = new Uri(codeBase);
            UriBuilder uriBuilder = new UriBuilder(uri);
            string path = Uri.UnescapeDataString(uriBuilder.Path);
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "Templates");
            return path;
        }

        /// <summary>
        /// Finds all the templates in a specific folder, that is, all those files
        /// which end with the AutoZilla template extension.
        /// </summary>
        /// <param name="templateFolder">The folder to scan.</param>
        /// <returns>List of matching files.</returns>
        public static IEnumerable<string> GetTemplatesInFolder(string templateFolder)
        {
            templateFolder.ThrowIfDirectoryDoesNotExist("templateFolder");
            return Directory.GetFiles(templateFolder, "*." + TemplateLoader.TemplateExtension);
        }

        /// <summary>
        /// Loads all the templates in <paramref name="templateFolder"/>.
        /// </summary>
        /// <param name="templateFolder">The folder to load templates from.</param>
        public static IEnumerable<TextTemplate> LoadTemplates(string templateFolder)
        {
            templateFolder.ThrowIfDirectoryDoesNotExist("templateFolder");

            IEnumerable<string> templateFiles = GetTemplatesInFolder(templateFolder);

            // ToList() to force eval at this point. If there are errors
            // the earlier we see them the better.
            return (from f in templateFiles
                    let t = LoadTemplate(f)
                    select t).ToList();
        }

        /// <summary>
        /// Load an individual template.
        /// </summary>
        /// <param name="templateFilePath">Full path to the template.</param>
        /// <returns>Template object.</returns>
        public static TextTemplate LoadTemplate(string templateFilePath)
        {
            templateFilePath.ThrowIfFileDoesNotExist("templateFilePath");

            string contents = File.ReadAllText(templateFilePath);
            return InternalLoadTemplateFromString(templateFilePath, contents);
        }

        /// <summary>
        /// Loads a template from a string.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns>Template object.</returns>
        public static TextTemplate LoadTemplateFromString(string template)
        {
            template.ThrowIfNullOrWhiteSpace("template");

            return InternalLoadTemplateFromString(null, template);
        }

        static TextTemplate InternalLoadTemplateFromString(string templateFilePath, string template)
        {
            template.ThrowIfNullOrWhiteSpace("template");

            try
            {
                string configAsString, templateBody;
                template.BeforeAndAfter(AutoZillaSeparator, StringComparison.OrdinalIgnoreCase, out configAsString, out templateBody);

                if (String.IsNullOrWhiteSpace(configAsString))
                {
                    string msg = String.Format
                        (
                        CultureInfo.InvariantCulture,
                        "The template{0}{1} has an invalid format. No config section was found. " +
                        "Check for the AutoZilla separator '{2}'.",
                        templateFilePath == null ? "" : " ",
                        templateFilePath ?? "",
                        AutoZillaSeparator
                        );
                    throw new TemplateFormatException(msg, template);
                }

                if (String.IsNullOrWhiteSpace(templateBody))
                {
                    string msg = String.Format
                        (
                        CultureInfo.InvariantCulture,
                        "The template{0}{1} has an invalid format. No template body was found. " +
                        "Check for the AutoZilla separator '{2}'.",
                        templateFilePath == null ? "" : " ",
                        templateFilePath ?? "",
                        AutoZillaSeparator
                        );
                    throw new TemplateFormatException(msg, template);
                }


                var config = ParseTemplateConfig(templateFilePath, configAsString);
                templateBody = templateBody.TrimOneLeadingNewLine();

                var t = new TextTemplate(config.Key, config.Name, config.Description, templateFilePath, templateBody);
                return t;
            }
            catch (Exception ex)
            {
                log.Error("Error while loading template.", ex);
                throw new TemplateFormatException("Error while loading template.", template, ex);
            }
        }

        static TemplateConfig ParseTemplateConfig(string templateFilePath, string azConfig)
        {
            using (var rdr = new StringReader(azConfig))
            {
                var doc = new IniDocument(rdr, IniFileType.PythonStyle);
                var source = new IniConfigSource(doc);
                var config = source.Configs["Config"];
                source.CaseSensitive = false;

                string HotKey = config.Get("Key");

                ModifiedKey modifiedKey = null;
                if (!String.IsNullOrWhiteSpace(HotKey))
                {
                    if (!ModifiedKey.TryParse(HotKey, out modifiedKey))
                    {
                        var msg = String.Format(CultureInfo.InvariantCulture, "The modified key specification {0} in template {1} is invalid.", HotKey, templateFilePath);
                        throw new TemplateFormatException(msg);
                    }
                }

                var result = new TemplateConfig();
                result.Key = modifiedKey;
                result.Name = config.Get("Name");
                result.Description = config.Get("Description");

                return result;
            }
        }

        /// <summary>
        /// Template configuration information as read from the template file.
        /// </summary>
        class TemplateConfig
        {
            /// <summary>
            /// The key that the template is bound to, that is, pressing this
            /// key should invoke the template. It can be null.
            /// </summary>
            public ModifiedKey Key { get; set; }

            /// <summary>
            /// Short name of the template.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Description of the template. Intended to be human-readable
            /// text suitable for presenting in the AutoZilla GUI.
            /// </summary>
            public string Description { get; set; }
        }
    }
}
