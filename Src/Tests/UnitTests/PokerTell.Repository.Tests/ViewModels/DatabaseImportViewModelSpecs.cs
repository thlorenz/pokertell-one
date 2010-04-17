namespace PokerTell.Repository.Tests.ViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Repository.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class DatabaseImportViewModelSpecs
    {
        static Mock<IDataProviderInfo> _sqliteInfo_Stub;
        static Mock<IDataProviderInfo> _mySqlInfo_Stub;
        static Mock<IDataProviderInfo> _postgresInfo_Stub;
        static Mock<IDataProviderInfos> _dataProviderInfos_Stub;

        static Mock<IDatabaseSettings> _databaseSettings_Stub;

        static Mock<IDatabaseConnector> _databaseConnector_Mock;

        static Mock<IExternalManagedDatabase> _externalManagedDatabase_Mock;

        static Mock<IEmbeddedManagedDatabase> _embeddedManagedDatabase_Mock;

        const string FirstEmbeddedDatabase = "FirstEmbeddedDatabase";
        const string SecondEmbeddedDatabase = "SecondEmbeddedDatabase";

        static IEnumerable<string> _sqliteDatabaseNames_Stub;

        static DatabaseImportViewModel _sut;

        Establish specContext = () => {
            _sqliteInfo_Stub = new Mock<IDataProviderInfo>();
            _sqliteInfo_Stub
                .SetupGet(dbi => dbi.IsEmbedded)
                .Returns(true);

            _mySqlInfo_Stub = new Mock<IDataProviderInfo>();
            _mySqlInfo_Stub
                .SetupGet(dbi => dbi.IsEmbedded)
                .Returns(false);

            _postgresInfo_Stub = new Mock<IDataProviderInfo>();
            _postgresInfo_Stub
                .SetupGet(dbi => dbi.IsEmbedded)
                .Returns(false);

            _dataProviderInfos_Stub = new Mock<IDataProviderInfos>();
            _dataProviderInfos_Stub
                .SetupGet(dpi => dpi.SQLiteProviderInfo)
                .Returns(_sqliteInfo_Stub.Object);
            _dataProviderInfos_Stub
                .SetupGet(dpi => dpi.MySqlProviderInfo)
                .Returns(_mySqlInfo_Stub.Object);
            _dataProviderInfos_Stub
                .SetupGet(dpi => dpi.PostgresProviderInfo)
                .Returns(_postgresInfo_Stub.Object);

            _databaseSettings_Stub = new Mock<IDatabaseSettings>();
            _databaseSettings_Stub
                .Setup(dbs => dbs.GetAvailableProviders())
                .Returns(new[] { _sqliteInfo_Stub.Object, _mySqlInfo_Stub.Object, _postgresInfo_Stub.Object });

            _databaseConnector_Mock = new Mock<IDatabaseConnector>();
            _databaseConnector_Mock
                .Setup(dbc => dbc.InitializeWith(Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_databaseConnector_Mock.Object);
            _databaseConnector_Mock
                .Setup(dbc => dbc.ConnectToServer())
                .Returns(_databaseConnector_Mock.Object);

            _sqliteDatabaseNames_Stub = new[] { FirstEmbeddedDatabase, SecondEmbeddedDatabase };

            _externalManagedDatabase_Mock = new Mock<IExternalManagedDatabase>();
            _externalManagedDatabase_Mock .Setup(exdb => exdb.InitializeWith(Moq.It.IsAny<IDataProvider>(), Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_externalManagedDatabase_Mock .Object);
          
            _embeddedManagedDatabase_Mock = new Mock<IEmbeddedManagedDatabase>();
            _embeddedManagedDatabase_Mock
                .Setup(emdb => emdb.InitializeWith(Moq.It.IsAny<IDataProvider>(), Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_embeddedManagedDatabase_Mock.Object);
            _embeddedManagedDatabase_Mock
                .Setup(emdb => emdb.GetAllPokerTellDatabaseNames())
                .Returns(_sqliteDatabaseNames_Stub);

            _sut = new DatabaseImportViewModel(_dataProviderInfos_Stub.Object,
                                               _databaseSettings_Stub.Object,
                                               _databaseConnector_Mock.Object,
                                               _externalManagedDatabase_Mock.Object,
                                               _embeddedManagedDatabase_Mock.Object);
        };

        [Subject(typeof(DatabaseImportViewModel), "Instantiation")]
        public class when_instantiated : DatabaseImportViewModelSpecs
        {
            It should_show_all_supported_applications = () => {
                _sut.SupportedApplications.ShouldContain(DatabaseImportViewModel.ApplicationPokerTell);
                _sut.SupportedApplications.ShouldContain(DatabaseImportViewModel.ApplicationPokerOffice);
                _sut.SupportedApplications.ShouldContain(DatabaseImportViewModel.ApplicationPokerTracker);
            };

            It should_select_PokerTell = () => _sut.SelectedApplication.ShouldEqual(DatabaseImportViewModel.ApplicationPokerTell);

        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerOffice_and_the_settings_say_MySql_DataProvider_is_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = true;

            Establish context = () => _databaseSettings_Stub
                .Setup(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object))
                .Returns(providerIsAvailable);
            
            Because of = () => _sut.SelectedApplication = DatabaseImportViewModel.ApplicationPokerOffice;

            It should_ask_the_settings_if_MySql_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object));

            It should_show_only_the_MySql_DataProvider = () => {
                _sut.DataProvidersInfos.ShouldContain(_mySqlInfo_Stub.Object);
                _sut.DataProvidersInfos.Count.ShouldEqual(1);
            };

            It should_select_the_MySql_DataProvider = () => _sut.SelectedDataProviderInfo.ShouldEqual(_mySqlInfo_Stub.Object);
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerTracker_and_the_settings_say_Postgres_DataProvider_is_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = true;

            Establish context = () => _databaseSettings_Stub
                .Setup(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object))
                .Returns(providerIsAvailable);
            
            Because of = () => _sut.SelectedApplication = DatabaseImportViewModel.ApplicationPokerTracker;

            It should_ask_the_settings_if_Postgres_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object));

            It should_show_only_the_Postgres_DataProvider = () => {
                _sut.DataProvidersInfos.ShouldContain(_postgresInfo_Stub.Object);
                _sut.DataProvidersInfos.Count.ShouldEqual(1);
            };

            It should_select_the_Postgres_DataProvider = () => _sut.SelectedDataProviderInfo.ShouldEqual(_postgresInfo_Stub.Object);
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectedApplication")]
        public class when_the_user_selects_PokerTell_and_the_settings_say_that_MySql_Postgres_and_SQLite_Dataproviders_are_available : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.DataProvidersInfos.Clear();
                _sut.SelectedDataProviderInfo = null;
            };

            Because of = () => _sut.SelectedApplication = DatabaseImportViewModel.ApplicationPokerTell;

            It should_show_all_available_DataProviderInfos_supported_by_PokerTell = () => {
                _sut.DataProvidersInfos.ShouldContain(_sqliteInfo_Stub.Object);
                _sut.DataProvidersInfos.ShouldContain(_mySqlInfo_Stub.Object);
                _sut.DataProvidersInfos.ShouldContain(_postgresInfo_Stub.Object);
            };

            It should_select_the_first_DataProviderInfo_SQLite = () => _sut.SelectedDataProviderInfo.ShouldEqual(_sqliteInfo_Stub.Object);

            It should_show_SQLite_databases = () => {
                _sut.DatabaseNames.ShouldContain(FirstEmbeddedDatabase);
                _sut.DatabaseNames.ShouldContain(SecondEmbeddedDatabase);
            };
        }

    }
}