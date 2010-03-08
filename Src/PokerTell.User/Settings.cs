//Date: 4/21/2009
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace PokerTell.User
{
    using System.Drawing;

    using Infrastructure.Interfaces;

    using log4net;

    using Tools.Extensions;

    /// <summary>
    /// Contains Set method which works for all objects
    /// Contains Retrieve Methods for objects of type int,double,string,Point,Size,Color
    /// </summary>
    public class Settings : ISettings
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUserConfiguration _userConfiguration;

        #endregion

        #region Constructors and Destructors

        public Settings(IUserConfiguration userConfiguration)
        {
            _userConfiguration = userConfiguration;
        }

        #endregion

        #region Properties

        public KeyValueConfigurationCollection AppSettings
        {
            get { return _userConfiguration.AppSettings; }
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _userConfiguration.ConnectionStrings; }
        }

        #endregion

        #region Implemented Interfaces

        #region ISettings

        public bool RetrieveBool(string key)
        {
            return RetrieveBool(key, false);
        }

        public bool RetrieveBool(string key, bool defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;
                bool result;
                if (!bool.TryParse(strValue, out result))
                {
                    Log.DebugFormat(
                        "Failed to load bool with key: [{0}] value: [{1}] correctly",
                        key.ToStringNullSafe(),
                        strValue.ToStringNullSafe());
                    return defaultValue;
                }
                return result;
            }
            return defaultValue;
        }

        public Color RetrieveColor(string key)
        {
            return RetrieveColor(key, Color.Empty);
        }

        public Color RetrieveColor(string key, Color defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;

                const string patKnownColor = @"Color \[(?<Name>\w+)\]";

                Match m = Regex.Match(strValue, patKnownColor);
                if (m.Success)
                {
                    return Color.FromName(m.Groups["Name"].Value);
                }
                else
                {
                    const string patColorFromArgb = @"Color \[A=(?<A>\d+), R=(?<R>\d+), G=(?<G>\d+), B=(?<B>\d+)\]";
                    m = Regex.Match(strValue, patColorFromArgb);
                    if (m.Success)
                    {
                        int a = int.Parse(m.Groups["A"].Value);
                        int r = int.Parse(m.Groups["R"].Value);
                        int g = int.Parse(m.Groups["G"].Value);
                        int b = int.Parse(m.Groups["B"].Value);

                        return Color.FromArgb(a, r, g, b);
                    }
                    else
                    {
                        Log.DebugFormat(
                            "Failed to load Argb Color with key: [{0}] value: [{1}] correctly",
                            key.ToStringNullSafe(),
                            strValue.ToStringNullSafe());
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        public double RetrieveDouble(string key)
        {
            return RetrieveDouble(key, double.MinValue);
        }

        public double RetrieveDouble(string key, double defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;
                double result;
                if (!double.TryParse(strValue, out result))
                {
                    Log.DebugFormat(
                        "Failed to load double with key: [{0}] value: [{1}] correctly",
                        key.ToStringNullSafe(),
                        strValue.ToStringNullSafe());
                    return defaultValue;
                }
                return result;
            }

            return defaultValue;
        }

        public int RetrieveInt(string key)
        {
            return RetrieveInt(key, int.MinValue);
        }

        public int RetrieveInt(string key, int defaultValue)
        {
            int result;
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;
                if (! int.TryParse(strValue, out result))
                {
                    Log.DebugFormat(
                        "Failed to load int with key: [{0}] value: [{1}] correctly",
                        key.ToStringNullSafe(),
                        strValue.ToStringNullSafe());
                    result = defaultValue;
                }
            }
            else
            {
                result = defaultValue;
            }

            return result;
        }

        public Point RetrievePoint(string key)
        {
            return RetrievePoint(key, Point.Empty);
        }

        public Point RetrievePoint(string key, Point defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;

                const string patPoint = @"{X=(?<X>-{0,1}\d+),.*Y=(?<Y>-{0,1}-{0,1}\d+)}";

                Match m = Regex.Match(strValue, patPoint);
                if (m.Success)
                {
                    int x = int.Parse(m.Groups["X"].Value);
                    int y = int.Parse(m.Groups["Y"].Value);
                    return new Point(x, y);
                }
                Log.DebugFormat(
                    "Failed to load Point with key: [{0}] value: [{1}] correctly",
                    key.ToStringNullSafe(),
                    strValue.ToStringNullSafe());
                return defaultValue;
            }
            return defaultValue;
        }

        public Size RetrieveSize(string key)
        {
            return RetrieveSize(key, Size.Empty);
        }

        public Size RetrieveSize(string key, Size defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                string strValue = _userConfiguration.AppSettings[key].Value;

                const string patSize = @"{Width=(?<Width>\d+),.*Height=(?<Height>\d+)}";

                Match m = Regex.Match(strValue, patSize);

                if (m.Success)
                {
                    int width = int.Parse(m.Groups["Width"].Value);
                    int height = int.Parse(m.Groups["Height"].Value);
                    return new Size(width, height);
                }
                Log.DebugFormat(
                    "Failed to load Size with key: [{0}] value: [{1}] correctly",
                    key.ToStringNullSafe(),
                    strValue.ToStringNullSafe());
                return defaultValue;
            }
            return defaultValue;
        }

        public string RetrieveString(string key)
        {
            return RetrieveString(key, string.Empty);
        }

        public string RetrieveString(string key, string defaultValue)
        {
            if (_userConfiguration.AppSettings[key] != null)
            {
                return _userConfiguration.AppSettings[key].Value;
            }
            return defaultValue;
        }

        public void Set(string strKey, object objValue)
        {
            if (_userConfiguration.AppSettings[strKey] == null)
            {
                _userConfiguration.AppSettings.Add(strKey, objValue.ToString());
            }
            else
            {
                _userConfiguration.AppSettings[strKey].Value = objValue.ToString();
            }

            _userConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Provides string representation of all (key,value) pairs in settings
        /// </summary>
        /// <returns>String representation of all (key,value) pairs</returns>
        public string ShowAll()
        {
            var sbToString = new StringBuilder();

            foreach (KeyValueConfigurationElement iConfElement in _userConfiguration.AppSettings)
            {
                sbToString.AppendFormat("{0}:   \"{1}\"\n", iConfElement.Key, iConfElement.Value);
            }

            return "\n" + sbToString;
        }

        #endregion

        #region IUserConfiguration

        public void Save(ConfigurationSaveMode saveMode)
        {
            _userConfiguration.Save(saveMode);
        }

        #endregion

        #endregion
    }
}