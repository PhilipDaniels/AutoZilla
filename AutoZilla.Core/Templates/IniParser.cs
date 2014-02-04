using AutoZilla.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoZilla.Core.Templates
{
    public sealed class IniSection
    {
        public IDictionary<string, string> Values;

        public IniSection(string name)
        {
            name.ThrowIfNull("name");
            Values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Dead simple class to read INI files.
    /// Based on https://gist.github.com/grumly57/5725301
    /// Better than NINI because it allows double quotes in values.
    /// </summary>
    public sealed class IniParser
    {
        public IDictionary<string, IniSection> Sections { get; private set; }

        Dictionary<string, Dictionary<string, string>> ini = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initialize an INI file from a string.
        /// </summary>
        /// <param name="iniData">The INI data content (such as the text read from a file).</param>
        public IniParser(string iniData)
            : this(iniData, false)
        {
        }

        /// <summary>
        /// Initialize an INI file from a string.
        /// </summary>
        /// <param name="iniData">The INI data content (such as the text read from a file).</param>
        /// <param name="coalesceLineContinuations">If true, the parser will consider a slash '\' at the
        /// end of a line to be a line continuation, and remove it before parsing, forming the
        /// value into one long string.</param>
        public IniParser(string iniData, bool coalesceLineContinuations)
        {
            iniData.ThrowIfNullOrWhiteSpace("iniData", "You must specify some INI data.");

            Sections = new Dictionary<string, IniSection>();

            if (coalesceLineContinuations)
            {
                iniData = iniData.Replace("\\" + Environment.NewLine, "");
            }

            Parse(iniData);
        }

        void Parse(string iniData)
        {
            var currentSection = MakeSectionDictionary();
            ini[""] = currentSection;

            foreach (var l in iniData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                              .Select((t, i) => new
                              {
                                  idx = i,
                                  text = t.Trim()
                              }))
            // .Where(t => !string.IsNullOrWhiteSpace(t) && !t.StartsWith(";")))
            {
                var line = l.text;

                if (line.StartsWith(";") || string.IsNullOrWhiteSpace(line))
                {
                    //currentSection.Add(";" + l.idx.ToString(), line);
                    continue;
                }

                if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                {
                    string sectionName = line.Substring(1, line.Length - 2);
                    currentSection = MakeSectionDictionary();
                    ini[sectionName] = currentSection;
                    continue;
                }

                var idx = line.IndexOf("=", StringComparison.OrdinalIgnoreCase);
                if (idx == -1)
                {
                    // Attempt an alternative.
                    idx = line.IndexOf(":", StringComparison.OrdinalIgnoreCase);
                }

                if (idx == -1)
                {
                    currentSection[line] = "";
                }
                else
                {
                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    currentSection[key] = value;
                }
            }
        }

        /// <summary>
        /// Get a parameter value at the root level.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string key)
        {
            key.ThrowIfNull("key");

            return GetValue("", key, "");
        }

        /// <summary>
        /// Get a parameter value in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">Parameter key.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key)
        {
            section.ThrowIfNull("section");
            key.ThrowIfNull("key");

            return GetValue(section, key, "");
        }

        /// <summary>
        /// Get a parameter value in a section with a default value if not found.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <param name="key">Parameter key.</param>
        /// /// <param name="defaultIfNotFound">Default to be returned if no value found.</param>
        /// <returns>Value of the <paramref name="key"/>.</returns>
        public string GetValue(string section, string key, string defaultIfNotFound)
        {
            if (!ini.ContainsKey(section))
                return defaultIfNotFound;

            if (!ini[section].ContainsKey(key))
                return defaultIfNotFound;

            return ini[section][key];
        }

        /// <summary>
        /// Get all the section names.
        /// </summary>
        /// <returns>Sequence of section names.</returns>
        public IEnumerable<string> GetSections()
        {
            return ini.Keys.Where(t => t != "");
        }

        /// <summary>
        /// Get all the keys names in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <returns>Sequence of key names.</returns>
        public IEnumerable<string> GetKeys(string section)
        {
            section.ThrowIfNull("section");

            if (!ini.ContainsKey(section))
                return new string[0];

            return ini[section].Keys;
        }



        static Dictionary<string, string> MakeSectionDictionary()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}
