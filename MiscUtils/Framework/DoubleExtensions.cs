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

namespace MiscUtils.Framework
{
    public static class DoubleExtensions
    {
        public static readonly double DefaultMaxRelativeError = 0.000000001;

        /// <summary>
        /// Check to see whether 2 double are equal, using an absolute error factor.
        /// Use this method if you think "equality" means within 0.00001 (or some
        /// other maxAbsoluteError). This method is not recommended because it takes no acount
        /// of the sizes of the numbers. For example, for 2 very large numbers they may
        /// well differ by much more than maxAbsoluteError, yet be the same to within
        /// 99.99999%. For this reason, you should strongly consider using AlmostEqualRelative
        /// instead. This overload of AlmostEqualAbsolute calls the other overload
        /// passing a maxAbsoluteError of double.Epsilon.
        /// </summary>
        /// <param name="d1">First double.</param>
        /// <param name="d2">Second double.</param>
        /// <returns>True if d1 and d2 are equal, false otherwise.</returns>
        public static bool AlmostEqualAbsolute(this double d1, double d2)
        {
            return AlmostEqualAbsolute(d1, d2, double.Epsilon);
        }

        /// <summary>
        /// Check to see whether 2 double are equal, using an absolute error factor.
        /// Use this method if you think "equality" means within 0.00001 (or some
        /// other maxAbsoluteError). This method is not recommended because it takes no acount
        /// of the sizes of the numbers. For example, for 2 very large numbers they may
        /// well differ by much more than absoluteError, yet be the same to within
        /// 99.99999%. For this reason, you should strongly consider using AlmostEqualRelative
        /// instead.
        /// </summary>
        /// <param name="d1">First double.</param>
        /// <param name="d2">Second double.</param>
        /// <param name="maxAbsoluteError">Maximum absolute difference between d1 and d2
        /// for the two numbers to still be equal.</param>
        /// <returns>True if d1 and d2 are equal, false otherwise.</returns>
        public static bool AlmostEqualAbsolute(this double d1, double d2, double maxAbsoluteError)
        {
            return Math.Abs(d1 - d2) < maxAbsoluteError;
        }

        /// <summary>
        /// Check to see whether 2 doubles are equal, using a relative error factor.
        /// This function is preferred over AlmostEqualAbsolute because it takes into
        /// account differences in number size. The maxRelativeError parameter specifies
        /// the error tolerance for the match. This overload calls the other
        /// overload passing in a maxRelativeError of Reals.DefaultMaxRelativeError
        /// See the documentation for the Reals.RelativeError function
        /// for examples of relative errors for various combinations of d1 and d2.
        /// </summary>
        /// <param name="d1">First double.</param>
        /// <param name="d2">Second double.</param>
        /// <returns>True if d1 and d2 are equal, false otherwise.</returns>
        public static bool AlmostEqualRelative(this double d1, double d2)
        {
            return AlmostEqualRelative(d1, d2, DefaultMaxRelativeError);
        }

        /// <summary>
        /// Check to see whether 2 doubles are equal, using a relative error factor.
        /// This function is preferred over AlmostEqualAbsolute because it takes into
        /// account differences in number size. The maxRelativeError parameter specifies
        /// the error tolerance for the match. See the documentation for the
        /// Reals.RelativeError function for examples of relative errors for various
        /// combinations of d1 and d2.
        /// </summary>
        /// <param name="d1">First double.</param>
        /// <param name="d2">Second double.</param>
        /// <param name="maxRelativeError">Size of the relative error factor. Typically 0.000000001
        /// or smaller.</param>
        /// <returns>True if d1 and d2 are equal, false otherwise.</returns>
        public static bool AlmostEqualRelative(this double d1, double d2, double maxRelativeError)
        {
            // Note: this is actually AlmostEqualRelativeOrAbsolute()
            // with a hardcoded parameter for maxAbsoluteError.
            if (Math.Abs(d1 - d2) < double.Epsilon)
                return true;

            double relativeError = RelativeError(d1, d2);

            return relativeError <= maxRelativeError;
        }

        /// <summary>
        /// Calculate the relative error between two doubles.
        /// Examples of relative errors (as returned by Reals.RelativeError():
        /// <code>
        ///  d1      d2      RE
        ///  ======= ======= =============
        ///  1       100     0.99
        ///  50      100     0.5
        ///  90      100     0.1
        ///  99      100     0.01
        ///  100     100     0
        ///  101     100     0.00990990099
        ///  110     100     0.0909090909
        ///  150     100     0.3333333333
        ///  10,000  100     0.99
        ///
        ///  99      100     0.01
        ///  999     1000    0.001
        ///  9999    10000   0.0001
        ///  99999   100000  0.00001   
        ///  999999  1000000 0.000001
        /// </code>
        /// </summary>
        /// <param name="d1">The first double.</param>
        /// <param name="d2">The second double.</param>
        /// <returns>Relative error.</returns>
        public static double RelativeError(this double d1, double d2)
        {
            double relativeError;

            if (Math.Abs(d2) > Math.Abs(d1))
            {
                relativeError = Math.Abs((d1 - d2) / d2);
            }
            else
            {
                relativeError = Math.Abs((d1 - d2) / d1);
            }

            return relativeError;
        }
    }
}
