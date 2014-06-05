using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CZ
{
    /// See http://codecutout.com/xml-deserialization-from-app-config

    /// <summary>
    /// This class contains only one method, Create().
    /// This is designed to handle custom sections in an Applications Configuration file.
    /// By Implementing <see cref="IConfigurationSectionHandler"/>, we can implement the
    /// Create method, which will provide the XmlNode from the configuration file. This is
    /// Deserialized into an object and passed back to the Caller.
    /// </summary>
    /// <example>
    /// Here is a configuration file entry in the <c>configSections</c> sectikon of the <c>App.Config</c>
    /// file.
    ///<code>	///
    ///&lt;section name="ServerConfig" type="ConfigSectionHandler.ConfigSectionHandler, ConfigSectionHandler" /&gt;
    ///</code>
    ///This tells the CLR that there is a section further in, with a node name of <c>ServerConfig</c>. When this section
    ///is to be parsed, an object of type <c>ConfigSectionHandler.ConfigSectionHandler</c> which resides in the 
    ///assembly <c>ConfigSectionHandler</c> will be instantiated. The CLR automatically calls a method in that object
    ///called <c>Create</c>
    ///</example>
    public abstract class XmlDeserializeConfigSectionHandler : IConfigurationSectionHandler
    {
        public XmlDeserializeConfigSectionHandler()
            : base()
        {
        }

        /// <summary>
        /// A method which is called by the CLR when parsing the App.Config file. If custom sections
        /// are found, then an entry in the configuration file will tell the runtime to call this method,
        /// passing in the XmlNode required.
        /// </summary>
        /// <param name="parent">The configuration settings in a corresponding parent configuration section. Passed in via the CLR</param>
        /// <param name="configContext">An <see cref="HttpConfigurationContext"/> when Create is called from the ASP.NET configuration system. Otherwise, 
        /// this parameter is reserved and is a null reference (Nothing in Visual Basic). Passed in via the CLR</param>
        /// <param name="section">The <see cref="XmlNode"/> that contains the configuration information from the configuration file. 
        /// Provides direct access to the XML contents of the configuration section. 	Passed in via the CLR.</param>
        /// <returns>The Deserialized object as an object</returns>
        /// <exception cref="System.Configuration.ConfigurationException">The Configuration file is not well formed,
        /// or the Custom section is not configured correctly, or the type of configuration handler was not specified correctly
        /// or the type of object was not specified correctly.
        /// or the copn</exception>
        public object Create(object parent, object configContext, XmlNode section)
        {
            Type t = this.GetType();
            XmlSerializer ser = new XmlSerializer(t);
            using (XmlNodeReader xNodeReader = new XmlNodeReader(section))
            {
                var thing = ser.Deserialize(xNodeReader);
                ApplyDefaultValues(thing);
                var errors = ValidateLoadedObject(thing);
                if (errors != null && errors.Count > 0)
                {
                    string msg = ErrorsToString(errors, section);
                    throw new ConfigurationErrorsException(msg);
                }
                return thing;
            }
        }

        /// <summary>
        /// Finds all properties in the object that have a <code>DefaultValueAttribute</code>
        /// on them and sets them if the properties currently have the same value as the
        /// default for their type, e.g. only set the value if numbers are 0, strings are null, etc.
        /// </summary>
        /// <param name="thing">Thing to set defaults on.</param>
        void ApplyDefaultValues(object thing)
        {
            // Find all properties that have a and set them.
            Type t = thing.GetType();

            var properties = from p in t.GetProperties()
                             let attrs = p.GetCustomAttributes(typeof(DefaultValueAttribute), false)
                             where attrs != null && attrs.Count() > 0
                             select new { Property = p, Attribute = attrs.First() as DefaultValueAttribute };

            foreach (var property in properties)
            {
                var currentVal = property.Property.GetValue(thing, null);
                var defaultTypeVal = GetDefaultValue(property.Property.PropertyType);
                if (Object.Equals(currentVal, defaultTypeVal))
                {
                    property.Property.SetValue(thing, property.Attribute.Value, null);
                }
            }
        }

        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Check that the loaded object is valid. You can apply any validation
        /// attribute that you want (even your own, use custom messages etc.).
        /// </summary>
        /// <param name="thing">Deserialized section object.</param>
        IList<ValidationResult> ValidateLoadedObject(object thing)
        {
            var context = new ValidationContext(thing, null, null);
            var errors = new List<ValidationResult>();
            Validator.TryValidateObject(thing, context, errors, true);
            return errors;
        }

        /// <summary>
        /// Render any configuration errors down to a useful string.
        /// </summary>
        /// <param name="errors">Set of errors.</param>
        /// <returns>String rep.</returns>
        string ErrorsToString(IEnumerable<ValidationResult> errors, XmlNode section)
        {
            var sb = new StringBuilder();

            if (errors != null && errors.Count() > 0)
            {
                sb.AppendFormat("There are errors in your .config file at section {0}.", section.Name);
                sb.AppendLine();
                foreach (var error in errors)
                {
                    sb.AppendFormat("  {0}{1}", error.ErrorMessage, Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
