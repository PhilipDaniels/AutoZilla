using AutoZilla.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// A simple type of text template that can do variable substitution.
    /// </summary>
    public class Template
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static Regex TemplateVariableRegex = new Regex
            (
            @"(?<!\\)" +        // Do not match ${var} when it is actually \${var}
            @"\${(.*?)}",       // match ${var}
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// The original text of the template, before any variable replacement is done.
        /// </summary>
        public readonly string OriginalText;

        /// <summary>
        /// The text of the template after variable replacement.
        /// </summary>
        public readonly string ReplacedText;

        /// <summary>
        /// The set of variable values that you passed in when you constructed
        /// the template. These values will be substituted into the appropriate
        /// places in the template.
        /// </summary>
        public readonly IDictionary<string, object> VariableValues;
        
        /// <summary>
        /// Construct a new template from the specified text. No user
        /// variables are specified.
        /// </summary>
        /// <param name="text">The text of the template.</param>
        public Template(string text)
            : this(text, null)
        {
        }

        // TODO: Allow dynamic? Only .Net 4.
        /// <summary>
        /// Construct a new template from the specified text. Values for user variables
        /// can be specified using a dictionary (which can be null or empty or partially
        /// populated).
        /// </summary>
        /// <param name="text">The text of the template.</param>
        /// <param name="variableValues">Dictionary in which the key of an entry is the name of a variable
        /// and the value of an entry is the value to be used by the formatter.</param>
        public Template(string text, IDictionary<string, object> variableValues)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text = text.TrimOneLeadingNewLine();

            OriginalText = text;
            VariableValues = variableValues;

            // Find all variable markers of the form ${VAR} and replace them with the appropriate data.
            ReplacedText = TemplateVariableRegex.Replace(text, m => VariableReplacer(m));
        }

        string VariableReplacer(Match match)
        {
            var variable = new Variable(match.Value);
            object thing = GetThing(variable);
            string replacement = variable.ApplyFormat(thing);
            return replacement;
        }

        object GetThing(Variable variable)
        {
            // Any values the user specifies have priority. This allows you to override
            // what the USER variable means for example.
            object thing = "";
            if (VariableValues != null && 
                !String.IsNullOrEmpty(variable.Name) &&
                VariableValues.TryGetValue(variable.Name, out thing))
            {
                return thing;
            }

            // Deal with all built in variables.
            switch (variable.Name)
            {
                case "USER":
                    thing = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    break;
                case "DATE":
                    thing = DateTime.Now;
                    break;
                case "MACHINE":
                    thing = Environment.MachineName;
                    break;
                default:
                    throw new Exception("Unknown internal variable: " + variable.Name);
            }

            return thing;
        }
    }
}
