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
using System.Data;

namespace MiscUtils.Data
{
    /// <summary>
    /// Extension methods to IDataParamter to make setting nullable parameters easier.
    /// </summary>
    public static class IDataParameterExtensions
    {
        /// <summary>
        /// Sets the <code>.Value</code> property of <paramref name="prm"/>.
        /// If the string is null, or Trim()s to empty string then the <code>.Value</code>
        /// is set to DBNull.Value, otherwise it is set to the value of the text
        /// field, with any *'s replaced with %'s.
        /// </summary>
        /// <param name="parameter">The parameter to have its value set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue(this IDataParameter parameter, string value)
        {
            parameter.ThrowIfNull("parameter");

            if (String.IsNullOrWhiteSpace(value))
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value.Trim();
        }

        /// <summary>
        /// Sets the <code>.Value</code> property of <paramref name="prm"/>.
        /// If the value is null, then the <code>.Value</code>
        /// is set to DBNull.Value, otherwise it is set to <paramref name="value"/>.
        /// </summary>
        /// <param name="parameter">The parameter to have its value set.</param>
        /// <param name="value">The value to set.</param>
        public static void SetValue<T>(this IDataParameter parameter, T? value)
            where T : struct
        {
            parameter.ThrowIfNull("parameter");

            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }
    }
}
