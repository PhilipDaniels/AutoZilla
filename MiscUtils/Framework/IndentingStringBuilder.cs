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
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace MiscUtils.Framework
{
    public class IndentingStringBuilder
    {
        const string DefaultIndentation = "    ";
        StringBuilder sb;
        readonly string indentation;
        bool CurrentLineIsEmpty { get; set; }

        #region Constructors
        public IndentingStringBuilder()
            : this(DefaultIndentation)
        {
        }

        public IndentingStringBuilder(string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder();
            CurrentLineIsEmpty = true;
        }

        public IndentingStringBuilder(int capacity)
            : this(capacity, DefaultIndentation)
        {
        }

        public IndentingStringBuilder(int capacity, string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder(capacity);
            CurrentLineIsEmpty = true;
        }

        public IndentingStringBuilder(string value, string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder(value);
            CurrentLineIsEmpty = true;
        }

        public IndentingStringBuilder(int capacity, int maxCapacity)
            : this(capacity, maxCapacity, DefaultIndentation)
        {
        }

        public IndentingStringBuilder(int capacity, int maxCapacity, string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder(capacity, maxCapacity);
            CurrentLineIsEmpty = true;
        }

        public IndentingStringBuilder(string value, int capacity)
            : this(value, capacity, DefaultIndentation)
        {
        }

        public IndentingStringBuilder(string value, int capacity, string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder(value, capacity);
            CurrentLineIsEmpty = true;
        }

        public IndentingStringBuilder(string value, int startIndex, int length, int capacity)
            : this(value, startIndex, length, capacity, DefaultIndentation)
        {
        }

        public IndentingStringBuilder(string value, int startIndex, int length, int capacity, string indentationUnit)
        {
            indentation = indentationUnit;
            sb = new StringBuilder(value, startIndex, length, capacity);
            CurrentLineIsEmpty = true;
        }
        #endregion

        public int IndentationLevel
        {
            get
            {
                return _IndentationLevel;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "IndentationLevel cannot be less than 0.");
                _IndentationLevel = value;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int _IndentationLevel;

        public IndentingStringBuilder Indent()
        {
            IndentationLevel++;
            return this;
        }

        public IndentingStringBuilder OutDent()
        {
            IndentationLevel--;
            return this;
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public int Capacity
        {
            get
            {
                return sb.Capacity;
            }
            set
            {
                sb.Capacity = value;
            }
        }

        public int Length
        {
            get
            {
                return sb.Length;
            }
            set
            {
                sb.Length = value;
            }
        }

        public int MaxCapacity
        {
            get
            {
                return sb.MaxCapacity;
            }
        }

        public char this[int index]
        {
            get
            {
                return sb[index];
            }
            set
            {
                sb[index] = value;
            }
        }

        #region Proxied methods from StringBuilder, for appending etc.
        public IndentingStringBuilder Append(bool value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(byte value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(char value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(char[] value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(decimal value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(double value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(float value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(int value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(long value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(object value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder Append(short value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        // This is a VB type, and is not CLSCompliant.
        // The System.Text.StringBuilder class is the same.
        public IndentingStringBuilder Append(sbyte value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        // This is a VB type, and is not CLSCompliant.
        // The System.Text.StringBuilder class is the same.
        public IndentingStringBuilder Append(uint value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        // This is a VB type, and is not CLSCompliant.
        // The System.Text.StringBuilder class is the same.
        public IndentingStringBuilder Append(ulong value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        // This is a VB type, and is not CLSCompliant.
        // The System.Text.StringBuilder class is the same.
        public IndentingStringBuilder Append(ushort value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }


        public IndentingStringBuilder Append(string value)
        {
            AppendIndentationIfNecessary();
            sb.Append(value);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder AppendLine()
        {
            AppendIndentationIfNecessary();
            sb.AppendLine();
            CurrentLineIsEmpty = true;
            return this;
        }

        public IndentingStringBuilder AppendLine(string value)
        {
            AppendIndentationIfNecessary();
            sb.AppendLine(value);
            CurrentLineIsEmpty = true;
            return this;
        }

        public IndentingStringBuilder AppendFormat(string format, object arg0)
        {
            AppendIndentationIfNecessary();
            sb.AppendFormat(format, arg0);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder AppendFormat(string format, params object[] args)
        {
            AppendIndentationIfNecessary();
            AppendFormat(CultureInfo.InvariantCulture, format, args);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            AppendIndentationIfNecessary();
            sb.AppendFormat(provider, format, args);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder AppendFormat(string format, object arg0, object arg1)
        {
            AppendIndentationIfNecessary();
            sb.AppendFormat(format, arg0, arg1);
            CurrentLineIsEmpty = false;
            return this;
        }

        public IndentingStringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
        {
            AppendIndentationIfNecessary();
            sb.AppendFormat(format, arg0, arg1, arg2);
            CurrentLineIsEmpty = false;
            return this;
        }
        #endregion

        void AppendIndentationIfNecessary()
        {
            if (!CurrentLineIsEmpty)
                return;

            CurrentLineIsEmpty = false;

            for (int i = 0; i < IndentationLevel; i++)
            {
                sb.Append(indentation);
            }
        }
    }
}
