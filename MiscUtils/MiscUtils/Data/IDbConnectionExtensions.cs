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

namespace MiscUtils.Data
{
    public static class IDbConnectionExtensions
    {
        /// <summary>
        /// Gets the schema of a table (or view) using the <c>IDataReader.GetSchemaTable()</c>
        /// method. This is good for basic information, but will not be full-fidelity compared
        /// to what is available from SQL server.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>DataTable containing the schema for the SQL table.</returns>
        public static DataTable GetSchemaTable(this IDbConnection connection, string tableName)
        {
            connection.ThrowIfNull("connection");
            tableName.ThrowIfNullOrEmpty("tableName");

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from " + tableName + " where 1 == 0";

                using (var rdr = cmd.ExecuteReader())
                {
                    return rdr.GetSchemaTable();
                }
            }
        }
    }
}
