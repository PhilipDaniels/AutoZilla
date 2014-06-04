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
using System.Runtime.Serialization;

namespace MiscUtils.Framework
{
    /// <summary>
    /// This is not a functional class. It simply demonstrates the canonical form
    /// of an application-specific exception. Copy and paste this class for your
    /// new exception types and rename. Code Analysis will then shut up.
    /// </summary>
    [Serializable]
    public class CanonicalException : Exception
    {
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public CanonicalException()
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public CanonicalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="innerException">Inner exception.</param>
        public CanonicalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected CanonicalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
