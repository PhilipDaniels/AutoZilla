using AutoZilla.Core.GlobalHotKeys;
using MiscUtils.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// A simple type of text template that can do variable substitution.
    /// </summary>
    public class TextTemplate
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static Regex TemplateVariableRegex = new Regex
            (
            @"(?<!\\)" +        // Do not match ${var} when it is actually \${var}
            @"\${(.*?)}",       // match ${var}
            RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// This event is raised when the template is replacing a variable.
        /// You can specify the value the variable should have by setting it
        /// in the event args.
        /// </summary>
        public event EventHandler<VariableReplacementEventArgs> OnVariableReplacement;

        /// <summary>
        /// The key that the template is bound to, that is, pressing this
        /// key should invoke the template. It can be null.
        /// </summary>
        public ModifiedKey ModifiedKey { get; private set; }

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

        /// <summary>
        /// Create a new <code>Template</code> object without any keys etc., just the text.
        /// </summary>
        /// <param name="originalText">The text of the template.</param>
        public TextTemplate(string originalText)
            : this(null, null, null, null, originalText)
        {
        }

        /// <summary>
        /// Create a new <code>Template</code> object.
        /// </summary>
        /// <param name="key">The modified key that the template will be invoked by.</param>
        /// <param name="originalText">The text of the template.</param>
        public TextTemplate(ModifiedKey key, string originalText)
            : this(key, null, null, null, originalText)
        {
        }

        /// <summary>
        /// Create a new <code>Template</code> object.
        /// </summary>
        /// <param name="key">The modified key that the template will be invoked by.</param>
        /// <param name="name">Name of the template. Keep this short.</param>
        /// <param name="description">Description of the template. More descriptive than the name.</param>
        /// <param name="filePath">The full path of the file that the template was loaded from.</param>
        /// <param name="originalText">The text of the template.</param>
        public TextTemplate(ModifiedKey key, string name, string description, string filePath, string originalText)
        {
            originalText.ThrowIfNull("originalText", "You must specify the text for the template.");

            ModifiedKey = key;
            Name = name;
            Description = description;
            FilePath = filePath;
            OriginalText = originalText;
        }

        /// <summary>
        /// Processes the template by applying the default variable substitutions.
        /// </summary>
        /// <returns>The text of the template after variable substitution.</returns>
        public string Process()
        {
            return Process(null, false);
        }

        /// <summary>
        /// Processes the template by applying the default variable substitutions
        /// and optionally allowing the variable values to be overridden by
        /// specifying them in <paramref name="variableValues"/>.
        /// </summary>
        /// <param name="variableValues">Dictionary (variable name -> variable value) of values to
        /// substitute into the template.</param>
        /// <returns>The text of the template after variable substitution.</returns>
        public string Process(IDictionary<string, object> variableValues)
        {
            return Process(variableValues, false);
        }

        /// <summary>
        /// Processes the template by applying the default variable substitutions
        /// and optionally allowing the variable values to be overridden by
        /// specifying them in <paramref name="variableValues"/>.
        /// </summary>
        /// <param name="variableValues">Dictionary (variable name -> variable value) of values to
        /// substitute into the template.</param>
        /// <param name="trimOneLeadingNewLine">If true and the template <code>OriginalText</code> begins
        /// with a new line then that new line is trimmed before substitution.</param>
        /// <returns>The text of the template after variable substitution.</returns>
        public string Process(IDictionary<string, object> variableValues, bool trimOneLeadingNewLine)
        {
            string text = OriginalText;
            if (trimOneLeadingNewLine)
                text = text.TrimOneLeadingNewLine();

            VariableValues = variableValues;

            int variableCount = 0;
            if (VariableValues != null)
            {
                variableCount = VariableValues.Count;
            }
            log.DebugFormat
                (
                CultureInfo.InvariantCulture,
                "Processing template, Name = {0}, FilePath = {1}, variableValues.Count = {2}",
                Name, FilePath, variableCount
                );

            // Find all variable markers of the form ${VAR} and replace them with the appropriate data.
            string replacedText = TemplateVariableRegex.Replace(text, m => VariableReplacer(m));
            return replacedText;
        }


        string VariableReplacer(Match match)
        {
            var variable = new Variable(match.Value);
            object thing = GetThing(variable);
            string replacement = variable.ApplyFormat(thing);

            log.DebugFormat(CultureInfo.InvariantCulture, "Replaced variable {0} with value {1}", variable.Name, replacement);

            return replacement;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        object GetThing(Variable variable)
        {
            // Any values the user specifies have priority. This allows you to override
            // what the USER variable means for example.
            object thing = "";
            if (VariableValues != null && 
                !String.IsNullOrEmpty(variable.Name) &&
                VariableValues.TryGetValue(variable.Name, out thing))
            {
                log.DebugFormat(CultureInfo.InvariantCulture, "GetThing for variable {0}, found entry in VariableValues of {1}", variable.Name, thing);
                return thing;
            }

            // Given the user an opportunity to replace via an event.
            // If this is a built in variable try use it to initialise the Thing.
            var e = OnVariableReplacement;
            if (e != null)
            {
                object variableVal = null;
                try
                {
                    variableVal = AutoZillaVariables.GetByName(variable.Name);
                }
                catch
                {
                }

                var args = new VariableReplacementEventArgs(variable.Name, variableVal);

                e(this, args);
                if (args.Thing != null)
                {
                    log.DebugFormat(CultureInfo.InvariantCulture, "GetThing for variable {0}, event handler supplied the value {1}", variable.Name, args.Thing);
                    return args.Thing;
                }
            }

            // Deal with all built in variables. This will throw if the
            // variable is not found.
            thing = AutoZillaVariables.GetByName(variable.Name);
            log.DebugFormat(CultureInfo.InvariantCulture, "GetThing for variable {0}, used built-in value of {1}", variable.Name, thing);
            return thing;
        }
    }
}
