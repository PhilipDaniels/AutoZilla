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

using MiscUtils.Framework;
using System.Data;
using System.Data.SqlClient;

namespace MiscUtils.Data
{
    public static class SqlParameterCollectionExtensions
    {
        public static SqlParameter AddWithNullableValue<T>
            (
            this SqlParameterCollection parameters,
            string parameterName,
            T? value
            )
        where T : struct
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        /// <summary>
        /// A convenience method for adding a string with size and value.
        /// </summary>
        public static SqlParameter AddString
            (
            this SqlParameterCollection parameters,
            string parameterName,
            int size,
            string value
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter(parameterName, SqlDbType.VarChar, size);
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        /// <summary>
        /// When adding an nvarchar, size (for this method) is just the length of the column!
        /// </summary>
        /// <param name="parameters">The collection.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="size">The length of the column, as you typed it in SSMS.</param>
        /// <param name="value">The string value.</param>
        /// <returns>New SqlParameter.</returns>
        public static SqlParameter AddNVString
            (
            this SqlParameterCollection parameters,
            string parameterName,
            int size,
            string value
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            if (size != -1)
                size *= 2;

            var p = new SqlParameter(parameterName, SqlDbType.NVarChar);
            p.SetValue(value);
            parameters.Add(p);
            return p;
        }

        public static SqlParameter AddOutputId
            (
            this SqlParameterCollection parameters,
            string parameterName = "Id",
            SqlDbType parameterType = SqlDbType.Int
            )
        {
            parameters.ThrowIfNull("parameters");
            parameterName.ThrowIfNullOrEmpty("parameterName");

            var p = new SqlParameter();
            p.ParameterName = parameterName;
            p.Direction = ParameterDirection.Output;
            p.SqlDbType = parameterType;
            parameters.Add(p);
            return p;
        }
    }
}
