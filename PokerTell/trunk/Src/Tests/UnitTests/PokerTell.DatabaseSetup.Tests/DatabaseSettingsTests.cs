namespace PokerTell.DatabaseSetup.Tests
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    using Enumerations;

    using Infrastructure;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Moq;

    using NUnit.Framework;

    using UnitTests.Fakes;

    using User;

    public class DatabaseSettingsTests
    {
        #region Constants and Fields

        ISettings _mockSettings;

        MockUserConfiguration _mockUserConfiguration;

        IDataProviderInfo _mySqlProvider; // External

        IDataProviderInfo _sqLiteProvider; // Embedded

        StubBuilder _stub;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _mockUserConfiguration = new MockUserConfiguration();
            _mockSettings = new Settings(_mockUserConfiguration);
            _mySqlProvider = new MySqlInfo();
            _sqLiteProvider = new SqLiteInfo();
        }

        [Test]
        public void ConnectionStringExistsFor_ConnectionsStringForAnotherProviderWasAdded_ReturnsFalse()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            _mockSettings.ConnectionStrings
                .Add(new ConnectionStringSettings(_mySqlProvider.FullName, "someConnectionString"));

            Assert.That(databaseSettings.ConnectionStringExistsFor(_sqLiteProvider), Is.False);
        }

        [Test]
        public void ConnectionStringExistsFor_ConnectionsStringForProviderWasAdded_ReturnsTrue()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            _mockSettings.ConnectionStrings
                .Add(new ConnectionStringSettings(_sqLiteProvider.FullName, "someConnectionString"));

            Assert.That(databaseSettings.ConnectionStringExistsFor(_sqLiteProvider), Is.True);
        }

        [Test]
        public void ConnectionStringExistsFor_ConnectionStringForProviderWasAdded_ReturnsTrue()
        {
            var dataProviders = new DataProviderInfos();

            _mockSettings
                .ConnectionStrings.Add(new ConnectionStringSettings(_sqLiteProvider.FullName, "someConnectionString"));

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.ConnectionStringExistsFor(_sqLiteProvider), Is.True);
        }

        [Test]
        public void ConnectionStringExistsFor_ConnectionStringForProviderWasNotAdded_ReturnsFalse()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.ConnectionStringExistsFor(_sqLiteProvider), Is.False);
        }

        [Test]
        public void ConnectionStringExistsFor_NoConnectionStringWasAdded_ReturnsFalse()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.ConnectionStringExistsFor(_sqLiteProvider), Is.False);
        }

        [Test]
        public void GetAvailableProviders_NoProvidersSupported_ReturnsEmpty()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_stub.Out<ISettings>(), dataProviders);

            IEnumerable<IDataProviderInfo> availableProviders = databaseSettings.GetAvailableProviders();

            Assert.That(availableProviders.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetAvailableProviders_OneEmbeddedProviderWithoutSavedServerConnectString_ReturnsProvider()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_sqLiteProvider);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            IEnumerable<IDataProviderInfo> availableProviders = databaseSettings.GetAvailableProviders();

            Assert.That(availableProviders.First(), Is.EqualTo(_sqLiteProvider));
        }

        [Test]
        public void GetAvailableProviders_OneExternalProviderWithoutSavedServerConnectString_ReturnsEmpty()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_mySqlProvider);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            IEnumerable<IDataProviderInfo> availableProviders = databaseSettings.GetAvailableProviders();

            Assert.That(availableProviders.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetAvailableProviders_OneExternalProviderWithSavedServerConnectString_ReturnsProvider()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_mySqlProvider);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            _mockSettings.AppSettings
                .Add(ServerSettingsUtility.GetServerConnectKeyFor(_mySqlProvider), "someServerConnectString");

            IEnumerable<IDataProviderInfo> availableProviders = databaseSettings.GetAvailableProviders();

            Assert.That(availableProviders.First(), Is.EqualTo(_mySqlProvider));
        }

        [Test]
        public void GetConnectionStringFor_ConnectionStringForOtherProviderWasAdded_ReturnsEmptyString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            _mockSettings
                .ConnectionStrings.Add(new ConnectionStringSettings(_mySqlProvider.FullName, "someConnectionString"));

            string connectionString = databaseSettings.GetConnectionStringFor(_sqLiteProvider);

            Assert.That(connectionString, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetConnectionStringFor_ConnectionStringForProviderWasAdded_ReturnsConnectionStringOfProvider()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            const string savedConnectionString = "someConnectionString";
            _mockSettings
                .ConnectionStrings.Add(new ConnectionStringSettings(_sqLiteProvider.FullName, savedConnectionString));

            string connectionString = databaseSettings.GetConnectionStringFor(_sqLiteProvider);

            Assert.That(connectionString, Is.EqualTo(savedConnectionString));
        }

        [Test]
        public void
            GetConnectionStringFor_ConnectionStringsForProviderAndAnotherProvidersWereAdded_ReturnsConnectionStringOfProvider
            ()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            const string savedConnectionStringForSqLite = "sqLiteConnectionString";

            _mockSettings
                .ConnectionStrings.Add(
                new ConnectionStringSettings(_sqLiteProvider.FullName, savedConnectionStringForSqLite));
            _mockSettings
                .ConnectionStrings.Add(new ConnectionStringSettings(_mySqlProvider.FullName, "mySqlConnectionString"));

            string connectionString = databaseSettings.GetConnectionStringFor(_sqLiteProvider);

            Assert.That(connectionString, Is.EqualTo(savedConnectionStringForSqLite));
        }

        [Test]
        public void GetConnectionStringFor_NoConnectionStringWasAdded_ReturnsEmptyString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            string connectionString = databaseSettings.GetConnectionStringFor(_sqLiteProvider);

            Assert.That(connectionString, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GetServerConnectStringFor_ServerConnectStringForProviderWasAdded_ReturnsServerConnectString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            string key = ServerSettingsUtility.GetServerConnectKeyFor(_mySqlProvider);
            const string savedServerConnectString = "someServerConnectString";
            _mockSettings
                .AppSettings.Add(key, savedServerConnectString);

            string serverConnectString = databaseSettings.GetServerConnectStringFor(_mySqlProvider);

            Assert.That(serverConnectString, Is.EqualTo(savedServerConnectString));
        }

        [Test]
        public void GetServerConnectStringFor_ServerConnectStringForProviderWasNotAdded_ReturnsEmptyString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            string serverConnectString = databaseSettings.GetServerConnectStringFor(_sqLiteProvider);

            Assert.That(serverConnectString, Is.EqualTo(string.Empty));
        }

        [Test]
        public void ProviderIsAvailable_EmbeddedProviderInSupportedProviders_ReturnsTrue()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_sqLiteProvider);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            Assert.That(databaseSettings.ProviderIsAvailable(_sqLiteProvider), Is.True);
        }

        [Test]
        public void ProviderIsAvailable_ExternalProviderWithoutServerConnectStringInSupportedProviders_ReturnsFalse()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_mySqlProvider);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            Assert.That(databaseSettings.ProviderIsAvailable(_mySqlProvider), Is.False);
        }

        [Test]
        public void ProviderIsAvailable_ExternalProviderWithServerConnectStringInSupportedProviders_ReturnsTrue()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_mySqlProvider);

            string key = ServerSettingsUtility.GetServerConnectKeyFor(_mySqlProvider);
            _mockSettings
                .AppSettings.Add(key, "someServerConnectString");

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            Assert.That(databaseSettings.ProviderIsAvailable(_mySqlProvider), Is.True);
        }

        [Test]
        public void ProviderIsAvailable_NoProviderInSupportedProviders_ReturnsFalse()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.ProviderIsAvailable(_sqLiteProvider), Is.False);
        }

        [Test]
        public void ProviderIsAvailable_OtherProviderInSupportedProviders_ReturnsFalse()
        {
            IDataProviderInfos dataProviderInfos = new DataProviderInfos()
                .Support(_mySqlProvider);
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviderInfos);

            Assert.That(databaseSettings.ProviderIsAvailable(_sqLiteProvider), Is.False);
        }

        [Test]
        public void SetConnectionStringFor_DataProviderConnectionStringAlready_SetsItToNewConnectionString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            const string previousConnectionString = "previousConnectionString";
            databaseSettings.SetConnectionStringFor(_sqLiteProvider, previousConnectionString);

            const string connectionString = "connectionString";
            databaseSettings.SetConnectionStringFor(_sqLiteProvider, connectionString);

            Assert.That(
                _mockSettings.ConnectionStrings[_sqLiteProvider.FullName].ConnectionString, Is.EqualTo(connectionString));
        }

        [Test]
        public void SetConnectionStringFor_DataProviderHasNoConnectionStringYet_AddsConnectionStringForDataProvider()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            const string connectionString = "connectionString";
            databaseSettings.SetConnectionStringFor(_sqLiteProvider, connectionString);

            Assert.That(
                _mockSettings.ConnectionStrings[_sqLiteProvider.FullName].ConnectionString, Is.EqualTo(connectionString));
        }

        [Test]
        public void SetCurrentDataProviderTo_ValidProvider_SetsCurrentDataProviderToProvider()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            databaseSettings.SetCurrentDataProviderTo(_sqLiteProvider);

            string key = ServerSettings.ProviderName.ToString();
            Assert.That(_mockSettings.AppSettings[key].Value, Is.EqualTo(_sqLiteProvider.FullName));
        }

        [Test]
        public void SetServerConnectStringFor_ValidProviderAndConnectString_SetsServerConnectString()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            const string serverConnectString = "serverConnectString";
            databaseSettings.SetServerConnectStringFor(_sqLiteProvider, serverConnectString);

            string key = ServerSettingsUtility.GetServerConnectKeyFor(_sqLiteProvider);

            Assert.That(_mockSettings.AppSettings[key].Value, Is.EqualTo(serverConnectString));
        }

        [Test]
        public void GetCurrentDataProvider_CurrentDataProviderNotSet_ReturnsNull()
        {
            var dataProviders = new DataProviderInfos();
            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.GetCurrentDataProvider(), Is.EqualTo(null));
        }

        [Test]
        public void GetCurrentDataProvider_CurrentDataProviderSetAndSupported_ReturnsCurrentDataProvider()
        {
            var dataProviders = new DataProviderInfos()
                .Support(_sqLiteProvider);
            _mockSettings
                .AppSettings.Add(ServerSettings.ProviderName.ToString(), _sqLiteProvider.FullName);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.GetCurrentDataProvider(), Is.EqualTo(_sqLiteProvider));
        }

        [Test]
        public void GetCurrentDataProvider_CurrentDataProviderSetButNotSupported_ReturnsNull()
        {
            var dataProviders = new DataProviderInfos();

            _mockSettings
                .AppSettings.Add(ServerSettings.ProviderName.ToString(), _sqLiteProvider.FullName);

            var databaseSettings = new DatabaseSettings(_mockSettings, dataProviders);

            Assert.That(databaseSettings.GetCurrentDataProvider(), Is.EqualTo(null));
        }

        #endregion
    }
}