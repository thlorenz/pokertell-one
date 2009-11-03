//Date: 4/21/2009

namespace PokerTell.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Interfaces;

    using log4net;

    using Tools.Extensions;

    /// <summary>
    /// Contains Save method which works for all objects
    /// Contains Persist Methods for objects of type int,double,string,Point,Size,Color
    /// </summary>
    public class Settings : ISettings
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUserConfiguration _userConfiguration;

        #endregion

        public Settings(IUserConfiguration userConfiguration)
        {
            _userConfiguration = userConfiguration;
        }

        #region Public Methods

        public void Persist(string strKey, out int objValue)
        {
            Persist(strKey, out objValue, int.MinValue);
        }

        public void Persist(string strKey, out int objValue, int objDefault)
        {

            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;
                if (! int.TryParse(strValue, out objValue))
                {
                    Log.DebugFormat(
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

        public void Persist(string strKey, out bool objValue)
        {
            Persist(strKey, out objValue, false);
        }

        public void Persist(string strKey, out bool objValue, bool objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;
                if (! bool.TryParse(strValue, out objValue))
                {
                    Log.DebugFormat(
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

        public void Persist(string strKey, out double objValue)
        {
            Persist(strKey, out objValue, double.MinValue);
        }

        public void Persist(string strKey, out double objValue, double objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;
                if (! double.TryParse(strValue, out objValue))
                {
                    Log.DebugFormat(
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

        public void Persist(string strKey, out string objValue)
        {
            Persist(strKey, out objValue, string.Empty);
        }

        public void Persist(string strKey, out string objValue, string objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;
                objValue = strValue;
            }
            else
            {
                objValue = objDefault;
            }
        }

        public void Persist(string strKey, out Point objValue)
        {
            Persist(strKey, out objValue, Point.Empty);
        }

        public void Persist(string strKey, out Point objValue, Point objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;

                const string patPoint = @"{X=(?<X>-{0,1}\d+),.*Y=(?<Y>-{0,1}-{0,1}\d+)}";

                Match m = Regex.Match(strValue, patPoint);
                if (m.Success)
                {
                    int x = int.Parse(m.Groups["X"].Value);
                    int y = int.Parse(m.Groups["Y"].Value);
                    objValue = new Point(x, y);
                }
                else
                {
                    Log.DebugFormat(
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

        public void Persist(string strKey, out Size objValue)
        {
            Persist(strKey, out objValue, Size.Empty);
        }

        public void Persist(string strKey, out Size objValue, Size objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;

                const string patSize = @"{Width=(?<Width>\d+),.*Height=(?<Height>\d+)}";

                Match m = Regex.Match(strValue, patSize);

                if (m.Success)
                {
                    var width = int.Parse(m.Groups["Width"].Value);
                    var height = int.Parse(m.Groups["Height"].Value);
                    objValue = new Size(width, height);
                }
                else
                {
                    Log.DebugFormat(
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

        public void Persist(string strKey, out Color objValue)
        {
            Persist(strKey, out objValue, Color.Empty);
        }

        public void Persist(string strKey, out Color objValue, Color objDefault)
        {
            if (_userConfiguration.AppSettings[strKey] != null)
            {
                string strValue = _userConfiguration.AppSettings[strKey].Value;

                const string patKnownColor = @"Color \[(?<Name>\w+)\]";

                Match m = Regex.Match(strValue, patKnownColor);
                if (m.Success)
                {
                    objValue = Color.FromName(m.Groups["Name"].Value);
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

                        objValue = Color.FromArgb(a, r, g, b);
                    }
                    else
                    {
                        Log.DebugFormat(
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

        public void Save(string strKey, object objValue)
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

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _userConfiguration.ConnectionStrings; }
        }

        public KeyValueConfigurationCollection AppSettings
        {
            get { return _userConfiguration.AppSettings; }
        }

        public void Save(ConfigurationSaveMode saveMode)
        {
           _userConfiguration.Save(saveMode);
        }
    }
}