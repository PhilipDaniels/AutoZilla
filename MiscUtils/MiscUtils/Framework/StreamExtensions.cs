// Copyright 2014 Philip Daniels - http://www.philipdaniels.com/
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

namespace MiscUtils.Framework
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Completely reads a stream from its current position and returns the data as an array of bytes.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>Array of bytes from the stream.</returns>
        public static byte[] ReadFully(this Stream stream)
        {
            stream.ThrowIfNull("stream");

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
