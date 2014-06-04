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

using MiscUtils.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscUtils.Tests.DataTests
{
    public class ObjectDataReaderTests
    {
        #region Helpers
        IEnumerable<Car> GetEmptyCarCollection()
        {
            return new List<Car>();
        }

        IEnumerable<Car> GetCarsWithoutNulls()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNoNulls(),
                Car.MakeCarWithNoNulls()
            };
        }

        IEnumerable<Car> GetCarsWithoutNullsWithPreviousModel()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNoNullsAndPreviousModel(),
                Car.MakeCarWithNoNullsAndPreviousModel()
            };
        }



        IEnumerable<Car> GetNullableCars()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNulls(),
                Car.MakeCarWithNulls()
            };
        }

        IEnumerable<Car> GetNullableCarsWithPreviousModels()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNullsAndPreviousModel(),
                Car.MakeCarWithNullsAndPreviousModel()
            };
        }



        IEnumerable<Car> GetCarsOfBothTypes()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNulls(),
                Car.MakeCarWithNoNulls()
            };
        }

        IEnumerable<Car> GetCarsOfBothTypesWithPreviousModels()
        {
            return new List<Car>()
            {
                Car.MakeCarWithNullsAndPreviousModel(),
                Car.MakeCarWithNoNullsAndPreviousModel()
            };
        }

        void IncludesFieldsFromMainObject<T>(ObjectDataReader<T> reader)
        {
            IncludesFieldsFromPrefixedObject(reader, "");
        }

        void IncludesFieldsFromPrefixedObject<T>(ObjectDataReader<T> reader, string prefix)
        {
            var columns = reader.GetColumns();
            CollectionAssert.Contains(columns, prefix + "IntField");
            CollectionAssert.Contains(columns, prefix + "NullableIntField");
            CollectionAssert.Contains(columns, prefix + "ReadOnlyIntField");
            CollectionAssert.Contains(columns, prefix + "NullableReadOnlyIntField");
            CollectionAssert.Contains(columns, prefix + "ReadOnlyStringField");
            CollectionAssert.Contains(columns, prefix + "StringProperty");
            CollectionAssert.Contains(columns, prefix + "EnumField");
            CollectionAssert.Contains(columns, prefix + "NullableEnumField");
        }

        void CanReadAllRowsAndColumns<T>(ObjectDataReader<T> reader)
        {
            var columns = reader.GetColumns();
            while (reader.Read())
            {
                for (int i = 0; i < columns.Count(); i++)
                {
                    object o1, o2;
                    Assert.DoesNotThrow(() => o1 = reader[i]);
                    Assert.DoesNotThrow(() => o2 = reader[columns[i]]);
                }
            }
        }

        void DoMainChecks<T>(ObjectDataReader<T> reader)
        {
            IncludesFieldsFromMainObject(reader);
            CanReadAllRowsAndColumns(reader);
        }

        void TestAllParameterCombinations(IEnumerable<Car> cars)
        {
            foreach (var nt in Enum.GetValues(typeof(NullConversion)))
            {
                var reader = new ObjectDataReader<Car>(cars, NullConversion.ToDBNull);
                DoMainChecks<Car>(reader);
            }
        }
        #endregion



        [Test]
        public void does_not_allow_null_collection()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectDataReader<Car>(null));
        }

        [Test]
        public void test_parameter_combinations()
        {
            TestAllParameterCombinations(GetEmptyCarCollection());
            TestAllParameterCombinations(GetNullableCars());
            TestAllParameterCombinations(GetNullableCarsWithPreviousModels());
            TestAllParameterCombinations(GetCarsWithoutNulls());
            TestAllParameterCombinations(GetCarsWithoutNullsWithPreviousModel());
            TestAllParameterCombinations(GetCarsOfBothTypes());
            TestAllParameterCombinations(GetCarsOfBothTypesWithPreviousModels());
        }

        [Test]
        public void as_db_null_converts_nulls_to_db_null()
        {
            var reader = new ObjectDataReader<Car>(GetNullableCars(), NullConversion.ToDBNull);
            reader.Read();

            Assert.True(Convert.IsDBNull(reader["NullableIntField"]));
            Assert.True(Convert.IsDBNull(reader["NullableReadOnlyIntField"]));
            Assert.True(Convert.IsDBNull(reader["ReadOnlyStringField"]));
            Assert.True(Convert.IsDBNull(reader["NullableEnumField"]));
        }

        [Test]
        public void as_nullable_gives_nullable_object()
        {
            var reader = new ObjectDataReader<Car>(GetNullableCars(), NullConversion.None);
            reader.Read();

            Assert.IsNull(reader["NullableIntField"]);
            Assert.IsNull(reader["NullableReadOnlyIntField"]);
            Assert.IsNull(reader["ReadOnlyStringField"]);
            Assert.IsNull(reader["NullableEnumField"]);
        }

        [Test]
        public void can_make_reader_on_collection_of_scalars()
        {
            var ints = new int[] { 5, 6, 7 };
            var rdr = ints.AsDataReader();
            var dt = rdr.ToDataTable();
        }

        [Test]
        public void data_table_works()
        {
            var cars = GetCarsOfBothTypes();
            using (var reader = cars.AsDataReader(NullConversion.ToDBNull))
            {
                var dt = reader.ToDataTable();
            }
        }
    }
}
