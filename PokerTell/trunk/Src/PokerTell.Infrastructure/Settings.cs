//Date: 4/21/2009

namespace PokerTell.Infrastructure
{
    using System.Configuration;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using log4net;

    using Tools.Extensions;

    /// <summary>
    /// Contains Save method which works for all objects
    /// Contains Persist Methods for objects of type int,double,string,Point,Size,Color
    /// </summary>
    public static class Settings
    {
        #region Constants and Fields

        static readonly ILog log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods

        public static void Persist(string strKey, out int objValue)
        {
            Persist(strKey, out objValue, int.MinValue);
        }

        public static void Persist(string strKey, out int objValue, int objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;
                if (! int.TryParse(strValue, out objValue))
                {
                    log.DebugFormat(
                        "Failed to load int with key: [{0}] value: [{1}] correctly", 
                        strKey.ToStringNullSafe(), 
                        strValue.ToStringNullSafe());
                    objValue = objDefault;
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out bool objValue)
        {
            Persist(strKey, out objValue, false);
        }

        public static void Persist(string strKey, out bool objValue, bool objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;
                if (! bool.TryParse(strValue, out objValue))
                {
                    log.DebugFormat(
                        "Failed to load bool with key: [{0}] value: [{1}] correctly", 
                        strKey.ToStringNullSafe(), 
                        strValue.ToStringNullSafe());
                    objValue = objDefault;
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out double objValue)
        {
            Persist(strKey, out objValue, double.MinValue);
        }

        public static void Persist(string strKey, out double objValue, double objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;
                if (! double.TryParse(strValue, out objValue))
                {
                    log.DebugFormat(
                        "Failed to load double with key: [{0}] value: [{1}] correctly", 
                        strKey.ToStringNullSafe(), 
                        strValue.ToStringNullSafe());
                    objValue = objDefault;
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out string objValue)
        {
            Persist(strKey, out objValue, string.Empty);
        }

        public static void Persist(string strKey, out string objValue, string objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;
                objValue = strValue;
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out Point objValue)
        {
            Persist(strKey, out objValue, Point.Empty);
        }

        public static void Persist(string strKey, out Point objValue, Point objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;

                string patPoint = @"{X=(?<X>-{0,1}\d+),.*Y=(?<Y>-{0,1}-{0,1}\d+)}";
                Match m;

                m = Regex.Match(strValue, patPoint);
                if (m.Success)
                {
                    int x = int.Parse(m.Groups["X"].Value);
                    int y = int.Parse(m.Groups["Y"].Value);
                    objValue = new Point(x, y);
                }
                else
                {
                    log.DebugFormat(
                        "Failed to load Point with key: [{0}] value: [{1}] correctly", 
                        strKey.ToStringNullSafe(), 
                        strValue.ToStringNullSafe());
                    objValue = objDefault;
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out Size objValue)
        {
            Persist(strKey, out objValue, Size.Empty);
        }

        public static void Persist(string strKey, out Size objValue, Size objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;

                string patSize = @"{Width=(?<Width>\d+),.*Height=(?<Height>\d+)}";

                Match m = Regex.Match(strValue, patSize);

                if (m.Success)
                {
                    int Width = int.Parse(m.Groups["Width"].Value);
                    int Height = int.Parse(m.Groups["Height"].Value);
                    objValue = new Size(Width, Height);
                }
                else
                {
                    log.DebugFormat(
                        "Failed to load Size with key: [{0}] value: [{1}] correctly", 
                        strKey.ToStringNullSafe(), 
                        strValue.ToStringNullSafe());
                    objValue = objDefault;
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Persist(string strKey, out Color objValue)
        {
            Persist(strKey, out objValue, Color.Empty);
        }

        public static void Persist(string strKey, out Color objValue, Color objDefault)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] != null)
            {
                string strValue = config.AppSettings.Settings[strKey].Value;

                string patKnownColor = @"Color \[(?<Name>\w+)\]";

                Match m = Regex.Match(strValue, patKnownColor);
                if (m.Success)
                {
                    objValue = Color.FromName(m.Groups["Name"].Value);
                }
                else
                {
                    string patColorFromArgb = @"Color \[A=(?<A>\d+), R=(?<R>\d+), G=(?<G>\d+), B=(?<B>\d+)\]";
                    m = Regex.Match(strValue, patColorFromArgb);
                    if (m.Success)
                    {
                        int a = int.Parse(m.Groups["A"].Value);
                        int r = int.Parse(m.Groups["R"].Value);
                        int g = int.Parse(m.Groups["G"].Value);
                        int b = int.Parse(m.Groups["B"].Value);

                        objValue = Color.FromArgb(a, r, g, b);
                    }
                    else
                    {
                        log.DebugFormat(
                            "Failed to load Argb Color with key: [{0}] value: [{1}] correctly", 
                            strKey.ToStringNullSafe(), 
                            strValue.ToStringNullSafe());
                        objValue = objDefault;
                    }
                }
            }
            else
            {
                objValue = objDefault;
            }
        }

        public static void Save(string strKey, object objValue)
        {
            Configuration config = UserData.Config;

            if (config.AppSettings.Settings[strKey] == null)
            {
                config.AppSettings.Settings.Add(strKey, objValue.ToString());
            }
            else
            {
                config.AppSettings.Settings[strKey].Value = objValue.ToString();
            }

            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Provides string representation of all (key,value) pairs in settings
        /// </summary>
        /// <returns>String representation of all (key,value) pairs</returns>
        public static string ShowAll()
        {
            Configuration config = UserData.Config;

            var sbToString = new StringBuilder();
            if (config != null)
            {
                foreach (KeyValueConfigurationElement iConfElement in config.AppSettings.Settings)
                {
                    sbToString.AppendFormat("{0}:   \"{1}\"\n", iConfElement.Key, iConfElement.Value);
                }
            }

            return "\n" + sbToString;
        }

        #endregion
    }
}