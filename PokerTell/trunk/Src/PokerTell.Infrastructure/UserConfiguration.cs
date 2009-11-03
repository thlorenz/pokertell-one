namespace PokerTell.Infrastructure
{
    using System.Configuration;

    using Interfaces;

    internal class UserConfiguration : IUserConfiguration
    {
        #region Constants and Fields

        readonly Configuration _config;

        #endregion

        #region Constructors and Destructors

        internal UserConfiguration()
        {
            string configFile = Files.dirAppData + Files.xmlUserConfig;

            // Map the new configuration file.
            var configFileMap =
                new ExeConfigurationFileMap { ExeConfigFilename = configFile };
            
            // Get the mapped configuration file

            _config = ConfigurationManager.OpenMappedExeConfiguration(
                configFileMap, ConfigurationUserLevel.None);
        }

        #endregion

        #region Properties

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _config.ConnectionStrings.ConnectionStrings;  }
        }

        public KeyValueConfigurationCollection AppSettings
        {
            get { return _config.AppSettings.Settings; }
        }

        public void Save(ConfigurationSaveMode saveMode)
        {
           _config.Save(saveMode);
        }

        #endregion
    }
}