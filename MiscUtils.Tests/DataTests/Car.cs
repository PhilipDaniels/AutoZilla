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
using System.Data;
using System.Linq;
using System.Text;

namespace MiscUtils.Tests.DataTests
{
    public enum DriveType
    {
        LeftHand,
        RightHand
    }

    public class Car
    {
        public int IntField = 1971;
        public int? NullableIntField;
        public readonly int ReadOnlyIntField;
        public readonly int? NullableReadOnlyIntField;
        public string StringField = "Hello world";
        public readonly String ReadOnlyStringField;
        public string StringProperty { get; set; }
        public DriveType EnumField = DriveType.RightHand;
        public DriveType? NullableEnumField = null;

        public Car PreviousModelField;
        public Car PreviousModelProperty { get; set; }
        public Car SupersedingModel;

        public Car(int roIntField, int? nullableROIntField, string roString)
        {
            ReadOnlyIntField = roIntField;
            NullableReadOnlyIntField = nullableROIntField;
            ReadOnlyStringField = roString;
        }



        // Static factory methods to help with testing.
        public static Car MakeCarWithNulls()
        {
            return new Car(50, null, null);
        }

        public static Car MakeCarWithNoNulls()
        {
            return new Car(50, 60, "RO String")
                {
                    NullableIntField = 1972,
                    StringProperty = "String Prop",
                    NullableEnumField = DriveType.LeftHand
                };
        }

        public static Car MakeCarWithNullsAndPreviousModel()
        {
            var c = MakeCarWithNulls();
            c.PreviousModelField = MakeCarWithNulls();
            c.PreviousModelProperty = MakeCarWithNulls();
            return c;
        }

        public static Car MakeCarWithNoNullsAndPreviousModel()
        {
            var c = MakeCarWithNoNulls();
            c.PreviousModelField = MakeCarWithNoNulls();
            c.PreviousModelField.StringField = "PreviousModelFieldString";
            c.PreviousModelProperty = MakeCarWithNoNulls();
            c.PreviousModelProperty.StringField = "PreviousModelPropertyString";
            return c;
        }
    }
}
