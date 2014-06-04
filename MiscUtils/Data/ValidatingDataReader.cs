// Copyright 2014 Philip Daniels - http://www.philipdaniels.com/
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace MiscUtils.Data
{
    /// <summary>
    /// From http://www.csvreader.com/posts/validating_datareader.php
    /// </summary>
    public class ValidatingDataReader : IDataReader
    {
        /// <summary>
        /// If true, the <c>ValidatingDataReader</c> will trim input strings that are too
        /// long to fit in the destination column down to size rather than throwing an exception.
        /// </summary>
        public bool TrimLongStringsInsteadOfThrowing { get; set; }

        IDataReader wrappedReader;
        bool disposed;
        int currentRecord;
        DataRow[] lookup;
        string targetTableName;
        string targetDatabaseName;
        string targetServerName;

        public ValidatingDataReader
            (
            IDataReader readerToWrap,
            SqlConnection conn,
            SqlBulkCopy bcp,
            bool trimLongStringsInsteadOfThrowing
            )
        {
            readerToWrap.ThrowIfNull("reader");
            conn.ThrowIfNull("conn");
            bcp.ThrowIfNull("bcp");
            bcp.DestinationTableName.ThrowIfNullOrEmpty("bcp.DestinationTableName");

            TrimLongStringsInsteadOfThrowing = trimLongStringsInsteadOfThrowing; 
            wrappedReader = readerToWrap;
            targetTableName = bcp.DestinationTableName;
            targetDatabaseName = conn.Database;
            targetServerName = conn.DataSource;
            currentRecord = -1;

            ConnectionState origState = conn.State;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            ValidateBulkCopySourceColumnMappings(bcp.ColumnMappings);
            DataTable schemaTableOfDestinationTable = GetSchemaTable(conn, origState);

            lookup = new DataRow[readerToWrap.FieldCount];

            if (bcp.ColumnMappings.Count > 0)
            {
                DataRow[] columns = new DataRow[schemaTableOfDestinationTable.Rows.Count];
                Dictionary<string, int> columnLookup = new Dictionary<string, int>();

                foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                {
                    string columnName = (string)column["ColumnName"];
                    int columnOrdinal = (int)column["ColumnOrdinal"];
                    columns[columnOrdinal] = column;
                    columnLookup[columnName] = columnOrdinal;
                }

                ValidateBulkCopyDestinationColumnMappings(bcp.ColumnMappings, columnLookup, columns);
                CreateLookupFromColumnMappings(bcp.ColumnMappings, schemaTableOfDestinationTable);
            }
            else
            {
                foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                {
                    int columnOrdinal = (int)column["ColumnOrdinal"];
                    lookup[columnOrdinal] = column;
                }
            }
        }

        void CreateLookupFromColumnMappings
            (
            SqlBulkCopyColumnMappingCollection mappings,
            DataTable schemaTableOfDestinationTable
            )
        {
            // create lookup dest column definition by source index
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                int sourceIndex = -1;
                var sourceColumn = new SqlName(mapping.SourceColumn);
                if (!String.IsNullOrEmpty(sourceColumn.Name))
                {
                    sourceIndex = wrappedReader.GetOrdinal(sourceColumn.Name);
                }
                else
                {
                    sourceIndex = mapping.SourceOrdinal;
                }

                DataRow destColumnDef = null;
                var destColumn = new SqlName(mapping.DestinationColumn);
                if (!String.IsNullOrEmpty(destColumn.Name))
                {
                    destColumnDef = schemaTableOfDestinationTable.Rows.Cast<DataRow>().
                                    Single(c => (string)c["ColumnName"] == destColumn.Name);

                    //foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                    //{
                    //    if ((string)column["ColumnName"] == destColumn.Name)
                    //    {
                    //        destColumnDef = column;
                    //        break;
                    //    }
                    //}
                }
                else
                {
                    destColumnDef = schemaTableOfDestinationTable.Rows.Cast<DataRow>().
                                    Single(c => (int)c["ColumnOrdinal"] == mapping.DestinationOrdinal);

                    //foreach (DataRow column in schemaTableOfDestinationTable.Rows)
                    //{
                    //    if ((int)column["ColumnOrdinal"] == mapping.DestinationOrdinal)
                    //    {
                    //        destColumnDef = column;
                    //        break;
                    //    }
                    //}
                }

                lookup[sourceIndex] = destColumnDef;
            }
        }

        DataTable GetSchemaTable(SqlConnection conn, ConnectionState origState)
        {
            DataTable schemaTable = null;

            try
            {
                schemaTable = conn.GetSchemaTable(targetTableName);
            }
            catch (SqlException ex)
            {
                if (ex.Message.StartsWith("Invalid object name", StringComparison.OrdinalIgnoreCase))
                {
                    throw new SchemaException
                        (
                        "Destination table " + targetTableName +
                        " does not exist in database " + conn.Database +
                        " on server " + conn.DataSource + ".",
                        ex
                        );
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (origState == ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

            return schemaTable;
        }

        /// <summary>
        /// Check that every column specified in the mappings collection exists
        /// in the wrapped reader.
        /// </summary>
        void ValidateBulkCopySourceColumnMappings(SqlBulkCopyColumnMappingCollection mappings)
        {
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                var sc = new SqlName(mapping.SourceColumn);

                if (!String.IsNullOrEmpty(sc.Name))
                {
                    if (wrappedReader.GetOrdinal(sc.Name) == -1)
                    {
                        string bestFit = wrappedReader.GetColumns().SingleOrDefault(c => c.Equals(sc.Name, StringComparison.OrdinalIgnoreCase));

                        if (bestFit == null)
                        {
                            throw new SchemaException("Source column " + mapping.SourceColumn + " does not exist in source.");
                        }
                        else
                        {
                            throw new SchemaException
                                (
                                "Source column " + mapping.SourceColumn + " does not exist in source." +
                                " Column name mappings are case specific and best found match is " + bestFit + "."
                                );
                        }
                    }
                }
                else
                {
                    if (mapping.SourceOrdinal < 0 || mapping.SourceOrdinal >= wrappedReader.FieldCount)
                    {
                        throw new SchemaException("No column exists at index " + mapping.SourceOrdinal + " in source.");
                    }
                }
            }
        }

        void ValidateBulkCopyDestinationColumnMappings
            (
            SqlBulkCopyColumnMappingCollection mappings,
            Dictionary<string, int> columnLookup,
            DataRow[] columns
            )
        {
            foreach (SqlBulkCopyColumnMapping mapping in mappings)
            {
                var destColumn = new SqlName(mapping.DestinationColumn);

                if (!String.IsNullOrEmpty(destColumn.Name))
                {
                    if (!columnLookup.ContainsKey(destColumn.Name))
                    {
                        // If we can't find an exact match by case, try for a case-insensitive match.
                        string bestFit = columnLookup.Keys.SingleOrDefault(c => c.Equals(destColumn.Name, StringComparison.OrdinalIgnoreCase));

                        if (bestFit == null)
                        {
                            throw new SchemaException
                                (
                                "Destination column " + mapping.DestinationColumn +
                                " does not exist in destination table " + targetTableName +
                                " in database " + targetDatabaseName +
                                " on server " + targetServerName + "."
                                );
                        }
                        else
                        {
                            throw new SchemaException
                                (
                                "Destination column " + mapping.DestinationColumn +
                                " does not exist in destination table " + targetTableName +
                                " in database " + targetDatabaseName +
                                " on server " + targetServerName +
                                ". Column name mappings are case specific and best found match is " + bestFit + "."
                                );
                        }
                    }
                }
                else
                {
                    if (mapping.DestinationOrdinal < 0 || mapping.DestinationOrdinal >= columns.Length)
                    {
                        throw new SchemaException
                            (
                            "No column exists at index " + mapping.DestinationOrdinal +
                            " in destination table " + targetTableName +
                            " in database " + targetDatabaseName +
                            " on server " + targetServerName + "."
                            );
                    }
                }
            }
        }

        public int CurrentRecord
        {
            get
            {
                return currentRecord;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // managed resource releases
                if (disposing)
                {
                }

                // unmanaged resource releases
                (wrappedReader as IDisposable).Dispose();
                wrappedReader = null;
                disposed = true;
            }
        }

        ~ValidatingDataReader()
        {
            Dispose(false);
        }

        #region IDataReader Members
        public int RecordsAffected
        {
            get
            {
                return wrappedReader.RecordsAffected;
            }
        }

        public bool IsClosed
        {
            get
            {
                return disposed;
            }
        }

        public bool NextResult()
        {
            return wrappedReader.NextResult();
        }

        public void Close()
        {
            (this as IDisposable).Dispose();
        }

        public bool Read()
        {
            bool canRead = wrappedReader.Read();

            if (canRead)
            {
                currentRecord++;
            }

            return canRead;
        }

        public int Depth
        {
            get
            {
                return wrappedReader.Depth;
            }
        }

        public DataTable GetSchemaTable()
        {
            return wrappedReader.GetSchemaTable();
        }
        #endregion

        #region IDataRecord Members
        public int GetInt32(int i)
        {
            return wrappedReader.GetInt32(i);
        }

        public object this[string name]
        {
            get
            {
                int ordinal = wrappedReader.GetOrdinal(name);

                if (ordinal > -1)
                {
                    return (this as IDataRecord).GetValue(ordinal);
                }
                else
                {
                    return wrappedReader[name];
                }
            }
        }

        public object this[int i]
        {
            get
            {
                return (this as IDataRecord).GetValue(i);
            }
        }

        public object GetValue(int i)
        {
            object columnValue = wrappedReader.GetValue(i);

            if (i > -1 && i < lookup.Length)
            {
                DataRow columnDef = lookup[i];
                if
                (
                    (
                        (string)columnDef["DataTypeName"] == "varchar" ||
                        (string)columnDef["DataTypeName"] == "nvarchar" ||
                        (string)columnDef["DataTypeName"] == "char" ||
                        (string)columnDef["DataTypeName"] == "nchar"
                    ) &&
                    (
                        columnValue != null &&
                        columnValue != DBNull.Value
                    )
                )
                {
                    string stringValue = columnValue.ToString();

                    int colSize = (int)columnDef["ColumnSize"];
                    if (stringValue.Length > colSize)
                    {
                        if (TrimLongStringsInsteadOfThrowing)
                        {
                            return stringValue.Substring(0, colSize);
                        }

                        string message =
                            "Column value \"" + stringValue.Replace("\"", "\\\"") + "\"" +
                            " with length " + stringValue.Length.ToString("###,##0", CultureInfo.InvariantCulture) +
                            " from source column " + (this as IDataRecord).GetName(i) +
                            " in record " + currentRecord.ToString("###,##0", CultureInfo.InvariantCulture) +
                            " does not fit in destination column " + columnDef["ColumnName"] +
                            " with length " + ((int)columnDef["ColumnSize"]).ToString("###,##0", CultureInfo.InvariantCulture) +
                            " in table " + targetTableName +
                            " in database " + targetDatabaseName +
                            " on server " + targetServerName + ".";

                        throw new SchemaException(message);
                    }
                }
            }

            return columnValue;
        }

        public bool IsDBNull(int i)
        {
            return wrappedReader.IsDBNull(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return wrappedReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public byte GetByte(int i)
        {
            return wrappedReader.GetByte(i);
        }

        public Type GetFieldType(int i)
        {
            return wrappedReader.GetFieldType(i);
        }

        public decimal GetDecimal(int i)
        {
            return wrappedReader.GetDecimal(i);
        }

        public int GetValues(object[] values)
        {
            return wrappedReader.GetValues(values);
        }

        public string GetName(int i)
        {
            return wrappedReader.GetName(i);
        }

        public int FieldCount
        {
            get
            {
                return wrappedReader.FieldCount;
            }
        }

        public long GetInt64(int i)
        {
            return wrappedReader.GetInt64(i);
        }

        public double GetDouble(int i)
        {
            return wrappedReader.GetDouble(i);
        }

        public bool GetBoolean(int i)
        {
            return wrappedReader.GetBoolean(i);
        }

        public Guid GetGuid(int i)
        {
            return wrappedReader.GetGuid(i);
        }

        public DateTime GetDateTime(int i)
        {
            return wrappedReader.GetDateTime(i);
        }

        public int GetOrdinal(string name)
        {
            return wrappedReader.GetOrdinal(name);
        }

        public string GetDataTypeName(int i)
        {
            return wrappedReader.GetDataTypeName(i);
        }

        public float GetFloat(int i)
        {
            return wrappedReader.GetFloat(i);
        }

        public IDataReader GetData(int i)
        {
            return wrappedReader.GetData(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return wrappedReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public string GetString(int i)
        {
            return (string)(this as IDataRecord).GetValue(i);
        }

        public char GetChar(int i)
        {
            return wrappedReader.GetChar(i);
        }

        public short GetInt16(int i)
        {
            return wrappedReader.GetInt16(i);
        }
        #endregion
    }
}
