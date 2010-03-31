namespace PokerTell.User
{
    using System.Configuration;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces;

    public class UserConfiguration : IUserConfiguration
    {
        readonly Configuration _config;

        public UserConfiguration()
        {
            string configFile = Files.LocalUserAppDataPath + @"\" + Files.UserConfigFile;

            // Map the new configuration file.
            var configFileMap =
                new ExeConfigurationFileMap { ExeConfigFilename = configFile };

            // Get the mapped configuration file
            _config = ConfigurationManager.OpenMappedExeConfiguration(
                configFileMap, ConfigurationUserLevel.None);
        }

        public KeyValueConfigurationCollection AppSettings
        {
            get { return _config.AppSettings.Settings; }
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _config.ConnectionStrings.ConnectionStrings; }
        }

        public void Save(ConfigurationSaveMode saveMode)
        {
            _config.Save(saveMode);
        }
    }
}