using System.Configuration;

using PokerTell.Infrastructure.Interfaces;

namespace PokerTell.User
{
    using Infrastructure;

    public class UserConfiguration : IUserConfiguration
    {
        #region Constants and Fields

        readonly Configuration _config;

        #endregion

        #region Constructors and Destructors

        public UserConfiguration()
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