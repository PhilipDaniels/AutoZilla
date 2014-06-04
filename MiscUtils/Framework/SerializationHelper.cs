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

using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace MiscUtils.Framework
{
    public static class SerializationHelper
    {
        public static string SerializeObjectToXmlString<T>(T value)
        {
            value.ThrowIfNull("value");

            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(sw, value);
                return sw.ToString();
            }
        }

        public static T DeserializeXmlStringToObject<T>(string xml)
        {
            xml.ThrowIfNullOrWhiteSpace("xml");

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                var data = (T)serializer.Deserialize(reader);
                return data;
            }
        }
    }
}
