namespace PokerTell.Repository.Tests.ViewModels
{
    using System.Collections.Generic;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using PokerTell.Repository.ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class DatabaseImportViewModelSpecs
    {
        static IEventAggregator _eventAggregator;

        static Mock<IDatabaseImporter> _databaseImporter_Mock;

        static Mock<IDataProviderInfo> _sqliteInfo_Stub;
        static Mock<IDataProviderInfo> _mySqlInfo_Stub;
        static Mock<IDataProviderInfo> _postgresInfo_Stub;
        static Mock<IDataProviderInfos> _dataProviderInfos_Stub;

        static Mock<IDatabaseSettings> _databaseSettings_Stub;

        static Mock<IDataProvider> _dataProvider_Stub;

        static Mock<IDatabaseConnector> _databaseConnector_Mock;

        static Mock<IExternalManagedDatabase> _externalManagedDatabase_Mock;

        static Mock<IEmbeddedManagedDatabase> _embeddedManagedDatabase_Mock;

        const string FirstEmbeddedDatabase = "FirstEmbeddedDatabase";
        const string SecondEmbeddedDatabase = "SecondEmbeddedDatabase";

        const string FirstPokerOfficeDatabase = "FirstPokerOfficeDatabase";
        const string SecondPokerOfficeDatabase = "SecondPokerOfficeDatabase";

        const string FirstPokerTrackerDatabase = "FirstPokerTrackerDatabase";
        const string SecondPokerTrackerDatabase = "SecondPokerTrackerDatabase";

        static IEnumerable<string> _sqliteDatabaseNames_Stub;
        static IEnumerable<string> _pokerOfficeDatabaseNames_Stub;
        static IEnumerable<string> _pokerTrackerDatabaseNames_Stub;

        static DatabaseImportViewModelSut _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _databaseImporter_Mock = new Mock<IDatabaseImporter>();

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

            _dataProvider_Stub = new Mock<IDataProvider>();

            _databaseConnector_Mock = new Mock<IDatabaseConnector>();
            _databaseConnector_Mock
                .Setup(dbc => dbc.InitializeWith(Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_databaseConnector_Mock.Object);
            _databaseConnector_Mock
                .Setup(dbc => dbc.ConnectToServer())
                .Returns(_databaseConnector_Mock.Object);
            _databaseConnector_Mock
                .SetupGet(dbc => dbc.DataProvider)
                .Returns(_dataProvider_Stub.Object);

            _sqliteDatabaseNames_Stub = new[] { FirstEmbeddedDatabase, SecondEmbeddedDatabase };
            _pokerOfficeDatabaseNames_Stub = new[] { FirstPokerOfficeDatabase, SecondPokerOfficeDatabase };
            _pokerTrackerDatabaseNames_Stub = new[] { FirstPokerTrackerDatabase, SecondPokerTrackerDatabase };

            _externalManagedDatabase_Mock = new Mock<IExternalManagedDatabase>();
            _externalManagedDatabase_Mock 
                .Setup(exdb => exdb.InitializeWith(Moq.It.IsAny<IDataProvider>(), Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_externalManagedDatabase_Mock .Object);
            _externalManagedDatabase_Mock
                .Setup(exdb => exdb.GetAllPokerOfficeDatabaseNames())
                .Returns(_pokerOfficeDatabaseNames_Stub);
            _externalManagedDatabase_Mock
                .Setup(exdb => exdb.GetAllPokerTrackerDatabaseNames())
                .Returns(_pokerTrackerDatabaseNames_Stub);
          
            _embeddedManagedDatabase_Mock = new Mock<IEmbeddedManagedDatabase>();
            _embeddedManagedDatabase_Mock
                .Setup(emdb => emdb.InitializeWith(Moq.It.IsAny<IDataProvider>(), Moq.It.IsAny<IDataProviderInfo>()))
                .Returns(_embeddedManagedDatabase_Mock.Object);
            _embeddedManagedDatabase_Mock
                .Setup(emdb => emdb.GetAllPokerTellDatabaseNames())
                .Returns(_sqliteDatabaseNames_Stub);

            _sut = new DatabaseImportViewModelSut(_eventAggregator, 
                                               _databaseImporter_Mock.Object,
                                               _dataProviderInfos_Stub.Object,
                                               _databaseSettings_Stub.Object,
                                               _databaseConnector_Mock.Object,
                                               _externalManagedDatabase_Mock.Object,
                                               _embeddedManagedDatabase_Mock.Object);
        };

        [Subject(typeof(DatabaseImportViewModel), "Instantiation")]
        public class when_instantiated : DatabaseImportViewModelSpecs
        {
            It should_show_all_supported_applications = () => {
                _sut.SupportedApplications.ShouldContain(PokerStatisticsApplications.PokerTell);
                _sut.SupportedApplications.ShouldContain(PokerStatisticsApplications.PokerOffice);
                _sut.SupportedApplications.ShouldContain(PokerStatisticsApplications.PokerTracker);
            };

            It should_select_PokerTell = () => _sut.SelectedApplication.ShouldEqual(PokerStatisticsApplications.PokerTell);

        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerOffice_and_the_settings_say_MySql_DataProvider_is_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = true;

            Establish context = () => _databaseSettings_Stub
                .Setup(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object))
                .Returns(providerIsAvailable);
            
            Because of = () => _sut.SelectedApplication = PokerStatisticsApplications.PokerOffice;

            It should_ask_the_settings_if_MySql_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object));

