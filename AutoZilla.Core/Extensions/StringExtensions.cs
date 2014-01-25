using AutoZilla.Core.Templates;
using AutoZilla.Core.Validation;
using System;
using System.Collections.Generic;

namespace AutoZilla.Core.Extensions
{
    public static class StringExtensions
    {
        public static string TrimOneLeadingNewLine(this string text)
        {
            if (text.StartsWith(Environment.NewLine))
                return text.Substring(2);
            else
                return text;
        }

        public static string PadAndAlign(this string text, int width)
        {
            return PadAndAlign(text, width, width, Alignment.Left, ' ');
        }

        public static string PadAndAlign(this string text, int minWidth, int maxWidth)
        {
            return PadAndAlign(text, minWidth, maxWidth, Alignment.Left, ' ');
        }

        public static string PadAndAlign(this string text, int minWidth, int maxWidth, Alignment alignment)
        {
            return PadAndAlign(text, minWidth, maxWidth, alignment, ' ');
        }

        public static string PadAndAlign(this string text, int minWidth, int maxWidth, Alignment alignment, char paddingChar)
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
                        throw new Exception("Unhandled alignment.");
                }
            }
            else if (text.Length < minWidth)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        text = text.PadRight(minWidth, paddingChar);
                        break;
                    case Alignment.Right:
                        text = text.PadLeft(minWidth, paddingChar);
                        break;
                    case Alignment.Center:
                        var leftSpaces = (minWidth - text.Length) / 2;
                        text = new String(paddingChar, leftSpaces) + text;
                        text = text.PadRight(minWidth, paddingChar);
                        break;
                    default:
                        throw new Exception("Unhandled alignment.");
                }
            }

            return text;
        }

        public static string Before(this string s, string value)
        {
            return s.Before(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string Before(this string s, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (s == null)
                return null;
            int index = s.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return s.Substring(0, index);
        }

        public static string After(this string s, string value)
        {
            return s.After(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string After(this string s, string value, StringComparison comparisonType)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            if (s == null)
                return null;
            int index = s.IndexOf(value, comparisonType);
            if (index == -1)
                return null;
            else
                return s.Substring(index + value.Length);
        }

        public static void BeforeAndAfter(this string s, string value, out string before, out string after)
        {
            s.BeforeAndAfter(value, StringComparison.InvariantCultureIgnoreCase, out before, out after);
        }

        public static void BeforeAndAfter(this string s, string value, StringComparison comparisonType, out string before, out string after)
        {
            value.ThrowIfNull("value", "You cannot search for a null value.");

            before = s.Before(value, comparisonType);
            after = s.After(value, comparisonType);
        }

        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            return s.IndexOf(value, comparisonType) != -1;
        }
    }
}
