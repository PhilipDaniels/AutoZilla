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

using System;
using System.Diagnostics;

namespace MiscUtils.Framework
{
    /// <summary>
    /// Provides supporting infrastructure for implementing the IDisposable interface.
    /// Just inherit this class, then override the Cleanup() method.
    /// http://blogs.msdn.com/b/bclteam/archive/2007/10/30/dispose-pattern-and-object-lifetime-brian-grunkemeyer.aspx
    /// </summary>
    public class DisposableBaseType : IDisposable
    {
        /// <summary>
        /// Returns true if the disposed flag is true, i.e. Dispose()
        /// has been called.
        /// </summary>
        protected bool Disposed
        {
            get
            {
                lock (this)
                {
                    return _disposed;
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _disposed;

        #region IDisposable Members
        /// <summary>
        /// Disposes the object by calling the virtual Cleanup()
        /// methods then setting the Disposed flag to true.
        /// </summary>
        public void Dispose()
        {
            lock (this)
            {
                if (_disposed == false)
                {
                    Cleanup();
                    _disposed = true;

                    GC.SuppressFinalize(this);
                }
            }
        }
        #endregion

        /// <summary>
        /// Override this method to provide your own cleanup code,
        /// e.g. disposing of resources.
        /// </summary>
        protected virtual void Cleanup()
        {
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBaseType"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBaseType()
        {
            Cleanup();
        }
    }
}