            It should_show_only_the_MySql_DataProvider = () => {
                _sut.DataProvidersInfos.ShouldContain(_mySqlInfo_Stub.Object);
                _sut.DataProvidersInfos.Count.ShouldEqual(1);
            };

            It should_select_the_MySql_DataProvider = () => _sut.SelectedDataProviderInfo.ShouldEqual(_mySqlInfo_Stub.Object);

            It should_initialize_the_database_connector_with_the_MySql_DataProvider_Info
                = () => _databaseConnector_Mock.Verify(dbc => dbc.InitializeWith(_mySqlInfo_Stub.Object));

            It should_connect_the_database_connector_to_the_server = () => _databaseConnector_Mock.Verify(dbc => dbc.ConnectToServer());
            
            It should_initialize_the_external_managed_database_with_the_DataProvider_returned_by_the_database_connector_and_the_MySql_DataProviderInfo
                = () => _externalManagedDatabase_Mock.Verify(exdb => exdb.InitializeWith(_dataProvider_Stub.Object, _mySqlInfo_Stub.Object));

            It should_ask_the_external_managed_database_to_get_all_PokerOffice_database_names
                = () => _externalManagedDatabase_Mock.Verify(exdb => exdb.GetAllPokerOfficeDatabaseNames());

            It should_show_only_the_PokerOffice_database_names = () => {
                _sut.DatabaseNames.ShouldContain(FirstPokerOfficeDatabase);
                _sut.DatabaseNames.ShouldContain(SecondPokerOfficeDatabase);
                _sut.DatabaseNames.Count.ShouldEqual(2);
            };

            It should_set_the_current_managed_database_to_the_external_managed_database
                = () => _sut.CurrentManagedDatabase.ShouldEqual(_externalManagedDatabase_Mock.Object);
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerTracker_and_the_settings_say_Postgres_DataProvider_is_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = true;

