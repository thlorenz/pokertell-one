namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Input;

    using Base;

    using log4net;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Win32;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF;

    public class ConfigureMySqlDataProviderViewModel : ConfigureDataProviderViewModelBase
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IDataProviderInfo _dataProviderInfo;

        readonly DatabaseConnectionInfo _pokerOfficeSettings;

        ICommand _getPokerOfficeSettingsCommand;

        public ConfigureMySqlDataProviderViewModel(
            IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDatabaseConnector databaseConnector)
            : base(eventAggregator, databaseSettings, databaseConnector)
        {
            _dataProviderInfo = new MySqlInfo();

            DetectMySqlAndSetMySqlVersionInfoAccordinly();

            _pokerOfficeSettings = GetPokerofficeSettings();

            Initialize();
        }

        public ICommand GetPokerOfficeSettingsCommand
        {
            get
            {
                return _getPokerOfficeSettingsCommand ?? (_getPokerOfficeSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg =>
                                          InititializeWith(
                                              _pokerOfficeSettings.Server, 
                                              _pokerOfficeSettings.User, 
                                              _pokerOfficeSettings.Password), 
                        CanExecuteDelegate = arg => _pokerOfficeSettings != null
                    });
            }
        }

        public string MySqlVersionInfo { get; private set; }

        public bool GetPokerOfficeSettingsVisible
        {
            get { return GetPokerOfficeSettingsCommand.CanExecute(null); }
        }

        protected override IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        /// <summary>
        /// Find MySQL
        /// Now regkey is set to MySql AB
        /// Find MySQL Server
        /// </summary>
        /// <param name="mySqlDir">Directory where MySql is installed</param>
        /// <param name="mySqlVersion">Version of installed MySql</param>
        static void ExtractMySqlDirectoryAndVersionFromRegistry(out string mySqlDir, out string mySqlVersion)
        {
            RegistryKey regKey = Registry.LocalMachine;
            regKey = regKey.OpenSubKey("SOFTWARE");

            mySqlDir = null;
            mySqlVersion = null;

            string subKeyNameLower;
            try
            {
                foreach (string subKeyName in regKey.GetSubKeyNames())
                {
                    subKeyNameLower = subKeyName.ToLower();
                    if (subKeyNameLower.Equals("mysql ab"))
                    {
                        regKey = regKey.OpenSubKey(subKeyName);
                        break;
                    }
                }

                foreach (string subKeyName in regKey.GetSubKeyNames())
                {
                    subKeyNameLower = subKeyName.ToLower();
                    if (subKeyNameLower.StartsWith("mysql server"))
                    {
                        regKey = regKey.OpenSubKey(subKeyName);
                        mySqlDir = regKey.GetValue("Location").ToString();
                        mySqlVersion = regKey.GetValue("Version").ToString();
                    }
                }
            }
            catch (Exception excep)
            {
                // Registry unaccessible will throw
                // Any not found registry key will throw null reference
                // In both cases, nothing we can do, but to assume MySql is not installed
                Log.Error(excep);
            }
        }

        /// <summary>
        /// Tries to find PokerOffice in registry and read its database settings from its config file
        /// </summary>
        /// <returns>Null if not successful</returns>
        static DatabaseConnectionInfo GetPokerofficeSettings()
        {
            StreamReader reader;

            RegistryKey regKey;

            string pokerOfficeDir = string.Empty;

            const string patHost = @"mysql://(?<Host>.*):\d+/";
            const string patUser = @"<user>(?<User>.*)</user>";
            const string patPassword = @"<pass>(?<Password>.*)</pass>";

            Match m;

            try
            {
                regKey = Registry.LocalMachine;
                regKey = regKey.CreateSubKey(@"SOFTWARE");

                if (regKey != null)
                {
                    foreach (string subKeyName in regKey.GetSubKeyNames())
                    {
                        string subKeyNameLower = subKeyName.ToLower();
                        if (subKeyNameLower.Equals("pokeroffice"))
                        {
                            regKey = regKey.CreateSubKey(subKeyName);
                            pokerOfficeDir = regKey.GetValue(string.Empty).ToString();
                        }
                    }
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            // Get Info from PokerOffice Files
            if (string.IsNullOrEmpty(pokerOfficeDir))
            {
                return null;
            }

            // Get Default PokerOffice Database
            try
            {
                // Get Other MySQL Info
                using (reader = new StreamReader(pokerOfficeDir + @"\settings\mysql.connection"))
                {
                    string strRead = reader.ReadToEnd();

                    m = Regex.Match(strRead, patHost);
                    string strHost = m.Groups["Host"].ToString();

                    m = Regex.Match(strRead, patUser);
                    string strUser = m.Groups["User"].ToString();

                    m = Regex.Match(strRead, patPassword);
                    string strPassword = m.Groups["Password"].ToString();

                    return new DatabaseConnectionInfo(strHost, strUser, strPassword);
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
                return null;
            }
        }

        void DetectMySqlAndSetMySqlVersionInfoAccordinly()
        {
            string mySqlDir;
            string mySqlVersion;

            ExtractMySqlDirectoryAndVersionFromRegistry(out mySqlDir, out mySqlVersion);

            if (string.IsNullOrEmpty(mySqlDir))
            {
                var userMessage =
                    new UserMessageEventArgs(UserMessageTypes.Warning, Resources.Warning_MySqlInstallationNotFound);
                _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);

                MySqlVersionInfo = Resources.Info_MySqlInstallationNotFound;
            }
            else
            {
                MySqlVersionInfo = string.Format(Resources.Info_FoundMySqInstallation, mySqlVersion);
            }
        }
    }
}