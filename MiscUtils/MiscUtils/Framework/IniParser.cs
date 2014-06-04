// Copyright 2013, 2014 Philip Daniels - http://www.philipdaniels.com/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscUtils.Framework
{
    /// <summary>
    /// Dead simple class to read INI files.
    /// Based on https://gist.github.com/grumly57/5725301
    /// Better than NINI because it allows double quotes in values and I've
    /// hacked it to support multiple line values.
    /// There is an IniSection class which is defined at the bottom of
    /// this file. The entire file is self contained.
    /// </summary>
    public sealed class IniParser
    {
        IDictionary<string, IniSection> iniSections { get; set; }

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

            iniSections = new Dictionary<string, IniSection>(StringComparer.OrdinalIgnoreCase);

            if (coalesceLineContinuations)
            {
                iniData = iniData.Replace("\\" + Environment.NewLine, "");
            }

            Parse(iniData);
        }

        void Parse(string iniData)
        {
            var currentSection = new IniSection("");
            iniSections.Add(currentSection.Name, currentSection);

            foreach (var l in iniData.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                              .Select((t, i) => new
                              {
                                  idx = i,
                                  text = t.Trim()
                              }))
            // .Where(t => !string.IsNullOrWhiteSpace(t) && !t.StartsWith(";")))
            {
                var line = l.text;

                if (line.StartsWith(";", StringComparison.OrdinalIgnoreCase) || String.IsNullOrWhiteSpace(line))
                {
                    //currentSection.Add(";" + l.idx.ToString(), line);
                    continue;
                }

                if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]", StringComparison.OrdinalIgnoreCase))
                {
                    string sectionName = line.Substring(1, line.Length - 2);
                    currentSection = new IniSection(sectionName);
                    iniSections.Add(currentSection.Name, currentSection);
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
            if (!iniSections.ContainsKey(section))
                return defaultIfNotFound;

            if (!iniSections[section].ContainsKey(key))
                return defaultIfNotFound;

            return iniSections[section][key];
        }

        /// <summary>
        /// Get all the section names.
        /// </summary>
        /// <returns>Sequence of section names.</returns>
        public IEnumerable<IniSection> Sections
        {
            get
            {
                return from s in iniSections
                       where !String.IsNullOrEmpty(s.Key)
                       select s.Value;
            }
        }

        /// <summary>
        /// Get all the keys names in a section.
        /// </summary>
        /// <param name="section">Name of the section.</param>
        /// <returns>Sequence of key names.</returns>
        public IEnumerable<string> GetKeys(string section)
        {
            section.ThrowIfNull("section");

            if (!iniSections.ContainsKey(section))
                return new string[0];

            return iniSections[section].Keys;
        }
    }






    /// <summary>
    /// Represents one section of the INI file.
    /// </summary>
    public sealed class IniSection : IDictionary<string, string>
    {
        public string Name { get; private set; }
        IDictionary<string, string> InnerDict;

        public IniSection(string name)
        {
            name.ThrowIfNull("name");
            Name = name;
            InnerDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public void Add(string key, string value)
        {
            InnerDict.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return InnerDict.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return InnerDict.Keys; }
        }

        public bool Remove(string key)
        {
            return InnerDict.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return InnerDict.TryGetValue(key, out value);
        }

        public ICollection<string> Values
        {
            get { return InnerDict.Values; }
        }

        public string this[string key]
        {
            get
            {
                return InnerDict[key];
            }
            set
            {
                InnerDict[key] = value;
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            InnerDict.Add(item);
        }

        public void Clear()
        {
            InnerDict.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return InnerDict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            InnerDict.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerDict.Count; }
        }

        public bool IsReadOnly
        {
            get { return InnerDict.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return InnerDict.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return InnerDict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return InnerDict.GetEnumerator();
        }
    }
}
