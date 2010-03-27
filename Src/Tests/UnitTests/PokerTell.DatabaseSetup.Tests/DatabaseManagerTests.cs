namespace PokerTell.DatabaseSetup.Tests
{
    using Infrastructure;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Moq;

    using NUnit.Framework;

    // Resharper disable InconsistentNaming
    public class DatabaseManagerTests
    {
        StubBuilder _stub;

        const string DatabaseName = "SomeDatabaseName";
        const string ConnectionString = "SomeConnectionString";

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
        }

        [Test]
        public void ChooseDatabase_DatabaseDoesNotExist_ThrowsDatabaseDoesNotExistException()
        {
            const bool databaseExists = false;
            var databaseStub = new Mock<IManagedDatabase>();
            databaseStub.Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseStub.Object, _stub.Out<IDatabaseSettings>());

            Assert.Throws<DatabaseDoesNotExistException>(() => manager.ChooseDatabase(DatabaseName));
        }

        [Test]
        public void ChooseDatabase_DatabaseExists_InvokesChooseDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = true;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.ChooseDatabase(DatabaseName);

            databaseMock.Verify(db => db.ChooseDatabase(DatabaseName));
        }

        [Test]
        public void ChooseDatabase_DatabaseExists_SetsConnectionStringFromManagedDatabase()
        {
            const bool databaseExists = true;
            var databaseStub = new Mock<IManagedDatabase>();
            databaseStub
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);
            databaseStub
                .SetupGet(db => db.ConnectionString).Returns(ConnectionString);

            var settingsMock = new Mock<IDatabaseSettings>();

            var manager = new DatabaseManager(databaseStub.Object, settingsMock.Object);

            manager.ChooseDatabase(DatabaseName);

            settingsMock.Verify(ds => ds.SetConnectionStringFor(It.IsAny<IDataProviderInfo>(), ConnectionString));
        }

        [Test]
        public void ClearDatabase_DatabaseDoesNotExist_ThrowsDatabaseDoesNotExistException()
        {
            const bool databaseExists = false;
            var databaseStub = new Mock<IManagedDatabase>();
            databaseStub.Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseStub.Object, _stub.Out<IDatabaseSettings>());

            Assert.Throws<DatabaseDoesNotExistException>(() => manager.ClearDatabase(DatabaseName));
        }

        [Test]
        public void ClearDatabase_DatabaseExists_InvokesChooseDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = true;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.ClearDatabase(DatabaseName);

            databaseMock.Verify(db => db.ChooseDatabase(DatabaseName));
        }

        [Test]
        public void ClearDatabase_DatabaseExists_InvokesCreateTablesOnManagedDatabase()
        {
            const bool databaseExists = true;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.ClearDatabase(DatabaseName);

            databaseMock.Verify(db => db.CreateTables());
        }

        [Test]
        public void ClearDatabase_DatabaseExists_InvokesVersionDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = true;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.ClearDatabase(DatabaseName);

            databaseMock.Verify(db => db.VersionDatabase(DatabaseName));
        }

        [Test]
        public void CreateDatabase_DatabaseDoesExist_ThrowsDatabaseExistsException()
        {
            const bool databaseExists = true;
            var databaseStub = new Mock<IManagedDatabase>();
            databaseStub.Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseStub.Object, _stub.Out<IDatabaseSettings>());

            Assert.Throws<DatabaseExistsException>(() => manager.CreateDatabase(DatabaseName));
        }

        [Test]
        public void CreateDatabase_DatabaseDoesNotExist_InvokesCreateDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = false;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.CreateDatabase(DatabaseName);

            databaseMock.Verify(db => db.CreateDatabase(DatabaseName));
        }

        [Test]
        public void CreateDatabase_DatabaseDoesNotExist_InvokesCreateTablesOnManagedDatabase()
        {
            const bool databaseExists = false;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.CreateDatabase(DatabaseName);

            databaseMock.Verify(db => db.CreateTables());
        }

        [Test]
        public void CreateDatabase_DatabaseDoesNotExist_InvokesVersionDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = false;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.CreateDatabase(DatabaseName);

            databaseMock.Verify(db => db.VersionDatabase(DatabaseName));
        }

        [Test]
        public void DeleteDatabase_DatabaseDoesNotExist_ThrowsDatabaseDoesNotExistException()
        {
            const bool databaseExists = false;
            var databaseStub = new Mock<IManagedDatabase>();
            databaseStub.Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseStub.Object, _stub.Out<IDatabaseSettings>());

            Assert.Throws<DatabaseDoesNotExistException>(() => manager.DeleteDatabase(DatabaseName));
        }

        [Test]
        public void DeleteDatabase_DatabaseExists_InvokesDeleteDatabaseOnManagedDatabaseWithDatabaseName()
        {
            const bool databaseExists = true;
            var databaseMock = new Mock<IManagedDatabase>();
            databaseMock
                .Setup(db => db.DatabaseExists(DatabaseName)).Returns(databaseExists);

            var manager = new DatabaseManager(databaseMock.Object, _stub.Out<IDatabaseSettings>());

            manager.DeleteDatabase(DatabaseName);

            databaseMock.Verify(db => db.DeleteDatabase(DatabaseName));
        }

        [Test]
        public void GetDatabaseInUse_SettingsReturnNullConnectionString_ReturnsNull()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.GetConnectionStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns<string>(null);

            var managedDatabase_Stub = new Mock<IManagedDatabase>();
            managedDatabase_Stub.SetupGet(md => md.DataProviderInfo).Returns(new Mock<IDataProviderInfo>().Object);
            
            var sut = new DatabaseManager(managedDatabase_Stub.Object, settingsStub.Object);

            Assert.That(sut.GetDatabaseInUse(), Is.Null);
        }

        [Test]
        public void GetDatabaseInUse_SettingsReturnInvalidConnectionString_ReturnsNull()
        {
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.GetConnectionStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns("invalidConnectionString");

            var managedDatabase_Stub = new Mock<IManagedDatabase>();
            managedDatabase_Stub.SetupGet(md => md.DataProviderInfo).Returns(new Mock<IDataProviderInfo>().Object);

            var sut = new DatabaseManager(managedDatabase_Stub.Object, settingsStub.Object);

            Assert.That(sut.GetDatabaseInUse(), Is.Null);
        }

        [Test]
        public void GetDatabaseInUse_SettingsReturnValidConnectionString_ReturnsDatabaseName()
        {
            const string databaseName = "databaseName";
           
            string validConnectionString = string.Format(
                "data source=localhost; user id = root; database = {0};", databaseName);
          
            var settingsStub = new Mock<IDatabaseSettings>();
            settingsStub
                .Setup(ds => ds.GetConnectionStringFor(It.IsAny<IDataProviderInfo>()))
                .Returns(validConnectionString);

            var managedDatabase_Stub = new Mock<IManagedDatabase>();
            managedDatabase_Stub.SetupGet(md => md.DataProviderInfo).Returns(new Mock<IDataProviderInfo>().Object);

            var sut = new DatabaseManager(managedDatabase_Stub.Object, settingsStub.Object);

            Assert.That(sut.GetDatabaseInUse(), Is.EqualTo(databaseName));
        }
    }
}