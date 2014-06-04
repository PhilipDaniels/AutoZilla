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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiscUtils.Framework
{
    public class ObjectValidator : IObjectValidator
    {
        object value;

        public ObjectValidator(object value)
        {
            value.ThrowIfNull("value");

            this.value = value;
        }

        public virtual IList<ValidationResult> Validate()
        {
            return Validate(false);
        }

        public virtual IList<ValidationResult> Validate(bool throwException)
        {
            ValidationContext context = new ValidationContext(value, null, null);
            var errors = new List<ValidationResult>();

            if (!Validator.TryValidateObject(value, context, errors, true))
            {
                if (throwException)
                {
                    throw new ValidationException("Validation failed. " + ErrorsToString(errors));
                }
                else
                {
                    return errors;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual bool IsValid
        {
            get
            {
                var errors = Validate();
                return errors == null || errors.Count == 0;
            }
        }

        public static string ErrorsToString(IList<ValidationResult> errors)
        {
            var sb = new StringBuilder();

            if (errors != null && errors.Count > 0)
            {
                sb.AppendLine(" Errors:");
                foreach (var error in errors)
                {
                    sb.AppendFormat("  {0}{1}", error.ErrorMessage, Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
