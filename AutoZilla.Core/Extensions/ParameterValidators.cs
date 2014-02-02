using System;
using System.Globalization;
using System.IO;

namespace AutoZilla.Core.Extensions
{
    /// <summary>
    /// Provides utility methods for validating arguments to methods.
    /// </summary>
    public static class ParameterValidators
    {
        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string paramName)
        {
            return parameter.ThrowIfNull(paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// </summary>
        /// <typeparam name="T">Generic type of the argument.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string paramName)
        {
            return parameter.ThrowIfNullOrEmpty(paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);
            if (parameter.Length == 0)
                throw new ArgumentOutOfRangeException(paramName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string paramName)
        {
            return parameter.ThrowIfNullOrWhiteSpace(paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentNullException</code> if <paramref name="parameter"/> is null.
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is the empty string or whitespace.
        /// </summary>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);
            if (parameter.Trim().Length == 0)
                throw new ArgumentOutOfRangeException(paramName, message);

            return parameter;
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThan(value, paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) < 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be less than {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
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
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThanOrEqualTo(value, paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is less than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) <= 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be less than or equal to {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, message);
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
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThan(value, paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) > 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be more than {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
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
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThanOrEqualTo(value, paramName, null);
        }

        /// <summary>
        /// Throws an <code>ArgumentOutOfRangeException</code> if <paramref name="parameter"/> is more than
        /// or equal to <paramref name="value."/>
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="parameter">The parameter itself.</param>
        /// <param name="value">The value to compare against.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="message">Message to associate with the exception.</param>
        /// <returns><paramref name="parameter"/> if no exception is thrown.</returns>
        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
        where T : IComparable<T>
        {
            if (parameter.CompareTo(value) >= 0)
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "Parameter {0} cannot be more than or equal to {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
            }

            return parameter;
        }

        /// <summary>
        /// Throws a <code>ArgumentNullException</code> if <paramref name="path"/> is null.
        /// Throws a <code>ArgumentOutOfRangeException</code> if <paramref name="path"/> is whitespace.
        /// Throws a <code>DirectoryNotFoundException</code> if the directory <paramref name="path"/> does not exist.
        /// </summary>
        /// <param name="path">Path of the directory.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfDirectoryDoesNotExist([ValidatedNotNull] this string path, string paramName)
        {
            path.ThrowIfNullOrWhiteSpace(paramName, "path must be specified.");

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
        /// <param name="paramName">The name of the parameter.</param>
        /// <returns><paramref name="path"/> if no exception is thrown.</returns>
        public static string ThrowIfFileDoesNotExist([ValidatedNotNull] this string path, string paramName)
        {
            path.ThrowIfNullOrWhiteSpace(paramName, "path must be specified.");

            if (!File.Exists(path))
            {
                string msg = String.Format(CultureInfo.InvariantCulture, "The file {0} does not exist.", path);
                throw new FileNotFoundException(msg, path);
            }

            return path;
        }
    }
}
