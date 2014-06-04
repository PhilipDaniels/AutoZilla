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
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MiscUtils.Data
{
    public static class IDataReaderExtensions
    {
        public static DataTable ToDataTable(this IDataReader reader)
        {
            reader.ThrowIfNull("reader");

            var dt = MakeTableFromReader(reader, "IDataReader table");
            return dt;
        }

        public static DataSet ToDataSet(this IDataReader reader)
        {
            reader.ThrowIfNull("reader");

            DataSet ds = new DataSet("Query Results");
            ds.Locale = CultureInfo.InvariantCulture;

            int tableNumber = 1;
            while (!reader.IsClosed)
            {
                string tableName = "Table " + tableNumber.ToString(CultureInfo.InvariantCulture);
                ds.Tables.Add(MakeTableFromReader(reader, tableName));
                tableNumber++;
            }

            return ds;
        }

        static DataTable MakeTableFromReader(IDataReader reader, string tableName)
        {
            reader.ThrowIfNull("reader");
            tableName.ThrowIfNull("tableName");

            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.TableName = tableName;
            dt.Load(reader);
            return dt;
        }

		/// <summary>
		/// Create an object of the specified type for every row in the DataReader.
		/// </summary>
		/// <typeparam name="T">The type of object to create.</typeparam>
		/// <param name="reader">The data reader to iterate.</param>
		/// <returns>An enumerable of objects of the specified type.</returns>
        public static IEnumerable<T> HydrateAll<T>(this IDataReader reader)
		{
			reader.ThrowIfNull("reader");

			return (from r in reader.CurrentRecords()
					let rec = r as IDataRecord
					select rec.Hydrate<T>()).AsEnumerable();
		}

		/// <summary>
		/// Create an object of the specified type for every row in the DataReader.
		/// A delegate is asked to do the actual object construction.
		/// </summary>
		/// <typeparam name="T">The type of object to create.</typeparam>
		/// <param name="reader">The data reader to iterate.</param>
		/// <param name="hydrationFunction">A delegate that can construct objects of type T.</param>
		/// <returns>An enumerable of objects of the specified type.</returns>
        public static IEnumerable<T> HydrateAll<T>(this IDataReader reader, Func<IDataRecord, T> hydrationFunction)
		{
            hydrationFunction.ThrowIfNull("hydrationFunction");

			return (from r in reader.CurrentRecords()
						   let rec = r as IDataRecord
						   select rec.Hydrate<T>(hydrationFunction)).AsEnumerable();
		}

        /// <summary>
        /// Iterate over all the rows in the current result set.
        /// </summary>
        /// <param name="reader">The data reader to iterate.</param>
        /// <returns>An enumerable of all the rows in the reader's current resultset.</returns>
        public static IEnumerable<IDataRecord> CurrentRecords(this IDataReader reader)
        {
            reader.ThrowIfNull("reader");

            while (reader.Read())
            {
                yield return reader;
            }
        }

        /// <summary>
        /// Copies the rows from <paramref name="sourceReader"/> to <paramref name="destinationTable"/>,
        /// but only if the columns actually exist in <paramref name="destinationTable"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceReader">The data reader to copy rows from.</param>
        /// <param name="destinationTable">The table to copy rows to.</param>
        public static void CopyRows(this IDataReader sourceReader, DataTable destinationTable)
        {
            sourceReader.ThrowIfNull("sourceReader");
            destinationTable.ThrowIfNull("destinationTable");

            var readerColumns = sourceReader.GetColumns();

            while (sourceReader.Read())
            {
                DataRow targetRow = destinationTable.NewRow();
                CopyData(readerColumns, sourceReader, targetRow);
                destinationTable.Rows.Add(targetRow);
            }
        }

        /// <summary>
        /// Copies 1 row from <paramref name="sourceReader"/> to <paramref name="destinationRow"/>,
        /// but only if the columns actually exist in <paramref name="destinationRow"/>.
        /// This routine can be used to "hydrate" entities from generic results sets.
        /// </summary>
        /// <param name="sourceReader">The row to copy columns from.</param>
        /// <param name="destinationRow">The row to copy columns to.</param>
        public static void CopyRow(this IDataReader sourceReader, DataRow destinationRow)
        {
            sourceReader.ThrowIfNull("sourceReader");
            destinationRow.ThrowIfNull("destinationRow");

            var readerColumns = sourceReader.GetColumns();
            CopyData(readerColumns, sourceReader, destinationRow);
        }

        /// <summary>
        /// Gets the columns in the reader.	
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>An enumerable of column names.</returns>
        public static ReadOnlyCollection<string> GetColumns(this IDataReader reader)
        {
            reader.ThrowIfNull("reader");

            var columns = new List<string>();

            DataTable schemaTable = reader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                columns.Add(row["ColumnName"].ToString());
            }

            return columns.AsReadOnly();
        }

        /// <summary>
        /// Copies the data in the specified columns to the target.
        /// </summary>
        /// <param name="readerColumns">The reader columns.</param>
        /// <param name="sourceReader">The source reader.</param>
        /// <param name="destinationRow">The target row.</param>
        private static void CopyData(IEnumerable<string> readerColumns, IDataReader sourceReader, DataRow destinationRow)
        {
            readerColumns.ThrowIfNull("readerColumns");
            sourceReader.ThrowIfNull("sourceReader");
            destinationRow.ThrowIfNull("destinationRow");

            foreach (DataColumn column in destinationRow.Table.Columns)
            {
                if (readerColumns.Contains(column.ColumnName))
                {
                    destinationRow[column.ColumnName] = sourceReader[column.ColumnName];
                }
            }
        }
    }
}
