using MiscUtils.Framework;
using System;
using System.Globalization;
using System.Text;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// A <code>WidthSpecification</code> says how text should be aligned
    /// and padded or trimmed. The syntax is
    /// [[PadChar]Alignment][MinWidth][.][MaxWidth]
    /// i.e. everything is optional. You can even create a <code>WidthSpecification</code>
    /// using a null string.
    /// <remarks>
    /// PadChar = any char
    /// Alignment = &lt; (align left), &gt; (align right) or ^ (align center)
    /// </remarks>
    /// </summary>
    internal class WidthSpecification : IEquatable<WidthSpecification>
    {
        /// <summary>
        /// The original text that was used to create the specification.
        /// </summary>
        string OriginalText { get; set; }

        /// <summary>
        /// The character that will be used for padding.
        /// </summary>
        public char PadChar { get; private set; }

        /// <summary>
        /// The alignment (left, right, center).
        /// </summary>
        public Alignment Alignment { get; private set; }

        /// <summary>
        /// The minimum width of the output. If the text is shorther than this
        /// then it will be padded to this length.
        /// </summary>
        public int MinWidth { get; private set; }

        /// <summary>
        /// The maximum width of the output. If the text is longer than this
        /// then it will be trimmed to this length.
        /// </summary>
        public int MaxWidth { get; private set; }

        /// <summary>
        /// Create a new <code>WidthSpecification</code> with all
        /// values set to defaults.
        /// </summary>
        public WidthSpecification()
            : this("")
        {
        }

        /// <summary>
        /// Create a new <code>WidthSpecification</code> with values determined
        /// by parsing the <paramref name="text"/> parameter. If the specification
        /// is invalid then an exception may be thrown.
        /// </summary>
        /// <param name="text">Textual specification.</param>
        public WidthSpecification(string text)
        {
            OriginalText = text;
            PadChar = ' ';
            Alignment = Alignment.Left;
            MinWidth = 0;
            MaxWidth = Int32.MaxValue;

            if (!String.IsNullOrWhiteSpace(text))
                Parse(text);
        }

        /// <summary>
        /// Returns the canonical form of the parsed <code>WidthSpecification</code>.
        /// This is not neccessarily the same as what you passed in, because the
        /// <code>PadChar</code> is always included for example, and whitespace
        /// around the widths will have been stripped.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(PadChar);
            sb.Append((char)Alignment);
            if (MinWidth == MaxWidth)
            {
                sb.Append(MinWidth);
            }
            else
            {
                if (MinWidth > 0)
                    sb.Append(MinWidth);
                sb.Append('.');
                if (MaxWidth < Int32.MaxValue)
                    sb.Append(MaxWidth);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the object's hash code.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            // See http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + PadChar.GetHashCode();
                hash = hash * 23 + Alignment.GetHashCode();
                hash = hash * 23 + MinWidth.GetHashCode();
                hash = hash * 23 + MaxWidth.GetHashCode();
                return hash;
            }
        }

        #region IEquatable<WidthSpecification> Members
        public override bool Equals(object obj)
        {
            return Equals((WidthSpecification)obj);
        }

        public bool Equals(WidthSpecification other)
        {
            if ((object)other == null)
                return false;

            // First check for an exact type match.
            if (!object.ReferenceEquals(GetType(), other.GetType()))
                return false;

            // If this type does not derive from object then add a call to base.Equals().
            return PadChar == other.PadChar &&
                Alignment == other.Alignment &&
                MinWidth == other.MinWidth &&
                MaxWidth == other.MaxWidth;
        }

        public static bool operator ==(WidthSpecification lhs, WidthSpecification rhs)
        {
            //Naive recursive loop: return lhs == null ? rhs == null : lhs.Equals(rhs);
            return (object)lhs == null ? (object)rhs == null : lhs.Equals(rhs);
        }

        public static bool operator !=(WidthSpecification lhs, WidthSpecification rhs)
        {
            //Naive recursive loop: return lhs == null ? rhs != null : !lhs.Equals(rhs);
            //return (object)lhs == null ? (object)rhs != null : !lhs.Equals(rhs);
            return !(lhs == rhs);
        }
        #endregion



        void Parse(string text)
        {
            // It starts PAN  where P = any pad char, A = alignment, N = digit or . or ""
            char c1 = text[0];
            bool c1_IsAlignment = IsAlignmentChar(c1);

            if (text.Length == 1)
            {
                // P or A or N? Actually things are ambiguous in this
                // case so this is just a reasonable default.
                if (c1_IsAlignment)
                {
                    Alignment = (Alignment)c1;
                }
                else if (Char.IsDigit(c1))
                {
                    // Assume it's a width.
                    MinWidth = Int32.Parse(c1.ToString(), CultureInfo.InvariantCulture);
                    MaxWidth = MinWidth;
                }
                else
                {
                    // No other thing it could be.
                    PadChar = c1;
                }

                return;
            }


            char c2 = text[1];
            bool c2_IsAlignment = IsAlignmentChar(c2);
            string numbers = "";

            if (c2_IsAlignment)
            {
                // We have PA... where P *may* be an alignment char.
                PadChar = c1;
                Alignment = (Alignment)c2;
                numbers = text.Substring(2);
            }
            else if (c1_IsAlignment && !c2_IsAlignment)
            {
                // We have A...
                Alignment = (Alignment)c1;
                numbers = text.Substring(1);
            }
            else
            {
                // We have N... or PN...
                if (Char.IsDigit(c1) || c1 == '.')
                {
                    numbers = text;
                }
                else
                {
                    PadChar = c1;
                    numbers = text.Substring(1);
                }
            }

            ParseNumbers(numbers);
        }

        void ParseNumbers(string numbers)
        {
            // We will allow this, it lets you do things like "<<"
            // which while not really meaningful, when coupled with the
            // default widths can be made to make sense.
            if (String.IsNullOrWhiteSpace(numbers))
                return;

            // 20
            // 20.
            //   .20
            // 20.20
            // .
            var parts = numbers.Split(new char[] { '.' });
            if (parts.Length == 1)
            {
                int n;
                if (!Int32.TryParse(parts[0], out n))
                {
                    var msg = String.Format(CultureInfo.InvariantCulture, "The width specification {0} is invalid.", OriginalText);
                    throw new ArgumentOutOfRangeException(msg);
                }
                MinWidth = n;
                MaxWidth = MinWidth;
            }
            else if (parts.Length == 2)
            {
                MinWidth = ConvertWidth(parts[0], 0);
                MaxWidth = ConvertWidth(parts[1], MaxWidth);
            }
            else
            {
                throw new ArgumentOutOfRangeException("A width specifier cannot have more than 2 numeric components (meaning min and max widths). You specified: " + OriginalText);
            }
        }

        int ConvertWidth(string width, int defaultIfNotSet)
        {
            if (String.IsNullOrWhiteSpace(width))
                return defaultIfNotSet;

            int result;
            if (!Int32.TryParse(width, out result))
            {
                var msg = String.Format(CultureInfo.InvariantCulture, "The width specification {0} is invalid.", OriginalText);
                throw new ArgumentOutOfRangeException(msg);
            }

            return result;
        }

        static bool IsAlignmentChar(char c)
        {
            return c == (char)Alignment.Left ||
                c == (char)Alignment.Center || 
                c == (char)Alignment.Right;
        }
    }
}
