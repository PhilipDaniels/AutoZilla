using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace AutoZilla.Core.GlobalHotKeys
{
    /// <summary>
    /// Represents a combination of a <code>Keys.Key</code> and <code>Modifiers</code>.
    /// This can be more convenient than passing the two things around separately.
    /// </summary>
    public class ModifiedKey : IEquatable<ModifiedKey>
    {
        /// <summary>
        /// The set of modifiers.
        /// </summary>
        public Modifiers Modifiers { get; private set; }

        /// <summary>
        /// The key.
        /// </summary>
        public Keys Key { get; private set; }

        /// <summary>
        /// Construct a new <code>ModifiedKey</code> object.
        /// </summary>
        /// <param name="modifiers">The set of modifiers.</param>
        /// <param name="key">The key.</param>
        public ModifiedKey(Modifiers modifiers, Keys key)
        {
            Modifiers = modifiers;
            Key = key;
        }

        /// <summary>
        /// Produces a human readable representation of the <code>ModifiedKey</code>.
        /// </summary>
        /// <returns>String rep using CSAW for Control, Shift, Alt and Win modifiers.</returns>
        public override string ToString()
        {
            return ToString(Modifiers, Key);
        }

        /// <summary>
        /// Produces a human readable representation of the hot key info.
        /// Static version because sometimes we have these two pieces of information
        /// separately, not as an object.
        /// </summary>
        /// <param name="modifiers">The key modifiers.</param>
        /// <param name="key">The keycode.</param>
        /// <returns>String rep using CSAW for Control, Shift, Alt and Win modifiers.</returns>
        public static string ToString(Modifiers modifiers, Keys key)
        {
            var result = ModifiersHelper.ToString(modifiers) + "-" + key.ToString();
            return result;
        }

        /// <summary>
        /// Attemp to parse out a <code>ModifiedKey</code> from a string.
        /// The string should be of the form CSAW-L where the part before the '-'
        /// is the modifiers and is optional, and the part afterwards is the Key
        /// from the <code>Keys</code> enumeration.
        /// </summary>
        /// <param name="s">String to parse.</param>
        /// <param name="result">Parsed object.</param>
        /// <returns>True if the parsing succeeds, false otherwise.</returns>
        public static bool TryParse(string s, out ModifiedKey result)
        {
            result = null; 
            
            if (String.IsNullOrWhiteSpace(s))
                return false;
            
            s = s.Trim();

            string[] parts = s.Split('-');
            if (parts.Length == 1)
            {
                // Could be "L" or "PgUp" but not modified.
                Keys k;
                if (!Enum.TryParse(parts[0], out k))
                    return false;

                result = new ModifiedKey(Modifiers.None, k);
                return true;
            }
            else if (parts.Length == 2)
            {
                // Could be "CS-L" or some variant.
                Keys k;
                if (!Enum.TryParse(parts[1], out k))
                {
                    k = ConvertCharactorToKey(parts[1][0]);
                }

                Modifiers m;
                if (!ModifiersHelper.TryParse(parts[0], out m))
                    return false;

                result = new ModifiedKey(m, k);
                return true;
            } 
            else
            {
                return false;
            }
        }

        static IntPtr GetKeyboardLayout()
        {
            IntPtr fg = NativeMethods.GetForegroundWindow();
            uint idThread = NativeMethods.GetWindowThreadProcessId(fg, IntPtr.Zero);
            IntPtr hKL = NativeMethods.GetKeyboardLayout(idThread);
            return hKL;
        }

        /// <summary>
        /// Converts a .net character into the corresponding Windows Forms Virtual Key.
        /// </summary>
        /// <param name="character">The character to resolve.</param>
        /// <returns>Corresponding member from the Keys enum.</returns>
        public static Keys ConvertCharactorToKey(char character)
        {
            IntPtr hKL = GetKeyboardLayout();
            short vkWithModifiers = NativeMethods.VkKeyScanEx(character, hKL);
            // Virtual key code is returned in lowest byte.
            int vkOnly = vkWithModifiers & 0xFF;
            Keys formsKey = (Keys)vkOnly;
            return formsKey;
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
                hash = hash * 23 + Modifiers.GetHashCode();
                hash = hash * 23 + Key.GetHashCode();
                return hash;
            }
        }

        #region IEquatable<ModifiedKey> Members
        public override bool Equals(object obj)
        {
            return Equals((ModifiedKey)obj);
        }

        public bool Equals(ModifiedKey other)
        {
            if ((object)other == null)
                return false;

            // First check for an exact type match.
            if (!object.ReferenceEquals(GetType(), other.GetType()))
                return false;

            // If this type does not derive from object then add a call to base.Equals().
            return Modifiers == other.Modifiers &&
                Key == other.Key;
        }

        public static bool operator ==(ModifiedKey lhs, ModifiedKey rhs)
        {
            //Naive recursive loop: return lhs == null ? rhs == null : lhs.Equals(rhs);
            return (object)lhs == null ? (object)rhs == null : lhs.Equals(rhs);
        }

        public static bool operator !=(ModifiedKey lhs, ModifiedKey rhs)
        {
            //Naive recursive loop: return lhs == null ? rhs != null : !lhs.Equals(rhs);
            //return (object)lhs == null ? (object)rhs != null : !lhs.Equals(rhs);
            return !(lhs == rhs);
        }
        #endregion

        private ModifiedKey(IntPtr lParam)
        {
            var lpInt = (int)lParam;
            Key = (Keys)((lpInt >> 16) & 0xFFFF);
            Modifiers = (Modifiers)(lpInt & 0xFFFF);
        }

        internal static ModifiedKey GetFromMessage(Message m)
        {
            return !IsHotKeyMessage(m) ? null : new ModifiedKey(m.LParam);
        }

        internal static bool IsHotKeyMessage(Message m)
        {
            return m.Msg == NativeMethods.WM_HotKey_MSG_ID;
        }
    }
}
