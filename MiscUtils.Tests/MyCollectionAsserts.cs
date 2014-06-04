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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscUtils.Tests
{
    class MyCollectionAsserts
    {
        public static void IsSorted<TElement>(IEnumerable<TElement> collection) where TElement : System.IComparable<TElement>
        {
            IsSorted(collection, (a) => a);
        }

        public static void IsStrictlySorted<TElement>(IEnumerable<TElement> collection) where TElement : System.IComparable<TElement>
        {
            IsStrictlySorted(collection, (a) => a);
        }

        public static void IsSorted<TElement, TKey>(IEnumerable<TElement> collection, Func<TElement, TKey> keyExtractor) where TKey : System.IComparable<TKey>
        {
            IsSortedImpl
                (
                collection,
                keyExtractor,
                (a, b) => a.CompareTo(b) <= 0
                );
        }

        public static void IsStrictlySorted<TElement, TKey>(IEnumerable<TElement> collection, Func<TElement, TKey> keyExtractor) where TKey : System.IComparable<TKey>
        {
            IsSortedImpl
                (
                collection,
                keyExtractor,
                (a, b) => a.CompareTo(b) < 0
                );
        }

        private static void IsSortedImpl<TElement, TKey>
            (
            IEnumerable<TElement> collection,
            Func<TElement, TKey> keyExtractor,
            Func<TKey, TKey, bool> comparison
            ) where TKey : System.IComparable<TKey>
        {
            bool lookingAtFirst = true;
            TElement prevElement = default(TElement);

            foreach (var element in collection)
            {
                if (lookingAtFirst)
                {
                    lookingAtFirst = false;
                }
                else
                {
                    Assert.True(comparison(keyExtractor(prevElement), keyExtractor(element)));
                }

                prevElement = element;
            }
        }
    }
}
