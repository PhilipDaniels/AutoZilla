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
using System.Globalization;
using System.Linq;

namespace MiscUtils.Framework
{
    /// <summary>
    /// A collection which always yields its items in a sorted order.
    /// A delegate (key extraction function) is used to specify the sort key.
    /// </summary>
    [DebuggerDisplay("{Count} items")]
    public class SortedCollection<TElement, TKey> : IEnumerable<TElement>
    where TKey : IEquatable<TKey>
    {
        readonly List<TElement> items;
        readonly Func<TElement, TKey> keyExtractor;
        readonly bool allowDuplicates;

        /// <summary>
        /// Initialise a new SortedCollection.
        /// </summary>
        /// <param name="keyExtractor">The delegate to be used to sort the objects.</param>
        /// <param name="allowDuplicates">If true, elements with the same key are
        /// allowed into the collection. If false, an exception will be thrown
        /// when attempting to add a duplicate key.</param>
        public SortedCollection
            (
            Func<TElement, TKey> keyExtractor,
            bool allowDuplicates
            )
        {
            keyExtractor.ThrowIfNull("keyExtractor");

            items = new List<TElement>();
            this.keyExtractor = keyExtractor;
            this.allowDuplicates = allowDuplicates;
        }

        public IEnumerable<TElement> Values
        {
            get
            {
                return items.OrderBy(keyExtractor);
            }
        }

        public void Add(TElement item)
        {
            item.ThrowIfNull("item");

            if (!allowDuplicates)
            {
                var key = keyExtractor(item);
                var index = items.FindIndex(e => keyExtractor(e).Equals(key));
                if (index != -1)
                {
                    var msg = String.Format(CultureInfo.InvariantCulture, "The object with key '{0}' already exists in the collection.", key);
                    throw new ArgumentException(msg);
                }
            }

            items.Add(item);
        }

        public void AddRange(IEnumerable<TElement> items)
        {
            if (items != null)
            {
                // So that we get the AllowDuplicates logic.
                foreach (var item in items)
                {
                    Add(item);
                }
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return items.Count; }
        }
    }
}