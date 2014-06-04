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
using System.Globalization;

namespace MiscUtils.Framework
{
    public static class DateTimeExtensions
    {
        const string ISO_8601_FORMAT = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Convert a DateTime to a database formated string.
        /// The string is in ISO 8601 format and is safe to use whatever the DATEFORMAT
        /// or LANGUAGE settings are in your app or database.
        /// See http://www.karaszi.com/sqlserver/info_datetime.asp#RecommendationsInput
        /// </summary>
        /// <param name="dt">The DateTime to format.</param>
        /// <returns>Formatted string.</returns>
        public static string ToMSSQLString(this DateTime dt)
        {
            dt.ThrowIfNull("dt");
            
            string result = String.Format(CultureInfo.InvariantCulture, "{0:" + ISO_8601_FORMAT + "}", dt);
            return result;
        }

        /// <summary>
        /// Converts an ISO 8601 format date (with the T in the middle) into a datetime object.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>Converted DateTime object.</returns>
        public static DateTime FromMSSQLString(string s)
        {
            s.ThrowIfNullOrWhiteSpace("s");

            return DateTime.ParseExact(s, ISO_8601_FORMAT, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Checks to see whether a datetime object has a time component.
        /// </summary>
        /// <param name="dt">The datetime to check.</param>
        /// <returns>True if there is a time component, false otherwise.</returns>
        public static bool HasTimeComponent(this DateTime dt)
        {
            dt.ThrowIfNull("dt");

            return dt.TimeOfDay != TimeSpan.Zero;
        }

        /// <summary>
        /// Returns the start of the specified day.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The start of the day.</returns>
        public static DateTime StartOfDay(this DateTime dt)
        {
            dt.ThrowIfNull("dt");

            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        /// <summary>
        /// Returns the end of the specified day (to SQL server accuracy).
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>End of the day.</returns>
        public static DateTime EndOfDay(this DateTime dt)
        {
            dt.ThrowIfNull("dt");

            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 997);
        }

        /// <summary>
        /// Returns the start of the year for a datetime.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The start of the year.</returns>
        public static DateTime StartOfYear(this DateTime dt)
        {
            dt.ThrowIfNull("dt");

            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        /// Returns the end of the year for a datetime (to SQL server accuracy).
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns>The end of the year.</returns>
        public static DateTime EndOfYear(this DateTime dt)
        {
            dt.ThrowIfNull("dt");

            dt = StartOfYear(dt).AddYears(1).AddMilliseconds(-3);
            return dt;
        }
    }
}
