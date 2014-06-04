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
using System.Globalization;
using System.IO;

namespace MiscUtils.Framework
{
    /// <summary>
    /// Provides utility methods for validating arguments to methods.
    /// </summary>
    public static class ArgumentValidators
    {
        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string parameterName)
        {
            return parameter.ThrowIfNull(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string parameterName)
        {
            return parameter.ThrowIfNullOrEmpty(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);
            if (parameter.Length == 0)
                throw new ArgumentOutOfRangeException(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string parameterName)
        {
            return parameter.ThrowIfNullOrWhiteSpace(parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string parameterName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, message);
            if (parameter.Trim().Length == 0)
                throw new ArgumentOutOfRangeException(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThan(value, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string parameterName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) < 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be less than {1} but {2} supplied.", parameterName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(parameterName, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than 
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThanOrEqualTo(value, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string parameterName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) <= 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be less than or equal to {1} but {2} supplied.", parameterName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThan(value, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string parameterName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) > 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be more than {1} but {2} supplied.", parameterName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(parameterName, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string parameterName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThanOrEqualTo(value, parameterName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string parameterName, string message)
        where T : IComparable<T>
        {
            if (parameter.CompareTo(value) >= 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be more than or equal to {1} but {2} supplied.", parameterName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(parameterName, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws a <code>ArgumentNullException</code> if <paramref name="path"/> is null.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="path"/> is whitespace.
        /// Throws a <code>DirectoryNotFoundException</code> if the directory <paramref name="path"/> does not exist.
        /// </summary>
        /// <param name="path">Path of the directory.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfDirectoryDoesNotExist([ValidatedNotNull] this string path, string parameterName)
        {
            path.ThrowIfNullOrWhiteSpace(parameterName, "path must be specified.");

            if (!Directory.Exists(path))
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "The directory {0} does not exist.", path);
                throw new DirectoryNotFoundException(msg);
            }

            return path;
        }

        /// <summary>
        /// Throws a <code>ArgumentNullException</code> if <paramref name="path"/> is null.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="path"/> is whitespace.
        /// Throws a <code>FileNotFoundException</code> if the file <paramref name="path"/> does not exist.
        /// </summary>
        /// <param name="path">Path of the directory.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfFileDoesNotExist([ValidatedNotNull] this string path, string parameterName)
        {
            path.ThrowIfNullOrWhiteSpace(parameterName, "path must be specified.");

            if (!File.Exists(path))
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "The file {0} does not exist.", path);
                throw new FileNotFoundException(msg, path);
            }

            return path;
        }
    }
}
