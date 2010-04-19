namespace PokerTell.Repository.Design
{
    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Repository.ViewModels;

    public class DatabaseImportDesignModel : DatabaseImportViewModel
    {
        public DatabaseImportDesignModel()
            : base(new EventAggregator(), new Mock<IDatabaseImporter>().Object, DataProviderInfos, DatabaseSettings, DatabaseConnector, ExternalManagedDatabase, EmbeddedManagedDatabase)
        {
        }

        static IDatabaseConnector DatabaseConnector
        {
            get
            {
                var databaseConnectorStub = new Mock<IDatabaseConnector>();
                databaseConnectorStub
                    .Setup(dbc => dbc.InitializeWith(It.IsAny<IDataProviderInfo>()))
                    .Returns(databaseConnectorStub.Object);
                databaseConnectorStub
                    .Setup(dbc => dbc.ConnectToServer())
                    .Returns(databaseConnectorStub.Object);

                return databaseConnectorStub.Object;
            }
        }

        static IDatabaseSettings DatabaseSettings
        {
            get
            {
                var databaseSettingsStub = new Mock<IDatabaseSettings>();
                databaseSettingsStub
                    .Setup(dbs => dbs.GetAvailableProviders())
                    .Returns(new[] { SQLiteInfo, MySqlInfo, PostgresInfo });
                databaseSettingsStub
                    .Setup(dbs => dbs.ProviderIsAvailable(It.IsAny<IDataProviderInfo>()))
                    .Returns(true);
                return databaseSettingsStub.Object;
            }
        }

        static IDataProviderInfos DataProviderInfos
        {
            get
            {
                var dataProviderInfosStub = new Mock<IDataProviderInfos>();
                dataProviderInfosStub
                    .SetupGet(dpi => dpi.SQLiteProviderInfo)
                    .Returns(SQLiteInfo);
                dataProviderInfosStub
                    .SetupGet(dpi => dpi.MySqlProviderInfo)
                    .Returns(MySqlInfo);
                dataProviderInfosStub
                    .SetupGet(dpi => dpi.PostgresProviderInfo)
                    .Returns(PostgresInfo);

                return dataProviderInfosStub.Object;
            }
        }

        static IEmbeddedManagedDatabase EmbeddedManagedDatabase
        {
            get
            {
                const string firstEmbeddedDatabase = "FirstEmbeddedDatabase";
                const string secondEmbeddedDatabase = "SecondEmbeddedDatabase";
                var sqliteDatabaseNamesStub = new[] { firstEmbeddedDatabase, secondEmbeddedDatabase };
                var embeddedManagedDatabaseStub = new Mock<IEmbeddedManagedDatabase>();
                embeddedManagedDatabaseStub
                    .Setup(emdb => emdb.InitializeWith(It.IsAny<IDataProvider>(), It.IsAny<IDataProviderInfo>()))
                    .Returns(embeddedManagedDatabaseStub.Object);
                embeddedManagedDatabaseStub
                    .Setup(emdb => emdb.GetAllPokerTellDatabaseNames())
                    .Returns(sqliteDatabaseNamesStub);

                return embeddedManagedDatabaseStub.Object;
            }
        }

        static IExternalManagedDatabase ExternalManagedDatabase
        {
            get
            {
                var externalManagedDatabaseStub = new Mock<IExternalManagedDatabase>();
                externalManagedDatabaseStub.Setup(exdb => exdb.InitializeWith(It.IsAny<IDataProvider>(), It.IsAny<IDataProviderInfo>()))
                    .Returns(externalManagedDatabaseStub.Object);

                return externalManagedDatabaseStub.Object;
            }
        }

        static IDataProviderInfo MySqlInfo
        {
            get
            {
                var mySqlInfoStub = new Mock<IDataProviderInfo>();
                mySqlInfoStub
                    .SetupGet(dbi => dbi.IsEmbedded)
                    .Returns(false);
                mySqlInfoStub
                    .SetupGet(dbi => dbi.NiceName)
                    .Returns("MySql");

                return mySqlInfoStub.Object;
            }
        }

        static IDataProviderInfo PostgresInfo
        {
            get
            {
                var postgresInfoStub = new Mock<IDataProviderInfo>();
                postgresInfoStub
                    .SetupGet(dbi => dbi.IsEmbedded)
                    .Returns(false);
                postgresInfoStub
                    .SetupGet(dbi => dbi.NiceName)
                    .Returns("Postgres");

                return postgresInfoStub.Object;
            }
        }

        static IDataProviderInfo SQLiteInfo
        {
            get
            {
                var sqliteInfoStub = new Mock<IDataProviderInfo>();
                sqliteInfoStub
                    .SetupGet(dbi => dbi.IsEmbedded)
                    .Returns(true);
                sqliteInfoStub
                    .SetupGet(dbi => dbi.NiceName)
                    .Returns("SQLite");

                return sqliteInfoStub.Object;
            }
        }
    }

    public static class DatabaseImportDesign
    {
        public static DatabaseImportViewModel Model
        {
            get { return new DatabaseImportDesignModel(); }
        }
    }
}