using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoZilla.Core.Extensions;
using AutoZilla.Core.Validation;
using System.Reflection;
using System.Diagnostics;

namespace AutoZilla.Core.Templates
{
    public static class AutoZillaVariables
    {
        public static string ExePath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string ExeFolder
        {
            get
            {
                return Path.GetDirectoryName(ExePath);
            }
        }

        public static string PluginsFolder
        {
            get
            {
                return Path.Combine(ExeFolder, "Plugins");
            }
        }

        public static string DomainUser
        {
            get
            {
                return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
        }

        public static string Domain
        {
            get
            {
                return DomainUser.Before(@"\");
            }
        }

        public static string User
        {
            get
            {
                return DomainUser.After(@"\");
            }
        }

        public static DateTime Date
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static string MachineName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        public static object GetByName(string name)
        {
            name.ThrowIfNullOrWhiteSpace("name", "The name of the variable must be specified.");

            var property = MyPropertyByName(name);
            if (property == null)
            {
                string msg = String.Format("No built-in AutoZilla variable with the name '{0}' exists.", name);
                throw new ArgumentOutOfRangeException("name", msg);
            }

            return property.GetValue(null, null);
        }



        static PropertyInfo MyPropertyByName(string name)
        {
            return MyProperties.SingleOrDefault(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        static PropertyInfo[] MyProperties
        {
            get
            {
                if (_MyProperties == null)
                {
                    _MyProperties = typeof(AutoZillaVariables).GetProperties(BindingFlags.Public | BindingFlags.Static);
                }
                return _MyProperties;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        static PropertyInfo[] _MyProperties;
    }
}
