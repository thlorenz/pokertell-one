namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Input;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using log4net;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Win32;

    using Properties;

    using Tools.WPF;

    public class AddMySqlDataProviderViewModel : AddDataProviderViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IDataProviderInfo _dataProviderInfo;

        #endregion

        #region Constructors and Destructors

        public AddMySqlDataProviderViewModel(
            IEventAggregator eventAggregator, IDatabaseSettings databaseSettings, IDataProvider dataProvider)
            : base(eventAggregator, databaseSettings, dataProvider)
        {
            _dataProviderInfo = new MySqlInfo();
        }

        #endregion

        #region Properties

        protected override IDataProviderInfo DataProviderInfo
        {
            get { return _dataProviderInfo; }
        }

        ICommand _getPokerOfficeSettingsCommand;

        public ICommand GetPokerOfficeSettingsCommand
        {
            get
            {
                return _getPokerOfficeSettingsCommand ?? (_getPokerOfficeSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var pokerOfficeSettings = GetPokerofficeSettings();
                            if (pokerOfficeSettings != null)
                            {
                                InititializeWith(
                                    pokerOfficeSettings.Server, pokerOfficeSettings.User, pokerOfficeSettings.Password);
                            }
                        }
                    });
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tries to find PokerOffice in registry and read its database settings from its config file
        /// </summary>
        /// <returns>Null if not successful</returns>
        DatabaseConnectionInfo GetPokerofficeSettings()
        {
            StreamReader reader;

            RegistryKey regKey;

            string pokerOfficeDir = string.Empty;

            //Regular Expressions
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
                            pokerOfficeDir = regKey.GetValue("").ToString();
                        }
                    }
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            //Get Info from PokerOffice Files
            if (string.IsNullOrEmpty(pokerOfficeDir))
            {
                var userMessage =
                    new UserMessageEventArgs(UserMessageTypes.Warning, Resources.Warning_PokerOfficeNotFound);
                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Publish(userMessage);

                return null;
            }
            else
            {
                //Get Default PokerOffice Database
                try
                {

                    //Get Other MySQL Info
                    using (reader = new StreamReader(pokerOfficeDir + @"\settings\mysql.connection"))
                    {
                        string strRead = reader.ReadToEnd();

                        m = Regex.Match(strRead, patHost);
                        var strHost = m.Groups["Host"].ToString();

                        m = Regex.Match(strRead, patUser);
                        var strUser = m.Groups["User"].ToString();

                        m = Regex.Match(strRead, patPassword);
                        var strPassword = m.Groups["Password"].ToString();

                        return new DatabaseConnectionInfo(strHost, strUser, strPassword);
                    }
                    
                }
                catch (FileNotFoundException excep)
                {
                    var userMessage =
                        new UserMessageEventArgs(UserMessageTypes.Warning, Resources.Warning_MySqlNotUsedInPokerOffice, excep);
                    _eventAggregator
                        .GetEvent<UserMessageEvent>()
                        .Publish(userMessage);

                    return null;
                }
                catch (Exception excep)
                {
                    Log.Error("Unexpected", excep);
                    return null;
                }
            }
        }

        #endregion
    }
}