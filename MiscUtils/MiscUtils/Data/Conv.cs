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

using MiscUtils.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MiscUtils.Data
{
    /// <summary>
    /// A class for doing conversions, in the style of System.Convert.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Conv", Justification="My version of System.Convert.")]
    public static class Conv
    {
        /// <summary>
        /// The set of strings that are considered to equate to <c>true</c> by the
        /// <see cref="ToBoolean"/> method.
        /// The strings are used in case-insensitive comparisons.
        /// </summary>
        static List<string> trueStrings = new List<string> { "TRUE", "T", "YES", "Y", "1" };

        /// <summary>
        /// The set of strings that are considered to equate to <c>false</c> by the
        /// <see cref="ToBoolean"/> method.
        /// The strings are used in case-insensitive comparisons.
        /// </summary>
        static List<string> falseStrings = new List<string> { "FALSE", "F", "NO", "N", "0" };

        /// <summary>
        /// Convert a <paramref name="value"/> to the corresponding enumeration value. First checks to see if the
        /// <paramref name="value"/> is an integer, it is is then tries to cast the number to the enum value. If the
        /// value is not a number and is a <c>Char</c> or a <c>String</c> of length 1, then convert that
        /// character to an integer and try and cast it to the enum value. Otherwise, if the <paramref name="value"/>
        /// is a string of more than length 1, try and match the enum by name using <c>Enum.Parse</c>.
        /// </summary>
        /// <param name="enumType">The target enum type.</param>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidOperationException">T is not an enum type.</exception>
        /// <exception cref="InvalidCastException">The value cannot be converted.</exception>
        /// <returns>An enum value of the specified type.</returns>
        public static object ToEnum(Type enumType, object value)
        {
            enumType.ThrowIfNull("enumType");
            value.ThrowIfNull("value");

            if (!enumType.IsEnum)
                throw new InvalidOperationException("Result type (" + enumType.FullName + ") is not an enum.");

            Type underlyingType = Enum.GetUnderlyingType(enumType);

            object convertedValue = null;
            if (IsIntegerType(value))
            {
                convertedValue = Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            }
            else if (value.GetType() == typeof(Char))
            {
                convertedValue = Convert.ChangeType((char)value, underlyingType, CultureInfo.InvariantCulture);
            }
            else if (value.GetType() == typeof(String))
            {
                string s = value.ToString();
                if (s.Length < 1)
                {
                    throw new InvalidCastException("Cannot convert empty string to enum " + enumType.FullName);
                }
                else if (s.Length == 1)
                {
                    convertedValue = Convert.ChangeType(s[0], underlyingType, CultureInfo.InvariantCulture);
                }
                else
                {
                    convertedValue = Enum.Parse(enumType, s);
                }
            }
            else
            {
                throw new InvalidCastException("Cannot convert " + value.ToString() + " to enumeration.");
            }

            if (!Enum.IsDefined(enumType, convertedValue))
            {
                string msg = String.Concat("The value '", convertedValue, "' does not map to a member of enumeration ", enumType.FullName);
                throw new InvalidCastException(msg);
            }

            return Enum.ToObject(enumType, convertedValue);
        }

        /// <summary>
        /// Convert a <paramref name="value"/> to the corresponding enumeration value. First checks to see if the
        /// <paramref name="value"/> is a number, it is is then tries to cast the number to the enum value. If the
        /// value is not a number and is a <c>Char</c> or a <c>String</c> of length 1, then convert that
        /// character to an integer and try and cast it to the enum value. Otherwise, if the <paramref name="value"/>
        /// is a string of more than length 1, try and match the enum by name using <c>Enum.Parse</c>.
        /// </summary>
        /// <typeparam name="T">The target enum type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidOperationException">T is not an enum type.</exception>
        /// <exception cref="InvalidCastException">The value cannot be converted.</exception>
        /// <returns>An enum value of the specified type.</returns>
        public static T ToEnum<T>(object value)
            where T : struct
        {
            return (T)ToEnum(typeof(T), value);
        }

        /// <summary>
        /// Convert a <paramref name="value"/> to a <c>Boolean</c>. If the <paramref name="value"/> is a
        /// number, then return false< if it is 0, else true. If the <paramref name="value"/>
        /// is not a number then it is converted to a string, and matched against an internal list
        /// of strings considered true and false.
        /// </summary>
        /// <remarks>
        /// The string collections are immutable due to reasons of thread-safety surrounding
        /// static collections.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="InvalidCastException">The value could not be converted.</exception>
        /// <returns>true or false, as appropriate.</returns>
        public static bool ToBoolean(object value)
        {
            value.ThrowIfNull("value");
                
            if (value.GetType() == typeof(Boolean))
                return (Boolean)value;

            if (Conv.IsNumeric(value))
            {
                double vdub = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                return vdub != 0;
            }

            if (value.GetType() == typeof(Char) || value.GetType() == typeof(String))
            {
                string vstr = value.ToString().Trim();

                if (trueStrings != null && trueStrings.Contains(vstr, StringComparer.OrdinalIgnoreCase))
                    return true;
                if (falseStrings != null && falseStrings.Contains(vstr, StringComparer.OrdinalIgnoreCase))
                    return false;

                throw new InvalidCastException("Cannot convert '" + vstr + "' to Boolean.");
            }

            throw new InvalidCastException("Cannot convert '" + value.ToString() + "' to Boolean.");
        }

        /// <summary>
        /// Checks to see if the <paramref name="typeCode"/> is an integer type.
        /// Char is not considered to be an integer type.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be an integer type.</returns>
        public static bool IsIntegerTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks to see if the <paramref name="typeCode"/> is a floating point type.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be an integer type.</returns>
        public static bool IsFloatingPointTypeCode(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks to see if the TypeCode corresponds to a numeric type (integer or floating point).
        /// Char is not considered to be numeric.
        /// </summary>
        /// <param name="typeCode">The TypeCode to check.</param>
        /// <returns>True if the typeCode is considered to be a numeric type.</returns>
        public static bool IsNumericTypeCode(TypeCode typeCode)
        {
            return IsIntegerTypeCode(typeCode) || IsFloatingPointTypeCode(typeCode);
        }

        /// <summary>
        /// Checks to see if the <paramref name="type"/> is integer.
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be an integer type.</returns>
        public static bool IsIntegerType(Type type)
        {
            return IsIntegerTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of an integer type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be an integer type.</returns>
        public static bool IsIntegerType(object value)
        {
            value.ThrowIfNull("value");

            return IsIntegerTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if the <paramref name="type"/> is floating point.
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be a floating point type.</returns>
        public static bool IsFloatingPointType(Type type)
        {
            return IsFloatingPointTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of a floating point type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be a floating point type.</returns>
        public static bool IsFloatingPointType(object value)
        {
            value.ThrowIfNull("value");

            return IsFloatingPointTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if the Type is numeric (integer or floating point).
        /// </summary>
        /// <param name="type">The Type to check.</param>
        /// <returns>True if the type is considered to be a numeric type.</returns>
        public static bool IsNumericType(Type type)
        {
            return IsNumericTypeCode(Type.GetTypeCode(type));
        }

        /// <summary>
        /// Checks to see if the value is of a numeric type (integer or floating point).
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <exception cref="ArgumentNullException">The value is null.</exception>
        /// <returns>True if the value is considered to be a numeric type.</returns>
        public static bool IsNumericType(object value)
        {
            value.ThrowIfNull("value");

            return IsNumericTypeCode(Type.GetTypeCode(value.GetType()));
        }

        /// <summary>
        /// Checks to see if a value can be considered numeric. If the type is numeric then it
        /// automaticaly is. If the value is String or Char, an attempt is made to parse it
        /// as a double: if the attempt succeeds, true is returned.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is numeric.</returns>
        public static bool IsNumeric(object value)
        {
            // Ripped from Microsoft.VisualBasic.dll using ILSpy and hacked until it works.
            // http://stackoverflow.com/questions/1749966/c-sharp-how-to-determine-whether-a-type-is-a-number
            IConvertible convertible = value as IConvertible;
            if (convertible == null)
            {
                char[] array = value as char[];
                if (array == null)
                {
                    return false;
                }
                value = new string(array);
            }

            TypeCode typeCode = convertible.GetTypeCode();
            if (typeCode == TypeCode.String || typeCode == TypeCode.Char)
            {
                string valueAsString = convertible.ToString(null);

                /*
                 * PD: For my purposes I do not want to consider hex and octal as numbers.
                 * For example, "F" is considered numeric if you uncomment the code
                 * below, but you will get an exception if you try and convert it to a 
                 * double.
                try
                {
                    if (IsHexValue(value) || IsOctalValue(value))
                        return true;
                }
                catch (StackOverflowException ex)
                {
                    throw ex;
                }
                catch (OutOfMemoryException ex2)
                {
                    throw ex2;
                }
                catch (ThreadAbortException ex3)
                {
                    throw ex3;
                }
                catch (Exception)
                {
                    return false;
                }
                */

                double num2;
                return Double.TryParse(valueAsString, out num2);
            }

            return IsNumericTypeCode(typeCode);
        }

        /// <summary>
        /// Checks to see if a string value represents a hex string.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the string can be parsed as a hex number.</returns>
        public static bool IsHexValue(string value)
        {
            int result;
            return Int32.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Checks to see if a string value represents an octal number.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the string can be parsed as an octal number.</returns>
        public static bool IsOctalValue(string value)
        {
            try
            {
                Convert.ToInt32(value, 8);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (StackOverflowException)
            {
                return false;
            }
        }
    }
}
