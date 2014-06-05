using System.Configuration;

namespace CZ
{
    /// <summary>
    /// Class to return the AppSettings.
    /// </summary>
    public static class AppSettings
    {
        public static string Setting1
        {
            get { return ConfigurationManager.AppSettings["Setting1"]; }
        }

        public static string Setting2
        {
            get { return ConfigurationManager.AppSettings["Setting2"]; }
        }
    }
}
