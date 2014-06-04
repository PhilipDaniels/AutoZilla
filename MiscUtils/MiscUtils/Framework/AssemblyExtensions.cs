// Copyright 2013 Philip Daniels - http://www.philipdaniels.com/
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
using System.Drawing;
using System.IO;
using System.Reflection;

namespace MiscUtils.Framework
{
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the full manifest filename of a manifest resource. The <paramref name="fileName" />
        /// is of the format "folder.folder.filename.ext" and is case sensitive.
        /// </summary>
        /// <param name="assembly">The assembly in which to form the filename.</param>
        /// <param name="fileName">Filename you want to refer to.</param>
        /// <returns>Fully qualified manifest resource filename.</returns>
        public static string GetResourceFileName(this Assembly assembly, string fileName)
        {
            assembly.ThrowIfNull("assembly");
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            string name = String.Concat(assembly.GetName().Name, ".", fileName);
            return name;
        }

        /// <summary>
        /// Gets an open stream on the specified embedded resource. It is the
        /// caller's responsibility to call Dispose() on the stream.
        /// The filename is of the format "folder.folder.filename.ext"
        /// and is case sensitive.
        /// </summary>
        /// <param name="assembly">The assembly from which to retrieve the Stream.</param>
        /// <param name="fileName">Filename whose contents you want.</param>
        /// <returns>Stream object.</returns>
        public static Stream GetResourceStream(this Assembly assembly, string fileName)
        {
            assembly.ThrowIfNull("assembly");
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            string name = assembly.GetResourceFileName(fileName);
            Stream s = assembly.GetManifestResourceStream(name);
            return s;
        }

        /// <summary>
        /// Get the contents of an embedded file as a string.
        /// The filename is of the format "folder.folder.filename.ext"
        /// and is case sensitive.
        /// </summary>
        /// <param name="assembly">The assembly from which to retrieve the file.</param>
        /// <param name="filename">Filename whose contents you want.</param>
        /// <returns>String object.</returns>
        public static string GetResourceAsString(this Assembly assembly, string fileName)
        {
            assembly.ThrowIfNull("assembly");
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            using (Stream s = GetResourceStream(assembly, fileName))
            using (StreamReader sr = new StreamReader(s))
            {
                string fileContents = sr.ReadToEnd();
                return fileContents;
            }
        }

        /// <summary>
        /// Get the contents of an embedded file as an Image.
        /// The filename is of the format "folder.folder.filename.ext"
        /// and is case sensitive.
        /// </summary>
        /// <param name="assembly">The assembly from which to retrieve the image.</param>
        /// <param name="filename">Filename whose contents you want.</param>
        /// <returns>Image object.</returns>
        public static Image GetResourceAsImage(this Assembly assembly, string fileName)
        {
            assembly.ThrowIfNull("assembly");
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            using (Stream s = GetResourceStream(assembly, fileName))
            {
                Image i = Image.FromStream(s);
                return i;
            }
        }

        /// <summary>
        /// Get the contents of an embedded file as an array of bytes.
        /// The filename is of the format "folder.folder.filename.ext"
        /// and is case sensitive.
        /// </summary>
        /// <param name="assembly">The assembly from which to retrieve the image.</param>
        /// <param name="filename">Filename whose contents you want.</param>
        /// <returns>The manifest resource as an array of bytes.</returns>
        public static byte[] GetResourceAsBytes(this Assembly assembly, string fileName)
        {
            assembly.ThrowIfNull("assembly");
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            using (Stream s = GetResourceStream(assembly, fileName))
            {
                byte[] data = s.ReadFully();
                return data;
            }
        }
        
        /*
        public static bool ReferencesAssemblies(this Assembly assembly, params string[] whitelist)
        {
            return assembly.ReferencesAssemblies((IEnumerable<string>)whitelist);
        }
        */

        public static bool DoesNotReferenceAssemblies(this Assembly assembly, params string[] blacklist)
        {
            return assembly.DoesNotReferenceAssemblies((IEnumerable<string>)blacklist);
        }

        public static bool DoesNotReferenceAssemblies(this Assembly assembly, IEnumerable<string> blacklist)
        {
            assembly.ThrowIfNull("assembly");
            blacklist.ThrowIfNull("blacklist");

            var references = assembly.GetReferencedAssemblies();

            foreach (var asmName in references)
            {
                foreach (var partialName in blacklist)
                {
                    if (asmName.Name.IndexOf(partialName, StringComparison.OrdinalIgnoreCase) != -1)
                        return false;
                }
            }

            return true;
        }

        /*
        public static bool ReferencesAssemblies(this Assembly assembly, IEnumerable<string> whitelist)
        {
            assembly.ThrowIfNull("assembly");
            whitelist.ThrowIfNull("whitelist");

            var references = assembly.GetReferencedAssemblies();

            foreach (var partialName in whitelist)
            {
                
            }


            foreach (var asmName in references)
            {
                foreach (var partialName in whitelist)
                {
                    if (asmName.Name.IndexOf(partialName, StringComparison.OrdinalIgnoreCase) != -1)
                        return false;
                }
            }

            return true;
        }
        */
    }
}
