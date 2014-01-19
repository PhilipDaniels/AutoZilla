using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZilla.Core.Extensions
{
    /// <summary>
    /// Handy class for enumerating over the values of an enum, because
    /// Enum.GetValues() does not return what you think it does...
    /// </summary>
    /// <example>
    /// foreach (var x in Enum&lt;Alignment&gt;.GetValues()
    /// </example>
    /// <typeparam name="T">An enum type.</typeparam>
    public static class Enum<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        public static IEnumerable<T> GetValues()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static IEnumerable<string> GetNames()
        {
            return Enum.GetNames(typeof(T));
        }
    }
}
