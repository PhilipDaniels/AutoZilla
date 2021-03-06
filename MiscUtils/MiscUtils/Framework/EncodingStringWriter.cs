﻿// Copyright 2013, 2014 Philip Daniels - http://www.philipdaniels.com/
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
using System.IO;
using System.Text;

namespace MiscUtils.Framework
{
    /// <summary>
    /// A subclass of StringWriter that allows the encoding to be set.
    /// </summary>
    public class EncodingStringWriter : StringWriter
    {
        readonly Encoding encoding;

        public EncodingStringWriter(StringBuilder builder, IFormatProvider formatProvider, Encoding encoding)
            : base(builder, formatProvider)
        {
            encoding.ThrowIfNull("encoding");

            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
