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
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MiscUtils.Data
{
    public static class DataViewExtensions
    {
        /// <summary>
        /// Return a list of all the column names in the view. This is the same as the underlying table, I think.
        /// </summary>
        /// <param name="dataView">The data view that you want the column names from.</param>
        /// <returns>List of column names.</returns>
        public static IEnumerable<string> ColumnNames(this DataView dataView)
        {
            dataView.ThrowIfNull("dataView");

            return dataView.Table.ColumnNames();
        }

        public static string ToCsv(this DataView dataView)
        {
            dataView.ThrowIfNull("dataView");

            return dataView.ToCsv(Encoding.UTF8);
        }

        public static string ToCsv(this DataView dataView, Encoding encoding)
        {
            dataView.ThrowIfNull("dataView");
            encoding.ThrowIfNull("encoding");

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, encoding))
            {
                WriteCsv(dataView, sw, ",", Environment.NewLine, null);
                sw.Flush();
                return encoding.GetString(ms.ToArray());
            }
        }

        public static void WriteCsv(this DataView dataView, TextWriter writer)
        {
            dataView.WriteCsv(writer, ",", Environment.NewLine, null);
        }

        public static void WriteCsv
            (
            this DataView dataView,
            TextWriter writer,
            string fieldSeparator,
            string recordSeparator,
            IEnumerable<string> columnsToWrite = null
            )
        {
            dataView.ThrowIfNull("dataView");
            writer.ThrowIfNull("writer");
            fieldSeparator.ThrowIfNull("fieldSeparator");
            recordSeparator.ThrowIfNull("recordSeparator");

            if (columnsToWrite == null || columnsToWrite.Count() == 0)
            {
                columnsToWrite = dataView.ColumnNames();
            }
            else
            {
                var unknownColumns = columnsToWrite.Except(dataView.ColumnNames(), StringComparer.OrdinalIgnoreCase);
                if (unknownColumns.Count() > 0)
                {
                    throw new ArgumentException("There are unknonwn columns.");
                }
            }

            // Write column headings.
            foreach (string column in columnsToWrite)
            {
                writer.Write(column);
                writer.Write(fieldSeparator);
            }
            writer.Write(recordSeparator);

            // Write data.
            foreach (DataRowView row in dataView)
            {
                bool first = true;
                foreach (string columnName in columnsToWrite)
                {
                    var column = row[columnName];
                    if (!first)
                    {
                        writer.Write(fieldSeparator);
                    }
                    writer.Write(column.ToString());
                    first = false;
                }

                writer.Write(recordSeparator);
            }
        }
    }
}
