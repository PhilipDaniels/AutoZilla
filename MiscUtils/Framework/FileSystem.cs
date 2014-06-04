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

using System.IO;

namespace MiscUtils.Framework
{
    /// <summary>
    /// Convenience methods for dealing with files and directories.
    /// n.b. For things like LINQ lazy enumerating and filtering the file system,
    /// see Directory.Enumerate* and DirectoryInfo.Enumerate* at
    /// http://msdn.microsoft.com/en-us/library/dd997370(v=vs.110).aspx
    /// </summary>
    public static class FileSystem
    {
        /// <summary>
        /// Ensure the <paramref name="directory"/> exists. This is a no-op
        /// if the directory already exists. If you pass the name of an existing
        /// FILE you will get an exception.
        /// </summary>
        /// <param name="directory">The directory to create.</param>
        public static void EnsureDirectory(string directory)
        {
            directory.ThrowIfNullOrWhiteSpace("directory");

            Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// Ensure the parent directory of the <paramref name="fileName"/> exists.
        /// This is a no-op if the directory already exists. You can pass a directory
        /// if you want.
        /// </summary>
        /// <param name="fileName">Full path of the file whose directory you want to ensure.</param>
        public static void EnsureParentDirectory(string fileName)
        {
            fileName.ThrowIfNullOrWhiteSpace("fileName");

            string dir = Path.GetDirectoryName(fileName);
            EnsureDirectory(dir);
        }

        /// <summary>
        /// Deletes a directory.
        /// No exception is thrown if the directory does not exist.
        /// </summary>
        /// <param name="directory">The directory to delete.</param>
        /// <param name="recursive">Whether to recursively delete directory contents.</param>
        /// <param name="squashAllExceptions">If true, any exceptions will be squashed. This makes it
        /// a "safe" variant.</param>
        public static void DeleteDirectory
            (
            string directory,
            SearchOption options = SearchOption.AllDirectories,
            bool squashAllExceptions = true
            )
        {
            directory.ThrowIfNullOrWhiteSpace("directory");

            try
            {
                if (Directory.Exists(directory))
                {
                    SetReadOnlyAttribute(directory, false, options);
                    Directory.Delete(directory, true);
                }
            }
            catch
            {
                if (!squashAllExceptions)
                    throw;
            }
        }

        /// <summary>
        /// Delete the contents of a directory, while leaving the directory itself.
        /// No exception is thrown if the directory does not exist.
        /// </summary>
        /// <param name="directory">The directory to have its contents deleted.</param>
        /// <param name="recursive">Whether to recursively delete directory contents.</param>
        /// <param name="squashAllExceptions">If true, any exceptions will be squashed. This makes it
        public static void DeleteDirectoryContents
            (
            string directory,
            SearchOption options = SearchOption.AllDirectories,
            bool squashAllExceptions = true
            )
        {
            directory.ThrowIfNullOrWhiteSpace("directory");

            try
            {
                if (Directory.Exists(directory))
                {
                    var di = new DirectoryInfo(directory);
                    foreach (var fi in di.EnumerateFiles())
                    {
                        if (fi.IsReadOnly)
                        {
                            fi.IsReadOnly = false;
                        }
                        fi.Delete();
                    }

                    foreach (string dir in Directory.GetDirectories(directory))
                    {
                        DeleteDirectory(dir, options, squashAllExceptions);
                    }
                }
            }
            catch
            {
                if (!squashAllExceptions)
                    throw;
            }
        }

        /// <summary>
        /// Copies a directory from one place to another.
        /// </summary>
        /// <param name="source">The directory to copy.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="overwrite">Whether to overwrite the destination.</param>
        public static void CopyDirectory(string source, string destination, bool overwrite = true)
        {
            source.ThrowIfDirectoryDoesNotExist("source");
            destination.ThrowIfNullOrWhiteSpace("destination");

            var c = new Microsoft.VisualBasic.Devices.Computer();
            c.FileSystem.CopyDirectory(source, destination, overwrite);
        }

        /// <summary>
        /// Moves a directory from one place to another.
        /// </summary>
        /// <param name="source">The directory to move.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="overwrite">Whether to overwrite the destination.</param>
        public static void MoveDirectory(string source, string destination, bool overwrite = true)
        {
            source.ThrowIfDirectoryDoesNotExist("source");
            destination.ThrowIfNullOrWhiteSpace("destination");

            var c = new Microsoft.VisualBasic.Devices.Computer();
            c.FileSystem.MoveDirectory(source, destination, overwrite);
        }

        /// <summary>
        /// Sets or resets the readonly flag on the files in a directory and optionally
        /// its subdirectories.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="readOnly">True or false.</param>
        /// <param name="options">Whether to search just the top directory or all directories.</param>
        public static void SetReadOnlyAttribute(string directory, bool readOnly, SearchOption options = SearchOption.AllDirectories)
        {
            directory.ThrowIfDirectoryDoesNotExist("directory");

            var di = new DirectoryInfo(directory);
            foreach (var fi in di.EnumerateFiles("*.*", options))
            {
                if (fi.IsReadOnly != readOnly)
                {
                    fi.IsReadOnly = readOnly;
                }
            }
        }

        /// <summary>
        /// Sets attributes on all files in the directory and subdirectory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="attributes"></param>
        public static void SetAllFileAttributes(string directory, FileAttributes attributes = FileAttributes.Normal)
        {
            directory.ThrowIfDirectoryDoesNotExist("directory");

            // Get all files and sub directories.
            string[] files = Directory.GetFiles(directory);
            string[] directories = Directory.GetDirectories(directory);

            // Kill all attributes, make files normal and directories just directories
            foreach (string file in files)
            {
                File.SetAttributes(file, attributes);
            }

            foreach (string dir in directories)
            {
                SetAllFileAttributes(dir, attributes);
            }

            //File.Delete(directory);
            // Finally set the main directory to a normal directory!
            //File.SetAttributes(directory, FileAttributes.Normal);
        } 
    }
}
