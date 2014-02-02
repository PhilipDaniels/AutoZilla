using System;

namespace AutoZilla.Core.GlobalHotKeys
{
    /// <summary>
    /// A helper class for converting the <code>Modifiers</code> enum
    /// to and from its string representation.
    /// </summary>
    public static class ModifiersHelper
    {
        /// <summary>
        /// Convert a <code>Modifiers</code> object into a human-readable form.
        /// </summary>
        /// <param name="modifiers">The modifiers.</param>
        /// <returns>String representation.</returns>
        public static string ToString(Modifiers modifiers)
        {
            string result = "";
            if (modifiers.HasFlag(Modifiers.Shift))
                result += "S";
            if (modifiers.HasFlag(Modifiers.Ctrl))
                result += "C";
            if (modifiers.HasFlag(Modifiers.Alt))
                result += "A";
            if (modifiers.HasFlag(Modifiers.Win))
                result += "W";

            return result;
        }

        /// <summary>
        /// Attempt to parse a string into a <code>Modifier</code>.
        /// This is the inverse of the <code>ToString</code> method,
        /// i.e. these two methods can be used for round-tripping.
        /// </summary>
        /// <remarks>
        /// We also support AutoHotKey style strings, using '+^!#'
        /// for Shift, Control, Alt and Win respectively.
        /// </remarks>
        /// <param name="s">The string to parse.</param>
        /// <param name="result">Parsed Modifier object.</param>
        /// <returns>True if the parsing succeeded, false otherwise.</returns>
        public static bool TryParse(string s, out Modifiers result)
        {
            result = Modifiers.None;

            if (String.IsNullOrWhiteSpace(s))
                return false;

            foreach (char c in s)
            {
                switch (c)
                {
                    case 'S':
                    case '+':
                        result |= Modifiers.Shift;
                        break;
                    case 'C':
                    case '^':
                        result |= Modifiers.Ctrl;
                        break;
                    case 'A':
                    case '!':
                        result |= Modifiers.Alt;
                        break;
                    case 'W':
                    case '#':
                        result |= Modifiers.Win;
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }
    }
}
