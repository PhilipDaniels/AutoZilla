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
using System.Threading;

namespace MiscUtils.Data
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Associate the current user and locale with the SQL connection.
        /// The user and locale are inferred from the current thread.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public static void SetContextInfo(this SqlConnection connection)
        {
            connection.ThrowIfNull("connection");

            // Just be careful here.
            if (Thread.CurrentPrincipal == null ||
                Thread.CurrentPrincipal.Identity == null ||
                Thread.CurrentThread.CurrentUICulture == null
                )
                return;

            SetContextInfo
                (
                connection,
                Thread.CurrentPrincipal.Identity.Name,
                Thread.CurrentThread.CurrentUICulture.ToString()
                );
        }

        /// <summary>
        /// Associate the specified user and locale with the SQL connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="locale">The .net locale, e.g. "en-gb".</param>
        public static void SetContextInfo(this SqlConnection connection, string userId, string locale)
        {
            connection.ThrowIfNull("conn");
            userId.ThrowIfNullOrWhiteSpace("userId");

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SetContextInfo";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Locale", locale);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
