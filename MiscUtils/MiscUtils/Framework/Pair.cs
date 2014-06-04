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
using System.Collections.Generic;
using System.Diagnostics;

namespace MiscUtils.Framework
{
    /// <summary>
    /// A generic pair class in the style of STL.
    /// </summary>
    /// <typeparam name="TFirst">Type of the first generic parameter.</typeparam>
    /// <typeparam name="TSecond">Type of the second generic parameter.</typeparam>
    [DebuggerDisplay("({First},{Second})")]
    [Serializable]
    public sealed class Pair<TFirst, TSecond>
        : IEquatable<Pair<TFirst, TSecond>>
    {
        /// <summary>
        /// The first element of the pair.
        /// </summary>
        public TFirst First { get { return _First; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly TFirst _First;

        /// <summary>
        /// The second element of the pair.
        /// </summary>
        public TSecond Second { get { return _Second; } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly TSecond _Second;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;TFirst, TSecond&gt;" /> class.	
        /// </summary>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        public Pair(TFirst first, TSecond second)
        {
            _First = first;
            _Second = second;
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>True if the two pairs are equal.</returns>
        public bool Equals(Pair<TFirst, TSecond> other)
        {
            if (other == null)
                return false;

            return EqualityComparer<TFirst>.Default.Equals(_First, other._First) &&
                   EqualityComparer<TSecond>.Default.Equals(_Second, other._Second);
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="obj">The other.</param>
        /// <returns>True if the two pairs are equal.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Pair<TFirst, TSecond>);
        }

        /// <summary>
        /// Gets the hash code.	
        /// </summary>
        /// <returns>The hash code of the pair.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<TFirst>.Default.GetHashCode(_First) * 37 +
                   EqualityComparer<TSecond>.Default.GetHashCode(_Second);
        }
    }
}
