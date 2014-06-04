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
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace MiscUtils.Data
{
    /// <summary>
    /// Transaction support for generated table adapters.
    /// </summary>
    /// <remarks>
    /// This class adds transaction support to table adapters. It is used by changing the base
    /// class of a table adapter from Component to this class. The implementation of this class
    /// then accesses the derived table adapter's properties through reflection.
    /// You should derive a project specific version from this base class.
    /// </remarks>
    public abstract class TableAdapterBase : System.ComponentModel.Component
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SqlTransaction transaction;

        /// <summary>
        /// Transaction of this table adapter.
        /// </summary>
        /// <remarks>
        /// This property is used to share a transaction and its associated connection
        /// across multiple table adapters. The typical pattern is shown in the sample
        /// code below.
        /// </remarks>
        /// <example>
        /// XTableAdapter xta = new XTableAdapter();
        /// YTableAdapter yta = new YTableAdapter();
        /// 
        /// xta.BeginTransation();
        /// yta.Transation = xta.Transaction;
        /// try
        /// {
        ///     // perform xta and yta modifications here.
        ///     xta.CommitTransaction();
        /// }
        /// catch( Exception )
        /// {
        ///     xta.RollbackTransaction();
        /// }
        /// </example>
        public SqlTransaction Transaction
        {
            get
            {
                return transaction;
            }
            set
            {
                // attach transaction to all commands of this adapter:
                if (CommandCollection != null)
                {
                    foreach (SqlCommand command in CommandCollection)
                    {
                        command.Transaction = value;
                    }
                }
                if (Adapter.InsertCommand != null)
                {
                    Adapter.InsertCommand.Transaction = value;
                }
                if (Adapter.UpdateCommand != null)
                {
                    Adapter.UpdateCommand.Transaction = value;
                }
                if (Adapter.DeleteCommand != null)
                {
                    Adapter.DeleteCommand.Transaction = value;
                }

                // also set connection of this adapter accordingly.
                // *** Be careful to dispose of any existing connection. ***
                if (value != null)
                {
                    SqlConnection = value.Connection;
                }
                else
                {
                    // only clear connection if it was attached to
                    // transaction before:
                    if (transaction != null)
                        SqlConnection = null;
                }

                transaction = value;
            }
        }

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            // Create the transaction and assign it to the Transaction property
            Transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void CommitTransaction()
        {
            Transaction.Commit();
            Connection.Close();
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            Transaction.Rollback();
            Connection.Close();
        }

        /// <summary>
        /// Sets the timeout (in seconds) on all the commands in the adapter.
        /// Can be useful to change this for long running commands.
        /// A timeout of 0 means "wait forever".
        /// <remarks>
        /// This is NOT the connection timeout, which is set in the connection
        /// string using the "Connect Timeout=xx" property, default for that
        /// is 30 seconds.
        /// </remarks>
        /// </summary>
        /// <param name="timeout">Command Timeout, in seconds.</param>
        public void SetTimeout(int timeout)
        {
            timeout.ThrowIfLessThan(0, "timeout", "timeout must be 0 (which means wait forever) or a positive number.");

            foreach (IDbCommand cmd in CommandCollection)
            {
                cmd.CommandTimeout = timeout;
            }
        }

        #region Reflective access to table adapter properties

        /// <summary>
        /// Gets or sets the current SqlConnection.
        /// </summary>
        public SqlConnection SqlConnection
        {
            get
            {
                return Connection;
            }
            set
            {
                if (Connection != null && Connection != value)
                {
                    Connection.Close();
                    Connection.Dispose();
                    Connection = null;
                }

                Connection = value;
            }
        }

        /// <summary>
        /// Proxies the normally private Connection property.
        /// </summary>
        SqlConnection Connection
        {
            // Access to properties as public and non-public as generated table-adapter
            // scope seems to be different for different installations:
            // http://www.codeproject.com/useritems/transactionta.asp?msg=2225021#xx2225021xx
            get
            {
                return (SqlConnection)GetType().GetProperty
                    (
                    "Connection",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                    ).GetValue(this, null);
            }
            set
            {
                GetType().GetProperty
                    (
                    "Connection",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
                    ).SetValue(this, value, null);
            }
        }

        /// <summary>
        /// Exposes the normally private Adapter property.
        /// </summary>
        SqlDataAdapter Adapter
        {
            get
            {
                return (SqlDataAdapter)GetType().GetProperty
                    (
                    "Adapter",
                    BindingFlags.NonPublic | BindingFlags.Instance
                    ).GetValue(this, null);
            }
        }

        /// <summary>
        /// Exposes the normally private CommandCollection.
        /// </summary>
        SqlCommand[] CommandCollection
        {
            get
            {
                return (SqlCommand[])GetType().GetProperty
                    (
                    "CommandCollection",
                    BindingFlags.NonPublic | BindingFlags.Instance
                    ).GetValue(this, null);
            }
        }
        #endregion

        #region IDisposable Implementation (for derived classes)
        /// <summary>
        /// VERY IMPORTANT - must dispose of connections to return them to the pool.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }

                GC.SuppressFinalize(this);
            }

            base.Dispose(disposing);
            _disposed = true;
        }
        private bool _disposed;
        #endregion
    }
}
