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

using MiscUtils.Framework;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MiscUtils.Data
{
	/*
	 * We are aiming for a family of methods:
	 * 
	 * public static Boolean GetBoolean(int i);                                 // built-in to IDataRecord
	 * public static Boolean GetBoolean(string name);                           // extension
	 * public static Boolean GetBoolean(int i, bool defaultIfNull);             // extension
	 * public static Boolean GetBoolean(string name, bool defaultIfNull);       // extension
	 * public static Boolean? GetNullableBoolean(int i);                        // extension
	 * public static Boolean? GetNullableBoolean(string name);                  // extension
	 */
    public static class IDataRecordExtensions
    {
		/// <summary>
		/// Attempt to create an object of type T and initialise it using an IDataRecord.
		/// The method looks for an instance constructor that takes an IDataRecord. If none
		/// is found it looks for a static method on the class that takes an IDataRecord and
		/// returns an object of type T, and assumes that this is a factory method.
		/// If this lookup fails, an exception is thrown.
		/// </summary>
		/// <typeparam name="T">The type of object to create.</typeparam>
		/// <param name="record">The data record.</param>
		/// <returns>A new object of type T.</returns>
        public static T Hydrate<T>(this IDataRecord record)
		{
			record.ThrowIfNull("record");

			Type theType = typeof(T);

			// Look for an instance constructor on T that takes just an IDataRecord.
			var instanceConstructor = (from ci in theType.GetConstructors()
									   where ci.GetParameters().Count() == 1
										 && ci.GetParameters()[0].ParameterType == typeof(IDataRecord)
									   select ci).FirstOrDefault();

			if (instanceConstructor != null)
			{
				return (T)instanceConstructor.Invoke(new object[] { record });
			}


			// Look for a static factory method on T that takes just an IDataRecord
			// and returns an instance of T.
			var factoryMethod = (from fm in theType.GetMethods(BindingFlags.Static | BindingFlags.Public)
								 where fm.GetParameters().Count() == 1
									&& fm.GetParameters()[0].ParameterType == typeof(IDataRecord)
									&& fm.ReturnType.IsAssignableFrom(theType)
								 select fm).FirstOrDefault();

			if (factoryMethod != null)
			{
				return (T)factoryMethod.Invoke(null, new object[] { record });
			}

			// TODO: Look for an instance constructor that takes no params
			// and then initialise the public settable properties and fields.

			throw new InvalidConstraintException("Could not find an instance constructor or static factory method that takes just an IDataRecord.");
		}

		/// <summary>
		/// Create a new object of type T using the specified delegate.
		/// </summary>
		/// <typeparam name="T">The type of object to create.</typeparam>
		/// <param name="record">The data record.</param>
		/// <param name="hydrationFunction">A delegate that can construct a new object of type T
		/// and initialise it from a data record.</param>
		/// <returns>A new object of type T.</returns>
		public static T Hydrate<T>(this IDataRecord record, Func<IDataRecord, T> hydrationFunction)
		{
			record.ThrowIfNull("record");
            hydrationFunction.ThrowIfNull("hydrationFunction");

			return hydrationFunction(record);
		}

		/// <summary>
		/// Return whether the specified field is set to null.
		/// </summary>
		/// <param name="record">The data record.</param>
		/// <param name="name">The name of the column to find.</param>
		/// <returns>True if the field is DbNull, false otherwise.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
		public static Boolean IsDbNull(this IDataRecord record, string name)
		{
			return record.IsDBNull(record.GetOrdinal(name));
		}

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBoolean(this IDataRecord record, string name)
        {
            return record.GetBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBoolean(this IDataRecord record, int i, bool defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBoolean(this IDataRecord record, string name, bool defaultIfNull)
        {
            return record.GetBoolean(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean? GetNullableBoolean(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Boolean?) : record.GetBoolean(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean? GetNullableBoolean(this IDataRecord record, string name)
        {
            return record.GetNullableBoolean(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Byte GetByte(this IDataRecord record, string name)
        {
            return record.GetByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Byte GetByte(this IDataRecord record, int i, Byte defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Byte GetByte(this IDataRecord record, string name, Byte defaultIfNull)
        {
            return record.GetByte(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/> of <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Byte? GetNullableByte(this IDataRecord record, string name)
        {
            return record.GetNullableByte(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Byte&gt;"/> of <see cref="Byte"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Byte? GetNullableByte(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Byte?) : record.GetByte(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Char GetChar(this IDataRecord record, string name)
        {
            return record.GetChar(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Char GetChar(this IDataRecord record, int i, Char defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Char GetChar(this IDataRecord record, string name, Char defaultIfNull)
        {
            return record.GetChar(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Char&gt;"/> of <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Char? GetNullableChar(this IDataRecord record, string name)
        {
            return record.GetNullableChar(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Char&gt;"/> of <see cref="Char"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Char? GetNullableChar(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Char?) : record.GetChar(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, string name)
        {
            return record.GetDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, int i, DateTime defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDateTime(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime GetDateTime(this IDataRecord record, string name, DateTime defaultIfNull)
        {
            return record.GetDateTime(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/> of <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime? GetNullableDateTime(this IDataRecord record, string name)
        {
            return record.GetNullableDateTime(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;DateTime&gt;"/> of <see cref="DateTime"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static DateTime? GetNullableDateTime(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(DateTime?) : record.GetDateTime(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Decimal GetDecimal(this IDataRecord record, string name)
        {
            return record.GetDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Decimal GetDecimal(this IDataRecord record, int i, Decimal defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDecimal(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Decimal GetDecimal(this IDataRecord record, string name, Decimal defaultIfNull)
        {
            return record.GetDecimal(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/> of <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Decimal? GetNullableDecimal(this IDataRecord record, string name)
        {
            return record.GetNullableDecimal(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Decimal&gt;"/> of <see cref="Decimal"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Decimal? GetNullableDecimal(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Decimal?) : record.GetDecimal(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Double GetDouble(this IDataRecord record, string name)
        {
            return record.GetDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Double GetDouble(this IDataRecord record, int i, Double defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetDouble(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Double GetDouble(this IDataRecord record, string name, Double defaultIfNull)
        {
            return record.GetDouble(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Double&gt;"/> of <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Double? GetNullableDouble(this IDataRecord record, string name)
        {
            return record.GetNullableDouble(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Double&gt;"/> of <see cref="Double"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Double? GetNullableDouble(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Double?) : record.GetDouble(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Single GetSingle(this IDataRecord record, string name)
        {
            return record.GetFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Single GetSingle(this IDataRecord record, int i, Single defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetFloat(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Single GetSingle(this IDataRecord record, string name, Single defaultIfNull)
        {
            return record.GetSingle(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Single&gt;"/> of <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Single? GetNullableFloat(this IDataRecord record, string name)
        {
            return record.GetNullableFloat(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Single&gt;"/> of <see cref="Single"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Single? GetNullableFloat(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Single?) : record.GetFloat(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int16 GetInt16(this IDataRecord record, string name)
        {
            return record.GetInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int16 GetInt16(this IDataRecord record, int i, Int16 defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt16(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int16 GetInt16(this IDataRecord record, string name, Int16 defaultIfNull)
        {
            return record.GetInt16(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int16&gt;"/> of <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int16? GetNullableInt16(this IDataRecord record, string name)
        {
            return record.GetNullableInt16(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int16&gt;"/> of <see cref="Int16"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int16? GetNullableInt16(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Int16?) : record.GetInt16(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int32 GetInt32(this IDataRecord record, string name)
        {
            return record.GetInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int32 GetInt32(this IDataRecord record, int i, Int32 defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt32(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int32 GetInt32(this IDataRecord record, string name, Int32 defaultIfNull)
        {
            return record.GetInt32(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int32&gt;"/> of <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int32? GetNullableInt32(this IDataRecord record, string name)
        {
            return record.GetNullableInt32(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int32&gt;"/> of <see cref="Int32"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int32? GetNullableInt32(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Int32?) : record.GetInt32(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int64 GetInt64(this IDataRecord record, string name)
        {
            return record.GetInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int64 GetInt64(this IDataRecord record, int i, Int64 defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetInt64(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int64 GetInt64(this IDataRecord record, string name, Int64 defaultIfNull)
        {
            return record.GetInt64(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int64&gt;"/> of <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int64? GetNullableInt64(this IDataRecord record, string name)
        {
            return record.GetNullableInt64(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Int64&gt;"/> of <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Int64? GetNullableInt64(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Int64?) : record.GetInt64(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static String GetString(this IDataRecord record, string name)
        {
            return record.GetString(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static String GetString(this IDataRecord record, int i, string defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static String GetString(this IDataRecord record, string name, string defaultIfNull)
        {
            return record.GetString(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static String GetStringOrNull(this IDataRecord record, string name)
        {
            return record.GetStringOrNull(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="String"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static String GetStringOrNull(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(String) : record.GetString(i);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Object GetValue(this IDataRecord record, string name)
        {
            return record.GetValue(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Object GetValue(this IDataRecord record, int i, Object defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetValue(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Int64"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Object GetValue(this IDataRecord record, string name, Object defaultIfNull)
        {
            return record.GetValue(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Object"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Object GetValueOrNull(this IDataRecord record, string name)
        {
            return record.GetValueOrNull(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Object"/>, or null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Object GetValueOrNull(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? null : record.GetValue(i);
        }

        /// <summary>
        /// Gets the value of the specified column as an <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, string name)
        {
            return record.GetGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, int i, Guid defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetGuid(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid GetGuid(this IDataRecord record, string name, Guid defaultIfNull)
        {
            return record.GetGuid(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Guid&gt;"/> of <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid? GetNullableGuid(this IDataRecord record, string name)
        {
            return record.GetNullableGuid(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Guid&gt;"/> of <see cref="Guid"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Guid? GetNullableGuid(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Guid?) : record.GetGuid(i);
        }

        #region BooleanEx methods (accepts Y, N, T, F, Yes, No, True, False, 0, 1)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .Net Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBooleanEx(this IDataRecord record, int i)
        {
            return Conv.ToBoolean(record.GetValue(i));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// Accepts T, F, Y, N, Yes, No.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBooleanEx(this IDataRecord record, string name)
        {
            return record.GetBooleanEx(record.GetOrdinal(name));
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBooleanEx(this IDataRecord record, int i, bool defaultIfNull)
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetBooleanEx(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <param name="defaultIfNull">The value to return if the value of the column is null.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean GetBooleanEx(this IDataRecord record, string name, bool defaultIfNull)
        {
            return record.GetBooleanEx(record.GetOrdinal(name), defaultIfNull);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean? GetNullableBooleanEx(this IDataRecord record, int i)
        {
            return record.IsDBNull(i) ? default(Boolean?) : record.GetBooleanEx(i);
        }

        /// <summary>
        /// Gets the value of the specified column as a <see cref="Nullable&lt;Boolean&gt;"/> of <see cref="Boolean"/>.
        /// </summary>
        /// <param name="record">The data record.</param>
        /// <param name="name">The name of the column to find.</param>
        /// <returns>The value of the column.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static Boolean? GetNullableBooleanEx(this IDataRecord record, string name)
        {
            return record.GetNullableBooleanEx(record.GetOrdinal(name));
        }
        #endregion

        #region Enumeration Conversion Extensions
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, int i)
            where T : struct
        {
            return Conv.ToEnum<T>(record.GetValue(i));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, string name)
             where T : struct
        {
            return record.GetEnum<T>(record.GetOrdinal(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, int i, T defaultIfNull)
             where T : struct
        {
            return record.IsDBNull(i) ? defaultIfNull : record.GetEnum<T>(i);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T GetEnum<T>(this IDataRecord record, string name, T defaultIfNull)
             where T : struct
        {
            return record.GetEnum<T>(record.GetOrdinal(name), defaultIfNull);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "i", Justification = "Consistent with .NET Framework."),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T? GetNullableEnum<T>(this IDataRecord record, int i)
             where T : struct
        {
            return record.IsDBNull(i) ? default(T?) : record.GetEnum<T>(i);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Unnecessary overhead.")]
        public static T? GetNullableEnum<T>(this IDataRecord record, string name)
             where T : struct
        {
            return record.GetNullableEnum<T>(record.GetOrdinal(name));
        }
        #endregion
    }
}
