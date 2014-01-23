using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
