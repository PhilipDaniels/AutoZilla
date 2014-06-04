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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MiscUtils.Framework
{
    /// <summary>
    /// Various handy string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes at most one leading <code>Environment.NewLine</code> from a string.
        /// This is rather handy when constructing in-line templates.
        /// </summary>
        /// <param name="str">The text to trim.</param>
        /// <returns>Text with up to one leading new line removed.</returns>
        public static string TrimOneLeadingNewLine(this string str)
        {
            str.ThrowIfNull("str");

            if (str.StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
                return str.Substring(2);
            else
                return str;
        }

        /// <summary>
        /// Left aligns <paramref name="str"/> and pads with spaces to the specified <paramref name="width"/>.
        /// </summary>
        /// <param name="str">The input text.</param>
        /// <param name="width">The final width of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string str, int width)
        {
            return PadAndAlign(str, width, width, Alignment.Left, ' ');
        }

        /// <summary>
        /// Left aligns <paramref name="str"/> and pads with spaces to the specified <paramref name="minWidth"/>,
        /// but trims the output if it exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="str">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string str, int minWidth, int maxWidth)
        {
            return PadAndAlign(str, minWidth, maxWidth, Alignment.Left, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="str"/> and pads 
        /// with spaces to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="str">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string str, int minWidth, int maxWidth, Alignment alignment)
        {
            return PadAndAlign(str, minWidth, maxWidth, alignment, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="str"/> and pads 
        /// with <paramref name="paddingChar"/> to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="str">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <param name="paddingCharacter">The character to pad with.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string str, int minWidth, int maxWidth, Alignment alignment, char paddingCharacter)
        {
            minWidth.ThrowIfLessThan(0, "minWidth");
            maxWidth.ThrowIfLessThan(0, "maxWidth");
            minWidth.ThrowIfMoreThan(maxWidth, "minWidth", "minWidth must be less than or equal to the maxWidth.");

            if (str == null)
                str = "";

            if (str.Length > maxWidth)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        // The left hand side is most important and should be retained.
                        str = str.Substring(0, maxWidth);
                        break;
                    case Alignment.Right:
                        // The right hand side is most important and should be retained.
                        str = str.Substring(str.Length - maxWidth);
                        break;
                    case Alignment.Center:
                        // The center is most important and should be retained.
                        var leftCharsToChop = (str.Length - maxWidth) / 2;
                        str = str.Substring(leftCharsToChop, maxWidth);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }
            else if (str.Length < minWidth)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        str = str.PadRight(minWidth, paddingCharacter);
                        break;
                    case Alignment.Right:
                        str = str.PadLeft(minWidth, paddingCharacter);
                        break;
                    case Alignment.Center:
                        var leftSpaces = (minWidth - str.Length) / 2;
                        str = new String(paddingCharacter, leftSpaces) + str;
                        str = str.PadRight(minWidth, paddingCharacter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }

            return str;
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string str, string value)
        {
            return str.Before(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string str, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (str == null)
                return null;
            int index = str.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return str.Substring(0, index);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string str, string value)
        {
            return str.After(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string str, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (str == null)
                return null;
            int index = str.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return str.Substring(index + value.Length);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public static void BeforeAndAfter(this string str, string value, out string before, out string after)
        {
            str.BeforeAndAfter(value, StringComparison.OrdinalIgnoreCase, out before, out after);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.        /// 
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
        public static void BeforeAndAfter(this string str, string value, StringComparison comparisonType, out string before, out string after)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            before = str.Before(value, comparisonType);
            after = str.After(value, comparisonType);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>A 2-tuple, where the first item is the substring before the value, and the second item is the substring after the value.</returns>
        public static Tuple<string, string> BeforeAndAfter(this string str, string value)
        {
            return BeforeAndAfter(str, value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="str"/> or
        /// <paramref name="str"/> is null then null is returned.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>A 2-tuple, where the first item is the substring before the value, and the second item is the substring after the value.</returns>
        public static Tuple<string, string> BeforeAndAfter(this string str, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            string before = str.Before(value, comparisonType);
            string after = str.After(value, comparisonType);
            var t = Tuple.Create(before, after);
            return t;
        }

        /// <summary>
        /// Check to see whether <paramref name="str"/> contains <paramref name="value"/>, and
        /// allow you to specify whether it is case-sensitive or not.
        /// </summary>
        /// <param name="str">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>True if text contains the value according to the comparisonType.</returns>
        public static bool Contains(this string str, string value, StringComparison comparisonType)
        {
            str.ThrowIfNull("str");
            value.ThrowIfNull("value");

            return str.IndexOf(value, comparisonType) != -1;
        }

        // TODO: Testing, validation, prm names.
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison)
        {
            str.ThrowIfNull("str");
            oldValue.ThrowIfNull("oldValue");
            newValue.ThrowIfNull("newValue");

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        /// <summary>
        /// Checks <paramref name="str"/> to see whether it matches the file-globbing
        /// pattern in <paramref name="pattern"/>.
        /// </summary>
        /// <param name="str">The string to test.</param>
        /// <param name="pattern">The wildcard, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static bool MatchesFileGlobPattern(this string str, string pattern)
        {
            var r = new Regex
                (
                "^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
                );

            return r.IsMatch(str);
        }

        /// <summary>
        /// Converts the input string to proper case, i.e. initial caps and
        /// lowercase rest.
        /// </summary>
        /// See also http://msdn.microsoft.com/en-us/library/system.globalization.textinfo.totitlecase.aspx
        /// which has limitations.
        /// <remarks>
        /// </remarks>
        /// <param name="str">The string to convert.</param>
        /// <returns>Proper cased string.</returns>
        public static string ToProperCase(this string str, CultureInfo culture)
        {
            str.ThrowIfNull("str");

            StringBuilder sb = new StringBuilder();
            bool emptyBefore = true;
            foreach (char ch in str)
            {
                char chThis = ch;
                if (Char.IsWhiteSpace(chThis))
                {
                    emptyBefore = true;
                }
                else
                {
                    if (Char.IsLetter(chThis) && emptyBefore)
                        chThis = Char.ToUpper(chThis, culture);
                    else
                        chThis = Char.ToLower(chThis, culture);
                    emptyBefore = false;
                }

                sb.Append(chThis);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Call String.Format in a safe fashion; if the number of args doesn't match the
        /// number of placeholders in the format string then just return the format string
        /// rather than throwing an exception.
        /// 
        /// If the format string is null, String.Empty is returned.
        /// </summary>
        /// <param name="culture">Culture to use for formatting.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">Arguments to be substituted.</param>
        /// <returns>Formatted string, or the original string if the formatting fails.</returns>
        public static string SafeFormat(this string format, CultureInfo culture, params object[] args)
        {
            try
            {
                string result = (format == null) ? String.Empty : String.Format(culture, format, args);
                return result;
            }
            catch (FormatException)
            {
                return format;
            }
        }

        /// <summary>
        /// Safely apply the Trim() operation to a string.
        /// If <paramref name="str"/> is null then null is
        /// returned, else <code>toTrim.Trim() is returned.</code>
        /// </summary>
        /// <param name="str">The string to trim. Can be null.</param>
        /// <param name="convertWhiteSpaceToNull">Whether to convert whitespace strings to null.</param>
        /// <returns>Trimmed string, or null.</returns>
        public static string SafeTrim(this string str, bool convertWhiteSpaceToNull = true)
        {
            if (str == null)
                return null;

            var s = str.Trim();
            if (convertWhiteSpaceToNull && s.Length == 0)
                return null;
            else
                return s;
        }

        /// <summary>
        /// Given a string InCamelCaseLikeThis, split it into words
        /// separated by a space.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <returns>String with spaces inserted at word breaks.</returns>
        public static string SplitCamelCaseIntoWords(this string str)
        {
            string result = Regex.Replace
                (
                str,
                "([A-Z][A-Z]*)",
                " $1",
                RegexOptions.Compiled
                );

            result = result.Trim();
            return result;
        }

        /// <summary>
        /// Return the leftmost characters of str. This function will not
        /// throw an exception if str is null or is shorter than length.
        /// If str is null then null is returned.
        /// </summary>
        /// <param name="str">The string to extract from.</param>
        /// <param name="length">The number of characters to extract. Can be zero.</param>
        /// <returns>Leftmost <paramref name="length"/> characters of <paramref name="str"/>.</returns>
        public static string Left(this string str, int length)
        {
            length.ThrowIfLessThan(0, "length");

            if (str == null)
                return null;

            if (length >= str.Length)
                return str;
            else
                return str.Substring(0, length);
        }

        /// <summary>
        /// Return the rightmost characters of str. This function will not
        /// throw an exception if str is null or is shorter than length.
        /// If str is null then null is returned.
        /// </summary>
        /// <param name="str">The string to extract from.</param>
        /// <param name="length">The number of characters to extract.</param>
        /// <returns>Rightmost <paramref name="length"/> characters of <paramref name="str"/>.</returns>
        public static string Right(this string str, int length)
        {
            length.ThrowIfLessThan(0, "length");

            if (str == null)
                return null;

            if (length >= str.Length)
                return str;
            else
                return str.Substring(str.Length - length, length);
        }

        // Should match decimal numbers.
        const string NUMBER_PATTERN = @"[0-9]([.,][0-9]{1,3})?";

        public static T GetLeadingNumber<T>(this string str)
        {
            str.ThrowIfNull("str");

            return GetLeadingNumber<T>(str, CultureInfo.InvariantCulture);
        }

        public static T GetLeadingNumber<T>(this string str, IFormatProvider provider)
        {
            str.ThrowIfNull("str");

            Match m = Regex.Match(str, "^" + NUMBER_PATTERN);
            T result = (T)Convert.ChangeType(m.Value, typeof(T), provider);
            return result;
        }

        public static T GetTrailingNumber<T>(this string str)
        {
            return GetTrailingNumber<T>(str, CultureInfo.InvariantCulture);
        }

        public static T GetTrailingNumber<T>(this string str, IFormatProvider provider)
        {
            str.ThrowIfNull("str");

            Match m = Regex.Match(str, NUMBER_PATTERN + "$");
            T result = (T)Convert.ChangeType(m.Value, typeof(T), provider);
            return result;
        }

        static readonly Regex BadFileNameCharacters = new Regex(@"[\\\/:\*\?""<>|]");

        public static string RemoveInvalidFileNameCharacters(this string str)
        {
            return BadFileNameCharacters.Replace(str, String.Empty);
        }

        /// <summary>
        /// Converts a delimited string to a list of objects of the requested type.
        /// </summary>
        /// <typeparam name="T">Type of thing in the output list.</typeparam>
        /// <param name="str">The input string. Can be null or empty, which results in an empty list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="allowDuplicates">Whether to allow duplicates in the result.</param>
        /// <returns>List of things.</returns>
        public static List<T> ToList<T>(this string str, string delimiter = ",", bool allowDuplicates = true)
            where T : IConvertible
        {
            var result = str.ToList((string s) => (T)Convert.ChangeType(s, typeof(T), CultureInfo.InvariantCulture), delimiter, allowDuplicates);
            return result;
        }

        /// <summary>
        /// Converts a delimited string to a list of objects of the requested type,
        /// using a specified conversion function.
        /// </summary>
        /// <typeparam name="T">Type of thing in the output list.</typeparam>
        /// <param name="str">The input string. Can be null or empty, which results in an empty list.</param>
        /// <param name="converter">A function to convert strings to objects of type <c>T</c>.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="allowDuplicates">Whether to allow duplicates in the result.</param>
        /// <returns>List of things.</returns>
        public static List<T> ToList<T>
            (
            this string str,
            Func<string, T> converter,
            string delimiter = ",",
            bool allowDuplicates = true
            )
        where T : IConvertible
        {
            var result = new List<T>();

            if (!String.IsNullOrEmpty(str))
            {
                string[] split = str.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                result = split.ToList().ConvertAll<T>(s => converter(s));
            }

            if (allowDuplicates)
                return result;
            else
                return result.Distinct().ToList();
        }
    }
}
