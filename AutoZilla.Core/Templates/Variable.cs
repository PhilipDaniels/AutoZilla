using AutoZilla.Core.Extensions;
using AutoZilla.Core.Validation;
using System;
using System.Diagnostics;

namespace AutoZilla.Core.Templates
{
    /// <summary>
    /// Represents a variable, typically as found within a template. Variables
    /// are created from strings of the form "${NAME!WIDTH:PATTERN}"
    /// where NAME, WIDTH and PATTERN are all optional but you must have at
    /// least a NAME or WIDTH.
    /// </summary>
    [DebuggerDisplay("Specification")]
    internal class Variable
    {
        /// <summary>
        /// The initial string specification of the variable, i.e. the text that
        /// was used as the basis for parsing out into components.
        /// </summary>
        public string Specification { get; private set; }

        /// <summary>
        /// The name of the variable. Can be null or empty.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The <code>WidthSpecification</code> determines how output will
        /// be padded and aligned.
        /// </summary>
        public WidthSpecification Width { get; private set; }

        /// <summary>
        /// The pattern is a .Net format string pattern, as used with
        /// <code>ToString()</code>.
        /// </summary>
        public string Pattern { get; private set; }

        /// <summary>
        /// Construct a new variable object. This has the effect of parsing out the
        /// components of the variable. This may throw an exception if the specification
        /// is invalid.
        /// </summary>
        /// <param name="specification">String rep of the variable.</param>
        public Variable(string specification)
        {
            specification.ThrowIfNull("specification");

            // Trim off leading "${" and trailing "}".
            if (specification.StartsWith("${") && specification.EndsWith("}"))
                Specification = specification.Substring(2, specification.Length - 3);
            else
                Specification = specification;

            specification.ThrowIfNullOrWhiteSpace("specification");
            
            string name, width, pattern;
            GetComponents(Specification, out name, out width, out pattern);
            Name = name;
            if (!String.IsNullOrWhiteSpace(width))
                Width = new WidthSpecification(width);
            Pattern = pattern;
        }

        /// <summary>
        /// Formats a thing by first applying the <code>Pattern</code>
        /// and then any <code>Width</code> specification.
        /// </summary>
        /// <param name="thing">The thing to format.</param>
        /// <returns>String representation of the thing.</returns>
        public string ApplyFormat(object thing)
        {
            string text = "";

            
            if (String.IsNullOrWhiteSpace(Name))
            {
                // Nameless variables are assumed to be literals.
                // You can pass null to blank them out.
                if (thing != null)
                {
                    text = Pattern;
                }
            }
            else
            {
                if (thing != null)
                {
                    var pattern = "{0:" + Pattern + "}";
                    text = String.Format(pattern, thing);
                }
            }

            // You don't have to have a Width specified, in which case you
            // just get back what you asked for.
            if (Width != null)
                text = text.PadAndAlign(Width.MinWidth, Width.MaxWidth, Width.Alignment, Width.PadChar);

            return text;
        }

        
        
        
        void GetComponents(string specification, out string name, out string width, out string pattern)
        {
            // In this logic: a) if there is an exclamation mark then there is a width (must not be blank).
            //                b) if you want to specify a width you must include an exclamation mark (and it will necessarily precede the colon)

            // null patterns are bad because they are ambiguous when passed to ToString(), so we use "" instead.
            // widths can be null or "" (they mean the same thing) because we are in charge of handling them
            // via another class.

            int excIndex = specification.IndexOf('!');
            int colonIndex = specification.IndexOf(':');

            if (excIndex == -1 && colonIndex == -1)
            {
                // Neither found. We have ${NAME}
                if (String.IsNullOrWhiteSpace(specification))
                    throw new VariableException("Empty variable specifications are not permitted.", Specification);
                name = specification;
                width = null;
                pattern = "";
            }
            else if (excIndex == -1 && colonIndex != -1)
            {
                // Only colon found. We have ${:PATTERN} or ${NAME:PATTERN} where PATTERN does not contain !.
                name = specification.Substring(0, colonIndex);
                width = null;
                pattern = specification.Substring(colonIndex + 1);
            }
            else if (excIndex != -1 && colonIndex == -1)
            {
                // Only exclamation mark found. We have ${!WIDTH} or ${NAME!WIDTH}
                name = specification.Substring(0, excIndex);
                width = specification.Substring(excIndex + 1);
                pattern = "";
            }
            else
            {
                // Both found. Which is first?
                if (excIndex < colonIndex)
                {
                    // Exclamation mark precedes colon, therefore there is a width.
                    // Problem is the pad char may be ! or :.
                    // Also we can't "scan in from the right" because the pattern may contain anything.
                    name = specification.Substring(0, excIndex);
                    char padChar = specification[excIndex + 1];

                    if (padChar == '!' || padChar == ':')
                    {
                        // We have one of:
                        // ${NAME!:WIDTH}
                        // ${NAME!!WIDTH}
                        // ${NAME!:WIDTH:PATTERN}
                        // ${NAME!!WIDTH:PATTERN}
                        // ${NAME!
                        int colonIndex2 = specification.IndexOf(':', excIndex + 2);
                        if (colonIndex2 == -1)
                        {
                            // No colon past the pad char means there is no pattern.
                            width = specification.Substring(excIndex + 1);
                            pattern = "";
                        }
                        else
                        {
                            // A colon past the pad char means we must have a pattern.
                            width = specification.Substring(excIndex + 1, colonIndex2 - excIndex - 1);
                            pattern = specification.Substring(colonIndex2 + 1);
                        }
                    }
                    else
                    {
                        // We know there must be a : and it cannot be at 'x' or in WIDTH, so it must be the PATTERN delimiter.
                        // ${!xWIDTH:PATTERN}
                        // ${NAME!xWIDTH:PATTERN}
                        width = specification.Substring(excIndex + 1, colonIndex - excIndex - 1);
                        pattern = specification.Substring(colonIndex + 1);
                    }
                }
                else
                {
                    // Colon precedes exclamation mark, therefore we have ${NAME:PATTERN} where PATTERN may contain an exclamation mark.
                    name = specification.Substring(0, colonIndex);
                    width = null;
                    pattern = specification.Substring(colonIndex + 1);
                }
            }
        }
    }
}
