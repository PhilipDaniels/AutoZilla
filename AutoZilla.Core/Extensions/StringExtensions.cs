using AutoZilla.Core.Templates;
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

        public static string Templatize(this string text)
        {
            return Templatize(text, null);
        }

        public static string Templatize(this string text, IDictionary<string, object> variableValues)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var template = new Template(text, variableValues);
            return template.ReplacedText;
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
            if (minWidth < 0)
            {
                var msg = String.Format("minWidth must be at least 0, but {0} specified", minWidth);
                throw new ArgumentOutOfRangeException(msg);
            }

            if (maxWidth < 0)
            {
                var msg = String.Format("maxWidth must be at least 0, but {0} specified", maxWidth);
                throw new ArgumentOutOfRangeException(msg);
            }

            if (minWidth > maxWidth) 
            {
                var msg = String.Format("minWidth of {0} is more than the maxWidth of {1}", minWidth, maxWidth);
                throw new ArgumentOutOfRangeException(msg);
            }

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
    }
}
