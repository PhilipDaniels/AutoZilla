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
using System.Data.SqlClient;
using System.Reflection;

namespace MiscUtils.Data
{
    /// <summary>
    /// Extension methods for the SqlBulkCopy class.
    /// </summary>
    public static class SqlBulkCopyExtensions
    {
        static FieldInfo rowsCopiedField = null;

        /// <summary>
        /// Retrieve the total number of rows copied in a SqlBulkCopy operation.
        /// </summary>
        /// <param name="bulkCopy">The bulk copy object.</param>
        /// <returns>Total number of rows copied.</returns>
        public static int TotalRowsCopied(this SqlBulkCopy bulkCopy)
        {
            bulkCopy.ThrowIfNull("bulkCopy");

            if (rowsCopiedField == null)
            {
                rowsCopiedField = typeof(SqlBulkCopy).GetField
                    (
                    "_rowsCopied",
                    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance
                    );
            }

            return (int)rowsCopiedField.GetValue(bulkCopy);
        }
    }
}
