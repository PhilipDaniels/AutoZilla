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
using System.Globalization;
using System.Xml;

namespace MiscUtils.Framework
{
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Writes an attribute, but only if the attribute is non-null.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public static void WriteAttr(this XmlWriter writer, string attributeName, object value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");
            value.ThrowIfNull("value");

            Type valType = value.GetType();

            if (valType == typeof(bool))
            {
                WriteAttr(writer, attributeName, (bool)value);
            }
            else if (valType == typeof(bool?))
            {
                WriteAttr(writer, attributeName, (bool?)value);
            }
            else if (valType == typeof(byte))
            {
                WriteAttr(writer, attributeName, (byte)value);
            }
            else if (valType == typeof(byte?))
            {
                WriteAttr(writer, attributeName, (byte?)value);
            }
            else if (valType == typeof(short))
            {
                WriteAttr(writer, attributeName, (short)value);
            }
            else if (valType == typeof(short?))
            {
                WriteAttr(writer, attributeName, (short?)value);
            }
            else if (valType == typeof(int))
            {
                WriteAttr(writer, attributeName, (int)value);
            }
            else if (valType == typeof(int?))
            {
                WriteAttr(writer, attributeName, (int?)value);
            }
            else if (valType == typeof(long))
            {
                WriteAttr(writer, attributeName, (long)value);
            }
            else if (valType == typeof(long?))
            {
                WriteAttr(writer, attributeName, (long?)value);
            }
            else if (valType == typeof(decimal))
            {
                WriteAttr(writer, attributeName, (decimal)value);
            }
            else if (valType == typeof(decimal?))
            {
                WriteAttr(writer, attributeName, (decimal?)value);
            }
            else if (valType == typeof(float))
            {
                WriteAttr(writer, attributeName, (float)value);
            }
            else if (valType == typeof(float?))
            {
                WriteAttr(writer, attributeName, (float?)value);
            }
            else if (valType == typeof(double))
            {
                WriteAttr(writer, attributeName, (double)value);
            }
            else if (valType == typeof(double?))
            {
                WriteAttr(writer, attributeName, (double?)value);
            }
            else if (valType == typeof(DateTime))
            {
                WriteAttr(writer, attributeName, (DateTime)value);
            }
            else if (valType == typeof(DateTime?))
            {
                WriteAttr(writer, attributeName, (DateTime?)value);
            }
            else if (valType == typeof(Guid))
            {
                WriteAttr(writer, attributeName, (Guid)value);
            }
            else if (valType == typeof(Guid?))
            {
                WriteAttr(writer, attributeName, (Guid?)value);
            }
            else if (valType == typeof(string))
            {
                WriteAttr(writer, attributeName, (string)value);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unhandled attribute type: " + valType.ToString());
            }
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, string value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            writer.WriteAttributeString(attributeName, value);
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, bool? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, byte? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");
            
            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, short? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, int? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, long? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttributeString(attributeName, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, decimal value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (format == null)
                writer.WriteAttributeString(attributeName, value.ToString(CultureInfo.InvariantCulture));
            else
                writer.WriteAttributeString(attributeName, value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, decimal? value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                writer.WriteAttr(attributeName, value.Value, format);
            }
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, float value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (format == null)
                writer.WriteAttributeString(attributeName, value.ToString(CultureInfo.InvariantCulture));
            else
                writer.WriteAttributeString(attributeName, value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, float? value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                writer.WriteAttr(attributeName, value.Value, format);
            }
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, double value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (format == null)
                writer.WriteAttributeString(attributeName, value.ToString(CultureInfo.InvariantCulture));
            else
                writer.WriteAttributeString(attributeName, value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, double? value, string format = "0.00")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                writer.WriteAttr(attributeName, value.Value, format);
            }
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, DateTime value, string format = "s")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (format == null)
                writer.WriteAttributeString(attributeName, value.ToString(CultureInfo.InvariantCulture));
            else
                writer.WriteAttributeString(attributeName, value.ToString(format, CultureInfo.InvariantCulture));
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, DateTime? value, string format = "s")
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
                writer.WriteAttr(attributeName, value.Value, format);
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, Guid value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            writer.WriteAttributeString(attributeName, value.ToString());
        }

        public static void WriteAttr(this XmlWriter writer, string attributeName, Guid? value)
        {
            writer.ThrowIfNull("writer");
            attributeName.ThrowIfNullOrWhiteSpace("attributeName");

            if (value != null)
            {
                writer.WriteAttr(attributeName, value.Value);
            }
        }
    }
}
