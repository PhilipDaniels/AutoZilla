using AutoZilla.Core.Templates;
using System;

namespace AutoZilla.Core.Extensions
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
        /// <param name="text">The text to trim.</param>
        /// <returns>Text with up to one leading new line removed.</returns>
        public static string TrimOneLeadingNewLine(this string text)
        {
            text.ThrowIfNull("text");

            if (text.StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
                return text.Substring(2);
            else
                return text;
        }

        /// <summary>
        /// Left aligns <paramref name="text"/> and pads with spaces to the specified <paramref name="width"/>.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="width">The final width of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string text, int width)
        {
            return PadAndAlign(text, width, width, Alignment.Left, ' ');
        }

        /// <summary>
        /// Left aligns <paramref name="text"/> and pads with spaces to the specified <paramref name="minWidth"/>,
        /// but trims the output if it exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string text, int minWidth, int maxWidth)
        {
            return PadAndAlign(text, minWidth, maxWidth, Alignment.Left, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="text"/> and pads 
        /// with spaces to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string text, int minWidth, int maxWidth, Alignment alignment)
        {
            return PadAndAlign(text, minWidth, maxWidth, alignment, ' ');
        }

        /// <summary>
        /// Applies the specified <paramref name="alignment"/> to <paramref name="text"/> and pads 
        /// with <paramref name="paddingChar"/> to the specified <paramref name="minWidth"/>, but trims the output if it 
        /// exceeds <paramref name="maxWidth"/>.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="minWidth">The minimum width of the result.</param>
        /// <param name="maxWidth">The maximum width of the result.</param>
        /// <param name="alignment">The alignment of the text.</param>
        /// <param name="paddingCharacter">The character to pad with.</param>
        /// <returns>Padded result.</returns>
        public static string PadAndAlign(this string text, int minWidth, int maxWidth, Alignment alignment, char paddingCharacter)
        {
            minWidth.ThrowIfLessThan(0, "minWidth");
            maxWidth.ThrowIfLessThan(0, "maxWidth");
            minWidth.ThrowIfMoreThan(maxWidth, "minWidth", "minWidth must be less than or equal to the maxWidth.");

            if (text == null)
                text = "";

            if (text.Length > maxWidth)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        // The left hand side is most important and should be retained.
                        text = text.Substring(0, maxWidth);
                        break;
                    case Alignment.Right:
                        // The right hand side is most important and should be retained.
                        text = text.Substring(text.Length - maxWidth);
                        break;
                    case Alignment.Center:
                        // The center is most important and should be retained.
                        var leftCharsToChop = (text.Length - maxWidth) / 2;
                        text = text.Substring(leftCharsToChop, maxWidth);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }
            else if (text.Length < minWidth)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        text = text.PadRight(minWidth, paddingCharacter);
                        break;
                    case Alignment.Right:
                        text = text.PadLeft(minWidth, paddingCharacter);
                        break;
                    case Alignment.Center:
                        var leftSpaces = (minWidth - text.Length) / 2;
                        text = new String(paddingCharacter, leftSpaces) + text;
                        text = text.PadRight(minWidth, paddingCharacter);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unhandled alignment: " + alignment.ToString());
                }
            }

            return text;
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string text, string value)
        {
            return text.Before(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters before the first occurence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text before the value.</returns>
        public static string Before(this string text, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (text == null)
                return null;
            int index = text.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return text.Substring(0, index);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string text, string value)
        {
            return text.After(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the characters after the first occurence of <paramref name="value"/>.
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>The portion of text after the value.</returns>
        public static string After(this string text, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (text == null)
                return null;
            int index = text.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return text.Substring(index + value.Length);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#")]
        public static void BeforeAndAfter(this string text, string value, out string before, out string after)
        {
            text.BeforeAndAfter(value, StringComparison.OrdinalIgnoreCase, out before, out after);
        }

        /// <summary>
        /// Returns the characters both before and after the first occurrence of <paramref name="value"/>.
        /// The search for <paramref name="value"/> is case-insensitive.        /// 
        /// If <paramref name="value"/> does not occur in <paramref name="text"/> or
        /// <paramref name="text"/> is null then null is returned.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <param name="before">The portion of text before the search value.</param>
        /// <param name="after">The portion of text after the search value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "4#")]
        public static void BeforeAndAfter(this string text, string value, StringComparison comparisonType, out string before, out string after)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            before = text.Before(value, comparisonType);
            after = text.After(value, comparisonType);
        }

        /// <summary>
        /// Check to see whether <paramref name="text"/> contains <paramref name="value"/>, and
        /// allow you to specify whether it is case-sensitive or not.
        /// </summary>
        /// <param name="text">The text to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="comparisonType">Type of string comparison to apply.</param>
        /// <returns>True if text contains the value according to the comparisonType.</returns>
        public static bool Contains(this string text, string value, StringComparison comparisonType)
        {
            text.ThrowIfNull("text");
            value.ThrowIfNull("value");

            return text.IndexOf(value, comparisonType) != -1;
        }
    }
}
