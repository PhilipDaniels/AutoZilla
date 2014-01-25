using AutoZilla.Core.Extensions;
using AutoZilla.Core.GlobalHotkeys;
using AutoZilla.Core.Validation;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// The key that the template is bound to, that is, pressing this
        /// key should invoke the template. It can be null.
        /// </summary>
        public ModifiedKey Key { get; private set; }

        /// <summary>
        /// Short name of the template.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the template. Intended to be human-readable
        /// text suitable for presenting in the AutoZilla GUI.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The path that this template was loaded from. Will be null
        /// for templates that are created in memory.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Returns the filename (without the full path) that this
        /// template was loaded from. Will be null for templates that
        /// are created in memory.
        /// </summary>
        public string Filename
        {
            get
            {
                if (FilePath == null)
                {
                    return null;
                }
                else
                {
                    return Path.GetFileName(FilePath);
                }
            }
        }

        /// <summary>
        /// The original text of the template, before any variable replacement is done.
        /// </summary>
        public string OriginalText { get; private set; }

        /// <summary>
        /// The set of variable values that you passed in when you constructed
        /// the template. These values will be substituted into the appropriate
        /// places in the template.
        /// </summary>
        IDictionary<string, object> VariableValues { get; set; }

        public Template(string originalText)
            : this(null, null, null, null, originalText)
        {
        }

        public Template(ModifiedKey key, string originalText)
            : this(key, null, null, null, originalText)
        {
        }

        public Template(ModifiedKey key, string name, string description, string filePath, string originalText)
        {
            originalText.ThrowIfNull("originalText", "You must specify the text for the template.");

            Key = key;
            Name = name;
            Description = description;
            FilePath = filePath;
            OriginalText = originalText;
        }

        public string Process()
        {
            return Process(null, false);
        }

        public string Process(IDictionary<string, object> variableValues)
        {
            return Process(variableValues, false);
        }

        public string Process(IDictionary<string, object> variableValues, bool trimOneLeadingNewLine)
        {
            string text = OriginalText;
            if (trimOneLeadingNewLine)
                text = text.TrimOneLeadingNewLine();

            VariableValues = variableValues;

            // Find all variable markers of the form ${VAR} and replace them with the appropriate data.
            string replacedText = TemplateVariableRegex.Replace(text, m => VariableReplacer(m));
            return replacedText;
        }

        /*
        /// <summary>
        /// The text of the template after variable replacement.
        /// </summary>
        public string ReplacedText { get; private set; }

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
        */

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

            // TODO: Raise an event here.


            // Deal with all built in variables.
            switch (variable.Name)
            {
                case "DOMAIN":
                    thing = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Before(@"\");
                    break;
                case "USER":
                    thing = System.Security.Principal.WindowsIdentity.GetCurrent().Name.After(@"\");
                    break;
                case "DOMAINUSER":
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
