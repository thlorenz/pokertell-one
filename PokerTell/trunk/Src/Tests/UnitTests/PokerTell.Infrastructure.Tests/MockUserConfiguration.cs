namespace PokerTell.Infrastructure.Tests
{
    using System.Configuration;

    using Interfaces;

    public class MockUserConfiguration : IUserConfiguration
    {
        readonly ConnectionStringSettingsCollection _connectionStringSettingsCollection;

        readonly KeyValueConfigurationCollection _appSettingsCollection;

        public MockUserConfiguration()
        {
            _appSettingsCollection = new KeyValueConfigurationCollection();
            _connectionStringSettingsCollection = new ConnectionStringSettingsCollection();
        }

        public ConnectionStringSettingsCollection ConnectionStrings
        {
            get { return _connectionStringSettingsCollection; }
        }

        public KeyValueConfigurationCollection AppSettings
        {
            get { return _appSettingsCollection; }
        }

        public void Save(ConfigurationSaveMode saveMode)
        {
            // Yeah man it's saved - don't you worry, hi, hi, hi, hi
        }
    }
}