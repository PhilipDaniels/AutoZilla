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
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MiscUtils.Data
{
    public static class DataTableExtensions
    {
        public static void SetReadOnly(this DataTable dataTable)
        {
            dataTable.SetReadOnly(true);
        }

        public static void SetReadOnly(this DataTable dataTable, bool readOnly)
        {
            dataTable.ThrowIfNull("dataTable");

            foreach (DataColumn dc in dataTable.Columns)
            {
                if (String.IsNullOrEmpty(dc.Expression))
                {
                    dc.ReadOnly = readOnly;
                }
            }
        }

        /// <summary>
        /// Copies the rows from <paramref name="sourceTable"/> to <paramref name="destinationTable"/>,
        /// but only if the columns actually exist in <paramref name="destinationTable"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// It used the CopyColumns routine.
        /// </summary>
        /// <param name="sourceTable">The table to copy rows from.</param>
        /// <param name="destinationTable">The table to copy rows to.</param>
        public static void CopyRows(this DataTable sourceTable, DataTable destinationTable)
        {
            sourceTable.ThrowIfNull("sourceTable");
            destinationTable.ThrowIfNull("targetTable");

            foreach (DataRow sourceRow in sourceTable.Rows)
            {
                DataRow targetRow = destinationTable.NewRow();
                sourceRow.CopyRow(targetRow);
                destinationTable.Rows.Add(targetRow);
            }
        }

        /// <summary>
        /// Return a list of all the column names in the table.
        /// </summary>
        /// <param name="dataTable">The data table that you want the column names from.</param>
        /// <returns>List of column names.</returns>
        public static IEnumerable<string> ColumnNames(this DataTable dataTable)
        {
            dataTable.ThrowIfNull("dataTable");

            var columnNames = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            return columnNames;
        }

        /// <summary>
        /// Determines whether the DataTable contains any changed rows.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>True if the table has at least one changed row, false otherwise.</returns>
        public static bool HasChangedRows(this DataTable table)
        {
            table.ThrowIfNull("table");

            foreach (DataRow row in table.Rows)
            {
                if (!row.RowState.Equals(DataRowState.Unchanged))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToCsv(this DataTable dataTable)
        {
            dataTable.ThrowIfNull("dataTable");

            return dataTable.DefaultView.ToCsv();
        }

        public static string ToCsv(this DataTable dataTable, Encoding encoding)
        {
            dataTable.ThrowIfNull("dataTable");

            return dataTable.DefaultView.ToCsv(encoding);
        }

        public static void WriteCsv(this DataTable dataTable, TextWriter writer)
        {
            dataTable.ThrowIfNull("dataTable");

            dataTable.DefaultView.WriteCsv(writer, ",", Environment.NewLine, null);
        }

        public static void WriteCsv
            (
            this DataTable dataTable,
            TextWriter writer,
            string fieldSeparator,
            string recordSeparator,
            IEnumerable<string> columnsToWrite = null
            )
        {
            dataTable.ThrowIfNull("dataTable");

            dataTable.DefaultView.WriteCsv(writer, fieldSeparator, recordSeparator, columnsToWrite);
        }
    }
}