            Establish context = () => _databaseSettings_Stub
                .Setup(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object))
                .Returns(providerIsAvailable);
            
            Because of = () => _sut.SelectedApplication = PokerStatisticsApplications.PokerTracker;

            It should_ask_the_settings_if_Postgres_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object));

            It should_show_only_the_Postgres_DataProvider = () => {
                _sut.DataProvidersInfos.ShouldContain(_postgresInfo_Stub.Object);
                _sut.DataProvidersInfos.Count.ShouldEqual(1);
            };

            It should_select_the_Postgres_DataProvider = () => _sut.SelectedDataProviderInfo.ShouldEqual(_postgresInfo_Stub.Object);

            It should_initialize_the_database_connector_with_the_Postgres_DataProvider_Info
                = () => _databaseConnector_Mock.Verify(dbc => dbc.InitializeWith(_postgresInfo_Stub.Object));

            It should_connect_the_database_connector_to_the_server = () => _databaseConnector_Mock.Verify(dbc => dbc.ConnectToServer());

            It should_initialize_the_external_managed_database_with_the_DataProvider_returned_by_the_database_connector_and_the_Postgres_DataProviderInfo
                = () => _externalManagedDatabase_Mock.Verify(exdb => exdb.InitializeWith(_dataProvider_Stub.Object, _postgresInfo_Stub.Object));

            It should_ask_the_external_managed_database_to_get_all_PokerTracker_database_names
                = () => _externalManagedDatabase_Mock.Verify(exdb => exdb.GetAllPokerTrackerDatabaseNames());

            It should_show_only_the_PokerTracker_database_names = () => {
                _sut.DatabaseNames.ShouldContain(FirstPokerTrackerDatabase);
                _sut.DatabaseNames.ShouldContain(SecondPokerTrackerDatabase);
                _sut.DatabaseNames.Count.ShouldEqual(2);
            };

            It should_set_the_current_managed_database_to_the_external_managed_database
                = () => _sut.CurrentManagedDatabase.ShouldEqual(_externalManagedDatabase_Mock.Object);
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectedApplication")]
        public class when_the_user_selects_PokerTell_and_the_settings_say_that_MySql_Postgres_and_SQLite_Dataproviders_are_available : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.DataProvidersInfos.Clear();
                _sut.SelectedDataProviderInfo = null;
            };

            Because of = () => _sut.SelectedApplication = PokerStatisticsApplications.PokerTell;

            It should_show_all_available_DataProviderInfos_supported_by_PokerTell = () => {
                _sut.DataProvidersInfos.ShouldContain(_sqliteInfo_Stub.Object);
                _sut.DataProvidersInfos.ShouldContain(_mySqlInfo_Stub.Object);
                _sut.DataProvidersInfos.ShouldContain(_postgresInfo_Stub.Object);
            };

            It should_select_the_first_DataProviderInfo_SQLite = () => _sut.SelectedDataProviderInfo.ShouldEqual(_sqliteInfo_Stub.Object);

            It should_initialize_the_database_connector_with_the_SQLite_DataProvider_Info
                = () => _databaseConnector_Mock.Verify(dbc => dbc.InitializeWith(_sqliteInfo_Stub.Object));

            It should_connect_the_database_connector_to_the_server = () => _databaseConnector_Mock.Verify(dbc => dbc.ConnectToServer());

            It should_initialize_the_embedded_managed_database_with_the_DataProvider_returned_by_the_database_connector_and_the_SQLite_DataProviderInfo
                = () => _embeddedManagedDatabase_Mock.Verify(emdb => emdb.InitializeWith(_dataProvider_Stub.Object, _sqliteInfo_Stub.Object));

            It should_ask_the_embedded_managed_database_to_get_all_PokerTell_database_names
                = () => _embeddedManagedDatabase_Mock.Verify(emdb => emdb.GetAllPokerTellDatabaseNames());

            It should_show_SQLite_databases = () => {
                _sut.DatabaseNames.ShouldContain(FirstEmbeddedDatabase);
                _sut.DatabaseNames.ShouldContain(SecondEmbeddedDatabase);
            };

            It should_set_the_current_managed_database_to_the_embedded_managed_database
                = () => _sut.CurrentManagedDatabase.ShouldEqual(_embeddedManagedDatabase_Mock.Object);
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerOffice_and_the_settings_say_MySql_DataProvider_is_not_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = false;

            static bool userWasWarned;

            Establish context = () => {
                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => userWasWarned = args.MessageType == UserMessageTypes.Warning);
                _databaseSettings_Stub
                    .Setup(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object))
                    .Returns(providerIsAvailable);
            };

            Because of = () => _sut.SelectedApplication = PokerStatisticsApplications.PokerOffice;

            It should_ask_the_settings_if_MySql_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_mySqlInfo_Stub.Object));

            It should_show_no_DataProvider = () => _sut.DataProvidersInfos.Count.ShouldEqual(0);

            It should_set_the_DataProvider_to_null = () => _sut.SelectedDataProviderInfo.ShouldEqual(null);

            It should_not_initialize_the_database_connector
                = () => _databaseConnector_Mock.Verify(dbc => dbc.InitializeWith(Moq.It.IsAny<IDataProviderInfo>()));

            It should_warn_the_user_and_inform_him_to_connect_the_MySql_data_provider_first
                = () => userWasWarned.ShouldBeTrue();
        }

        [Subject(typeof(DatabaseImportViewModel), "SelectApplication")]
        public class when_the_user_selects_PokerTracker_and_the_settings_say_Postgres_DataProvider_is_not_supported : DatabaseImportViewModelSpecs
        {
            const bool providerIsAvailable = false;
            static bool userWasWarned;

            Establish context = () => {
                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => userWasWarned = args.MessageType == UserMessageTypes.Warning);
                _databaseSettings_Stub
                    .Setup(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object))
                    .Returns(providerIsAvailable);
            };
            
            Because of = () => _sut.SelectedApplication = PokerStatisticsApplications.PokerTracker;

            It should_ask_the_settings_if_Postgres_DataProvider_is_available
                = () => _databaseSettings_Stub.Verify(dbs => dbs.ProviderIsAvailable(_postgresInfo_Stub.Object));

            It should_show_no_DataProvider = () => _sut.DataProvidersInfos.Count.ShouldEqual(0);

            It should_set_the_DataProvider_to_null = () => _sut.SelectedDataProviderInfo.ShouldEqual(null);

            It should_not_initialize_the_database_connector
                = () => _databaseConnector_Mock.Verify(dbc => dbc.InitializeWith(Moq.It.IsAny<IDataProviderInfo>()));

            It should_warn_the_user_and_inform_him_to_connect_the_Postgres_data_provider_first
                = () => userWasWarned.ShouldBeTrue();
        }

        [Subject(typeof(DatabaseImportViewModel), "ImportDatabaseCommand")]
        public class when_the_selected_DataProvider_is_non_null_the_selected_DatabaseName_is_non_null_and_the_DatabaseImporter_is_not_busy : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.SelectedDataProviderInfo = _sqliteInfo_Stub.Object;
                _sut.SelectedDatabaseName = "some Name";
                _databaseImporter_Mock
                    .SetupGet(dbi => dbi.IsBusy)
                    .Returns(false);
            };

            It should_be_possible_to_import_the_selected_database = () => _sut.ImportDatabaseCommand.CanExecute(null).ShouldBeTrue();
        }

        [Subject(typeof(DatabaseImportViewModel), "ImportDatabaseCommand")]
        public class when_the_selected_DataProvider_is_null_the_selected_DatabaseName_is_non_null_and_the_DatabaseImporter_is_not_busy : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.SelectedDataProviderInfo = null;
                _sut.SelectedDatabaseName = "some Name";
                _databaseImporter_Mock
                    .SetupGet(dbi => dbi.IsBusy)
                    .Returns(false);
            };

            It should_not_be_possible_to_import_the_selected_database = () => _sut.ImportDatabaseCommand.CanExecute(null).ShouldBeFalse();
        }

        [Subject(typeof(DatabaseImportViewModel), "ImportDatabaseCommand")]
        public class when_the_selected_DataProvider_is_non_null_the_selected_DatabaseName_is_null_and_the_DatabaseImporter_is_not_busy : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.SelectedDataProviderInfo = _sqliteInfo_Stub.Object;
                _sut.SelectedDatabaseName = null;
                _databaseImporter_Mock
                    .SetupGet(dbi => dbi.IsBusy)
                    .Returns(false);
            };

            It should_not_be_possible_to_import_the_selected_database = () => _sut.ImportDatabaseCommand.CanExecute(null).ShouldBeFalse();
        }

        [Subject(typeof(DatabaseImportViewModel), "ImportDatabaseCommand")]
        public class when_the_selected_DataProvider_is_non_null_the_selected_DatabaseName_is_non_null_but_the_DatabaseImporter_is_busy : DatabaseImportViewModelSpecs
        {
            Establish context = () => {
                _sut.SelectedDataProviderInfo = _sqliteInfo_Stub.Object;
                _sut.SelectedDatabaseName = "some Name";
                _databaseImporter_Mock
                    .SetupGet(dbi => dbi.IsBusy)
                    .Returns(true);
            };

            It should_not_be_possible_to_import_the_selected_database = () => _sut.ImportDatabaseCommand.CanExecute(null).ShouldBeFalse();
        }


        [Subject(typeof(DatabaseImportViewModel), "ImportDatabaseCommand")]
        public class when_the_import_database_command_is_executed_and_the_selected_application_is_PokerOFfice : DatabaseImportViewModelSpecs
        {
            static Mock<IDataProvider> currentDataProvider_Stub;
            static Mock<IManagedDatabase> currentManagedDatabase_Mock;
            const string selectedDatabaseName = "some Name";
 
            Establish context = () => {
                currentDataProvider_Stub = new Mock<IDataProvider>();
                currentManagedDatabase_Mock = new Mock<IManagedDatabase>();
                currentManagedDatabase_Mock
                    .SetupGet(mdb => mdb.DataProvider)
                    .Returns(currentDataProvider_Stub.Object);

                _sut.SelectedApplication = PokerStatisticsApplications.PokerOffice;
                _sut.CurrentManagedDatabase = currentManagedDatabase_Mock.Object;
                _sut.SelectedDatabaseName = selectedDatabaseName;
            };
            
            Because of = () => _sut.ImportDatabaseCommand.Execute(null);

            It should_tell_the_current_managed_database_to_choose_the_database_with_the_selected_database_name
                = () => currentManagedDatabase_Mock.Verify(mdb => mdb.ChooseDatabase(selectedDatabaseName));

            It should_tell_the_database_importer_to_import_PokerOffice_data_from_the_seleted_database_using_the_dataprovider_of_the_current_managed_database
                = () => _databaseImporter_Mock.Verify(dbi => dbi.ImportFrom(PokerStatisticsApplications.PokerOffice, selectedDatabaseName, currentDataProvider_Stub.Object));
        }

        [Subject(typeof(DatabaseImportViewModel), "Database importer busy state")]
        public class when_not_currently_importing_and_the_database_importer_says_he_got_busy : DatabaseImportViewModelSpecs
        {
            const bool notCurrentlyImporting = true;
            const bool isBusy = true;

            Establish context = () => _sut.Set_NotCurrentlyImporting(notCurrentlyImporting);

            Because of = () => _databaseImporter_Mock.Raise(dbi => dbi.IsBusyChanged += null, isBusy);

            It should_set_NotCurrentlyImporting_to_false = () => _sut.NotCurrentlyImporting.ShouldBeFalse();
        }

        [Subject(typeof(DatabaseImportViewModel), "Database importer busy state")]
        public class when_currently_importing_and_the_database_importer_says_he_is_not_busy_anymore : DatabaseImportViewModelSpecs
        {
            const bool notCurrentlyImporting = false;
            const bool isBusy = false;

            Establish context = () => _sut.Set_NotCurrentlyImporting(notCurrentlyImporting);

            Because of = () => _databaseImporter_Mock.Raise(dbi => dbi.IsBusyChanged += null, isBusy);

            It should_set_NotCurrentlyImporting_to_true = () => _sut.NotCurrentlyImporting.ShouldBeTrue();
        }
    }

    public class DatabaseImportViewModelSut : DatabaseImportViewModel
    {
        public DatabaseImportViewModelSut(IEventAggregator eventAggregator, IDatabaseImporter databaseImporter, IDataProviderInfos dataProviderInfos, IDatabaseSettings databaseSettings, IDatabaseConnector databaseConnector, IExternalManagedDatabase externalManagedDatabase, IEmbeddedManagedDatabase embeddedManagedDatabase)
            : base(eventAggregator, databaseImporter, dataProviderInfos, databaseSettings, databaseConnector, externalManagedDatabase, embeddedManagedDatabase)
        {
        }

        public DatabaseImportViewModelSut Set_NotCurrentlyImporting(bool notCurrentlyImporting)
        {
            NotCurrentlyImporting = notCurrentlyImporting;;
            return this;
        }

        public IManagedDatabase CurrentManagedDatabase
        {
            get { return _currentManagedDatabase; }
            set { _currentManagedDatabase = value; }
        }

    }
}