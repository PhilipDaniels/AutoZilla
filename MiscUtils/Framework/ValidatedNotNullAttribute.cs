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

namespace MiscUtils.Framework
{
    /// <summary>
    /// VS Code Analysis will not actually recognise the generic helper method
    /// (and possibly sometimes non-generics) as having checked parameters, so
    /// CA1062 warnings will not go away. By introducing this dummy attribute
    /// we can fool the code analysis engine, and stop it producing false
    /// warnings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
