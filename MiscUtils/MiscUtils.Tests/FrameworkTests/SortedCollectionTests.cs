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
using NUnit.Framework;
using System;
using System.Linq;

namespace MiscUtils.Tests.FrameworkTests
{
    [TestFixture]
    public class SortedCollectionTests
    {
        class Car
        {
            public int Year;
        }

        [Test]
        public void key_extractor_cannot_be_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SortedCollection<Car, int>(null, true));
        }

        [Test]
        public void allows_duplicates_if_asked_to_do_so()
        {
            var car1 = new Car() { Year = 2012 };
            var car2 = new Car() { Year = 2012 };

            var sc = new SortedCollection<Car, int>((c) => c.Year, true);
            sc.Add(car1);
            sc.Add(car2);
        }

        [Test]
        public void prevents_duplicates_if_asked_to_do_so()
        {
            var car1 = new Car() { Year = 2012 };
            var car2 = new Car() { Year = 2012 };

            var sc = new SortedCollection<Car, int>((c) => c.Year, false);
            sc.Add(car1);
            Assert.Throws<ArgumentException>(() => sc.Add(car1));
        }

        [Test]
        public void returns_items_in_sorted_order()
        {
            var car1 = new Car() { Year = 2012 };
            var car2 = new Car() { Year = 1990 };
            var car3 = new Car() { Year = 2000 };

            var sortedCars = new SortedCollection<Car, int>((c) => c.Year, false);
            sortedCars.Add(car1);
            sortedCars.Add(car2);
            sortedCars.Add(car3);

            Assert.True(sortedCars.IsStrictlySorted<Car, int>((c) => c.Year));
            Assert.AreSame(car1, sortedCars.Values.ElementAt(2));
            Assert.AreSame(car2, sortedCars.Values.ElementAt(0));
            Assert.AreSame(car3, sortedCars.Values.ElementAt(1));
        }
    }
}
