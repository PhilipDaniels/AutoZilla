using System;
using System.Collections.Generic;

namespace AutoZilla.Core.Extensions
{
    /// <summary>
    /// Handy class for enumerating over the values of an enum, because
    /// Enum.GetValues() does not return what you think it does...
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Gets all the values of an enumeration.
        /// </summary>
        /// <returns>All the values of an enumeration.</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// Gets all the names of an enumeration.
        /// </summary>
        /// <returns>All the names of an enumeration.</returns>
        public static IEnumerable<string> GetNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }
    }
}
