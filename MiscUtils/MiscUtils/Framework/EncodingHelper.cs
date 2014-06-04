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

using System.IO;
using System.Text;

namespace MiscUtils.Framework
{
    public static class EncodingHelper
    {
        /// <summary>
        /// Convert bytes in one encoding to a string in another encoding.
        /// </summary>
        /// <param name="data">The bytes to be converted.</param>
        /// <param name="sourceEncoding">The current encoding of the data.</param>
        /// <param name="targetEncoding">The desired encoding of the data.</param>
        /// <returns>The bytes converted to a string.</returns>
        public static string BytesToString(this byte[] data, Encoding sourceEncoding, Encoding targetEncoding)
        {
            data.ThrowIfNull("data");
            data.Length.ThrowIfLessThanOrEqualTo(0, "data.Length");
            sourceEncoding.ThrowIfNull("sourceEncoding");
            targetEncoding.ThrowIfNull("targetEncoding");

            byte[] bytesInTargetEncoding = Encoding.Convert(sourceEncoding, targetEncoding, data);
            return targetEncoding.GetString(bytesInTargetEncoding);
        }

        /// <summary>
        /// Try to guess the encoding of a file using the Bom. If the Bom is less than 4 bytes
        /// long then the default will be returned (you probably have an ASCII file that is less
        /// than 4 bytes long).
        /// </summary>
        /// <param name="fileName">Name of the file. The first 4 bytes will be read.</param>
        /// <returns>The encoding of the file, or the default if it could not be determined.</returns>
        public static Encoding GuessEncodingFromBom(string fileName)
        {
            fileName.ThrowIfFileDoesNotExist("fileName");

            var Bom = new byte[4];
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                file.Read(Bom, 0, 4);
            }

            return GuessEncodingFromBom(Bom);
        }

        /// <summary>
        /// Try to guess the encoding of a byte array using the Bom. If the Bom is less than 4 bytes
        /// long then the default will be returned (you probably have an ASCII file that is less
        /// than 4 bytes long).
        /// </summary>
        /// <param name="bom">Byte array containing the Bom.</param>
        /// <returns>The encoding of the data, or the default if it could not be determined.</returns>
        public static Encoding GuessEncodingFromBom(byte[] bom)
        {
            return GuessEncodingFromBom(bom, Encoding.Default);
        }

        /// <summary>
        /// Try to guess the encoding of a byte array using the Bom. If the Bom is less than 4 bytes
        /// long then the default will be returned (you probably have an ASCII file that is less
        /// than 4 bytes long).
        /// </summary>
        /// <param name="Bom">Byte array containing the Bom.</param>
        /// <param name="defaultIfUndetermined">If the encoding cannot be determined,
        /// what to return instead.</param>
        /// <returns>The encoding of the data, or the default if it could not be determined.</returns>
        public static Encoding GuessEncodingFromBom(byte[] data, Encoding defaultIfUndetermined)
        {
            data.ThrowIfNull("data");

            if (data.Length < 4)
                return defaultIfUndetermined;

            // Analyze the Bom
            if (data[0] == 0x2b && data[1] == 0x2f && data[2] == 0x76)
                return Encoding.UTF7;
            if (data[0] == 0xef && data[1] == 0xbb && data[2] == 0xbf)
                return Encoding.UTF8;
            if (data[0] == 0xff && data[1] == 0xfe)
                return Encoding.Unicode; //UTF-16LE
            if (data[0] == 0xfe && data[1] == 0xff)
                return Encoding.BigEndianUnicode; //UTF-16BE
            if (data[0] == 0 && data[1] == 0 && data[2] == 0xfe && data[3] == 0xff)
                return Encoding.UTF32;

            return defaultIfUndetermined;
        }

    }
}
