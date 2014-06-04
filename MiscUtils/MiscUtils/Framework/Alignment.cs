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


namespace MiscUtils.Framework
{
    /// <summary>
    /// How to align the input string when padding.
    /// </summary>
    /// <remarks>
    /// I hate enums, can never remember:
    /// a.ToString() gives "Left"
    /// Enum.Parse(typeof(Alignment), "Right") gives Alignment.Right
    /// 
    /// (char)a gives '&lt;'
    /// var b = (Alignment)'&gt;' // b is "Right"
    /// </remarks>
    public enum Alignment
    {
        /// <summary>
        /// No alignment. This is not a valid value
        /// within AutoZilla, but all enums should have it.
        /// </summary>
        None = 0,

        /// <summary>
        /// Align the input string to the left: "ab" -> "ab  ".
        /// </summary>
        Left = '<',

        /// <summary>
        /// Align the input string to the right: "ab" -> "  ab".
        /// </summary>
        Right = '>',

        /// <summary>
        /// Align the input string to the center: "ab" -> " ab ".
        /// </summary>
        Center = '^'
    }
}
