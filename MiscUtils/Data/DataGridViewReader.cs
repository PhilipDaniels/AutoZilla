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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MiscUtils.Data
{
    /*
    /// <summary>
    /// Implements an IDataReader interface over a DataGridView.
    /// The grid is assumed to contain a simple rectangle of data.
    /// </summary>
    public class DataGridViewReader : IDataReader
    {
        DataGridView Grid;
        TableMetaData TableSchema;
        Dictionary<string, int> NamesToOrdinalsMap;

        public DataGridViewReader(DataGridView grid, TableMetaData tableSchema)
        {
            grid.ThrowIfNull("grid");
            tableSchema.ThrowIfNull("tableSchema");

            Grid = grid;
            TableSchema = tableSchema;

            NamesToOrdinalsMap = new Dictionary<string, int>();
            for (var i = 0; i < TableSchema.Columns.Count; i++)
            {
                NamesToOrdinalsMap.Add(TableSchema.Columns[i].Name, i);
            }
        }

        void ErrorIfClosed()
        {
            if (IsClosed)
                throw new InvalidOperationException("Attempt to access DataGridViewReader which was already closed.");
        }

        public void Close()
        {
            IsClosed = true;
        }

        public int Depth
        {
            get
            {
                ErrorIfClosed();
                return 1;
            }
        }

        public DataTable GetSchemaTable()
        {
            // TODO: We want this to work.
            // We need to make it the inverse of creating a TableDefn from an IDataReader.
            throw new NotImplementedException();
        }

        public bool IsClosed { get; private set; }

        public bool NextResult()
        {
            ErrorIfClosed();
            return false;
        }

        int currentRowIndex = -1;
        DataGridViewCellCollection currentCells = null;
        public bool Read()
        {
            ErrorIfClosed();
            if (ReachedTheEnd())
                return false;
            ReadNextRow();
            return true;
        }

        bool ReachedTheEnd()
        {
            return Grid.Rows.Count == 0 || currentRowIndex == Grid.Rows.Count - 1;
        }

        void ReadNextRow()
        {
            currentRowIndex++;
            currentCells = Grid.Rows[currentRowIndex].Cells;
        }

        public int RecordsAffected
        {
            get { return 0; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed = false;
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Grid != null)
                    {
                        Grid.Dispose();
                        Grid = null;
                    }
                }

                disposed = true;
            }
        }

        ~DataGridViewReader()
        {
            Dispose(false);
        }

        public int FieldCount
        {
            get
            {
                ErrorIfClosed();
                return Grid.Columns.Count;
            }
        }

        public bool GetBoolean(int i)
        {
            ErrorIfClosed();
            return (bool)currentCells[i].Value;
        }

        public byte GetByte(int i)
        {
            ErrorIfClosed();
            return (byte)currentCells[i].Value;
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            ErrorIfClosed();
            var sourceBuffer = (byte[])currentCells[i].Value;
            length = Math.Min(length, sourceBuffer.Length - (int)fieldOffset);
            Array.Copy(sourceBuffer, fieldOffset, buffer, bufferoffset, length);
            return length;
        }

        public char GetChar(int i)
        {
            ErrorIfClosed();
            return (char)currentCells[i].Value;
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            ErrorIfClosed();
            var sourceBuffer = (char[])currentCells[i].Value;
            length = Math.Min(length, sourceBuffer.Length - (int)fieldoffset);
            Array.Copy(sourceBuffer, fieldoffset, buffer, bufferoffset, length);
            return length;
        }

        public IDataReader GetData(int i)
        {
            // No idea what to do here.
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            ErrorIfClosed();
            return TableSchema.Columns[i].DataType.Name;
        }

        public DateTime GetDateTime(int i)
        {
            ErrorIfClosed();
            return (DateTime)currentCells[i].Value;
        }

        public decimal GetDecimal(int i)
        {
            ErrorIfClosed();
            return (decimal)currentCells[i].Value;
        }

        public double GetDouble(int i)
        {
            ErrorIfClosed();
            return (double)currentCells[i].Value;
        }

        public Type GetFieldType(int i)
        {
            ErrorIfClosed();
            return TableSchema.Columns[i].DataType;
        }

        public float GetFloat(int i)
        {
            ErrorIfClosed();
            return (float)currentCells[i].Value;
        }

        public Guid GetGuid(int i)
        {
            ErrorIfClosed();
            return (Guid)currentCells[i].Value;
        }

        public short GetInt16(int i)
        {
            ErrorIfClosed();
            return (short)currentCells[i].Value;
        }

        public int GetInt32(int i)
        {
            ErrorIfClosed();
            return (int)currentCells[i].Value;
        }

        public long GetInt64(int i)
        {
            ErrorIfClosed();
            return (long)currentCells[i].Value;
        }

        public string GetName(int i)
        {
            ErrorIfClosed();
            return TableSchema.Columns[i].Name;
        }

        public int GetOrdinal(string name)
        {
            ErrorIfClosed();
            return NamesToOrdinalsMap[name];
        }

        public string GetString(int i)
        {
            ErrorIfClosed();
            return (string)currentCells[i].Value;
        }

        public object GetValue(int i)
        {
            ErrorIfClosed();
            return currentCells[i].Value;
        }

        public int GetValues(object[] values)
        {
            ErrorIfClosed();
            var length = Math.Min(values.Length, currentCells.Count);
            for (var i = 0; i < length; i++)
                values[i] = currentCells[i];

            return length;
        }

        public bool IsDBNull(int i)
        {
            ErrorIfClosed();
            return Convert.IsDBNull(currentCells[i].Value);
        }

        public object this[string name]
        {
            get
            {
                ErrorIfClosed();
                return GetValue(GetOrdinal(name));
            }
        }

        public object this[int i]
        {
            get
            {
                ErrorIfClosed();
                return GetValue(i);
            }
        }
    }
     */
}
