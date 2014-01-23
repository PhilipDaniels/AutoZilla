using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZilla.Core.Validation
{
    public static class ParameterValidators
    {
        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string paramName)
        {
            return parameter.ThrowIfNull(paramName, null);
        }

        public static T ThrowIfNull<T>([ValidatedNotNull] this T parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);

            return parameter;
        }



        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string paramName)
        {
            return parameter.ThrowIfNullOrEmpty(paramName, null);
        }

        public static string ThrowIfNullOrEmpty([ValidatedNotNull] this string parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);
            if (parameter.Length == 0)
                throw new ArgumentOutOfRangeException(paramName, message);

            return parameter;
        }



        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string paramName)
        {
            return parameter.ThrowIfNullOrWhiteSpace(paramName, null);
        }

        public static string ThrowIfNullOrWhiteSpace([ValidatedNotNull] this string parameter, string paramName, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(paramName, message);
            if (parameter.Trim().Length == 0)
                throw new ArgumentOutOfRangeException(paramName, message);

            return parameter;
        }




        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThan(value, paramName, null);
        }

        public static T ThrowIfLessThan<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) < 0)
            {
                string msg = String.Format("Parameter {0} cannot be less than {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
            }

            return parameter;
        }



        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfLessThanOrEqualTo(value, paramName, null);
        }

        public static T ThrowIfLessThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) <= 0)
            {
                string msg = String.Format("Parameter {0} cannot be less than or equal to {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return parameter;
        }


        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThan(value, paramName, null);
        }

        public static T ThrowIfMoreThan<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
            where T : IComparable<T>
        {
            if (parameter.CompareTo(value) > 0)
            {
                string msg = String.Format("Parameter {0} cannot be more than {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
            }

            return parameter;
        }


        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName)
            where T : IComparable<T>
        {
            return parameter.ThrowIfMoreThanOrEqualTo(value, paramName, null);
        }

        public static T ThrowIfMoreThanOrEqualTo<T>([ValidatedNotNull] this T parameter, T value, string paramName, string message)
        where T : IComparable<T>
        {
            if (parameter.CompareTo(value) >= 0)
            {
                string msg = String.Format("Parameter {0} cannot be more than or equal to {1} but {2} supplied.", paramName, value, parameter);
                if (message != null)
                    msg += " " + message;
                throw new ArgumentOutOfRangeException(paramName, msg);
            }

            return parameter;
        }
    }
}
