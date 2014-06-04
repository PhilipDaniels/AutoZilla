using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

// http://stackoverflow.com/questions/35007/how-to-expose-a-collection-property

namespace MiscUtils.Framework
{
    /// <summary>
    /// A collection class with very limited operations, which are guaranteed
    /// to be thread-safe. This class can therefore be used to expose mutable
    /// static collections.
    /// </summary>
    public class MutableThreadSafeCollection<T>
    {
        object _Lock = new object();

        readonly List<T> Items = new List<T>();

        public void Add(T item)
        {
            lock (_Lock)
            {
                Items.Add(item);
            }
        }

        public void Remove(T item)
        {
            lock (_Lock)
            {
                Items.Remove(item);
            }
        }

        public ReadOnlyCollection<T> Values
        {
            get
            {
                lock (_Lock)
                {
                    return Items.AsReadOnly();
                }
            }
        }

        public bool Contains(T item)
        {
            lock (_Lock)
            {
                return Items.Contains(item);
            }
        }

        public bool Contains(T item, IEqualityComparer<T> comparer)
        {
            lock (_Lock)
            {
                return Items.Contains(item, comparer);
            }
        }

        public void Clear()
        {
            lock (_Lock)
            {
                Items.Clear();
            }
        }
    }
}
