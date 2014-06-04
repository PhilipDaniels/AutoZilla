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
using System.Data;

namespace MiscUtils.Data
{
    public static class DataRowExtensions
    {
        /// <summary>
        /// Copies the columns from <paramref name="sourceRow"/> to <paramref name="targetRow"/>,
        /// but only if the columns actually exist in <paramref name="targetRow"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceRow">The row to copy columns from.</param>
        /// <param name="targetRow">The row to copy columns to.</param>
        public static void CopyRow(this DataRow sourceRow, DataRow targetRow)
        {
            sourceRow.ThrowIfNull("sourceRow");
            targetRow.ThrowIfNull("targetRow");

            foreach (DataColumn column in targetRow.Table.Columns)
            {
                if (sourceRow.Table.Columns.Contains(column.ColumnName))
                {
                    targetRow[column.ColumnName] = sourceRow[column.ColumnName];
                }
            }
        }
    }
}
