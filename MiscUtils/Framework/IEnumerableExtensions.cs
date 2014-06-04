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

namespace MiscUtils.Framework
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Applies an action to every element in an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of object in the sequence.</typeparam>
        /// <param name="collection">The sequence to operate on.</param>
        /// <param name="action">The action to apply.</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.ThrowIfNull("sequence");
            action.ThrowIfNull("action");

            foreach (T element in collection)
            {
                action(element);
            }
        }

        public static bool IsSorted<TElement>(this IEnumerable<TElement> collection)
        where TElement : System.IComparable<TElement>
        {
            return IsSorted(collection, (a) => a);
        }

        public static bool IsStrictlySorted<TElement>(this IEnumerable<TElement> collection)
        where TElement : System.IComparable<TElement>
        {
            return IsStrictlySorted(collection, (a) => a);
        }

        public static bool IsSorted<TElement, TKey>(this IEnumerable<TElement> collection, Func<TElement, TKey> keyExtractor)
        where TKey : System.IComparable<TKey>
        {
            return IsSortedImpl
                (
                collection,
                keyExtractor,
                (a, b) => a.CompareTo(b) <= 0
                );
        }

        public static bool IsStrictlySorted<TElement, TKey>(this IEnumerable<TElement> collection, Func<TElement, TKey> keyExtractor)
        where TKey : System.IComparable<TKey>
        {
            return IsSortedImpl
                (
                collection,
                keyExtractor,
                (a, b) => a.CompareTo(b) < 0
                );
        }

        static bool IsSortedImpl<TElement, TKey>
            (
            IEnumerable<TElement> collection,
            Func<TElement, TKey> keyExtractor,
            Func<TKey, TKey, bool> comparison
            )
        where TKey : System.IComparable<TKey>
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
                    if (!comparison(keyExtractor(prevElement), keyExtractor(element)))
                        return false;
                }

                prevElement = element;
            }

            return true;
        }
    }
}
