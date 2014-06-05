using System.Configuration;

namespace CZ
{
    /// <summary>
    /// Class to return the connection strings.
    /// </summary>
    public static class ConnStrings
    {
        public static string ConnStr1
        {
            get { return ConfigurationManager.ConnectionStrings["ConnStr1"].ConnectionString; }
        }

        public static string ConnStr2
        {
            get { return ConfigurationManager.ConnectionStrings["ConnStr2"].ConnectionString; }
        }
    }
}
