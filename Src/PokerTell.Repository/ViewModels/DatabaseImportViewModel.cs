namespace PokerTell.Repository.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.Repository.Interfaces;
    using PokerTell.Repository.Properties;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class DatabaseImportViewModel : NotifyPropertyChanged, IDatabaseImportViewModel
    {
        readonly IDatabaseConnector _databaseConnector;

        readonly IDatabaseImporter _databaseImporter;

        readonly IDatabaseSettings _databaseSettings;

        readonly IDataProviderInfos _dataProviderInfos;

        readonly IEmbeddedManagedDatabase _embeddedManagedDatabase;

        readonly IEventAggregator _eventAggregator;

        readonly IExternalManagedDatabase _externalManagedDatabase;

        protected IManagedDatabase _currentManagedDatabase;

        ICommand _importDatabaseCommand;

        PokerStatisticsApplications _selectedApplication;

        IDataProviderInfo _selectedDataProviderInfo;

        public DatabaseImportViewModel(
            IEventAggregator eventAggregator, 
            IDatabaseImporter databaseImporter, 
            IDataProviderInfos dataProviderInfos, 
            IDatabaseSettings databaseSettings, 
            IDatabaseConnector databaseConnector, 
            IExternalManagedDatabase externalManagedDatabase, 
            IEmbeddedManagedDatabase embeddedManagedDatabase)
        {
            _eventAggregator = eventAggregator;
            _databaseImporter = databaseImporter;
            _dataProviderInfos = dataProviderInfos;
            _databaseSettings = databaseSettings;
            _databaseConnector = databaseConnector;
            _externalManagedDatabase = externalManagedDatabase;
            _embeddedManagedDatabase = embeddedManagedDatabase;

            DataProvidersInfos = new ObservableCollection<IDataProviderInfo>();
            DatabaseNames = new ObservableCollection<string>();

            NotCurrentlyImporting = true;

            SelectedApplication = PokerStatisticsApplications.PokerTell;

            RegisterEvents();
        }

        void RegisterEvents()
        {
            _databaseImporter.IsBusyChanged += isBusy => NotCurrentlyImporting = !isBusy;
        }

        public IList<string> DatabaseNames { get; protected set; }

        string _selectedDatabaseName;

        public string SelectedDatabaseName
        {
            get { return _selectedDatabaseName; }
            set
            {
                _selectedDatabaseName = value;
                RaisePropertyChanged(() => SelectedDatabaseName);
            }
        }

        bool _notCurrentlyImporting;

        public bool NotCurrentlyImporting
        {
            get { return _notCurrentlyImporting; }
            protected set
            {
                _notCurrentlyImporting = value;
                RaisePropertyChanged(() => NotCurrentlyImporting);
            }
        }

        public IList<IDataProviderInfo> DataProvidersInfos { get; protected set; }

        public ICommand ImportDatabaseCommand
        {
            get
            {
                return _importDatabaseCommand ?? (_importDatabaseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _currentManagedDatabase.ChooseDatabase(SelectedDatabaseName);
                            _databaseImporter.ImportFrom(SelectedApplication, SelectedDatabaseName, _currentManagedDatabase.DataProvider);
                        }, 
                        CanExecuteDelegate = arg => SelectedDataProviderInfo != null && SelectedDatabaseName != null && ! _databaseImporter.IsBusy
                    });
            }
        }

        public PokerStatisticsApplications SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                RaisePropertyChanged(() => SelectedApplication);

                ShowTheAvailableDataProvidersForTheSelectedApplication();
            }
        }

        public IDataProviderInfo SelectedDataProviderInfo
        {
            get { return _selectedDataProviderInfo; }
            set
            {
                _selectedDataProviderInfo = value;

                DatabaseNames.Clear();

                if (SelectedDataProviderInfo != null)
                {
                    ConnectToServerOfSelectedDataProvider();

                    if (SelectedApplication == PokerStatisticsApplications.PokerTell)
                        InitializeManagedDatabaseAndAddPokerTellDatabaseNames();
                    else
                    {
                        InitializeExternalManagedDatabase();
                        switch (SelectedApplication)
                        {
                            case PokerStatisticsApplications.PokerOffice:
                                AddPokerOfficeDatabaseNames();
                                break;
                            case PokerStatisticsApplications.PokerTracker:
                                AddPokerTrackerDatabaseNames();
                                break;
                        }
                    }
                }

                RaisePropertyChanged(() => SelectedDataProviderInfo);
            }
        }

        public IList<PokerStatisticsApplications> SupportedApplications
        {
            get { return new List<PokerStatisticsApplications> { PokerStatisticsApplications.PokerTell, PokerStatisticsApplications.PokerOffice, PokerStatisticsApplications.PokerTracker }; }
        }

        void AddPokerOfficeDatabaseNames()
        {
            _externalManagedDatabase
                .GetAllPokerOfficeDatabaseNames()
                .ForEach(name => DatabaseNames.Add(name));
        }

        void AddPokerTrackerDatabaseNames()
        {
            _externalManagedDatabase
                .GetAllPokerTrackerDatabaseNames()
                .ForEach(name => DatabaseNames.Add(name));
        }

        void ConnectToServerOfSelectedDataProvider()
        {
            _databaseConnector
                .InitializeWith(SelectedDataProviderInfo)
                .ConnectToServer();
        }

        void InitializeExternalManagedDatabase()
        {
            _currentManagedDatabase = _externalManagedDatabase
                .InitializeWith(_databaseConnector.DataProvider, SelectedDataProviderInfo);
        }

        void InitializeManagedDatabaseAndAddPokerTellDatabaseNames()
        {
            if (SelectedDataProviderInfo.IsEmbedded)
            {
                _embeddedManagedDatabase
                    .InitializeWith(_databaseConnector.DataProvider, SelectedDataProviderInfo)
                    .GetAllPokerTellDatabaseNames()
                    .ForEach(name => DatabaseNames.Add(name));
                _currentManagedDatabase = _embeddedManagedDatabase;
            }
            else
            {
                _externalManagedDatabase
                    .InitializeWith(_databaseConnector.DataProvider, SelectedDataProviderInfo)
                    .GetAllPokerTellDatabaseNames()
                    .ForEach(name => DatabaseNames.Add(name));
                _currentManagedDatabase = _externalManagedDatabase;
            }

            RaisePropertyChanged(() => DatabaseNames);
        }

        void ShowTheAvailableDataProvidersForTheSelectedApplication()
        {
            DataProvidersInfos.Clear();

            switch (SelectedApplication)
            {
                case PokerStatisticsApplications.PokerOffice:
                    {
                        var providerForPokerOffice = _dataProviderInfos.MySqlProviderInfo;

                        if (_databaseSettings.ProviderIsAvailable(providerForPokerOffice))
                            DataProvidersInfos.Add(providerForPokerOffice);
                        else
                            ShowUserWarningAboutUnavailableDataProvider(providerForPokerOffice.NiceName);

                        break;
                    }

                case PokerStatisticsApplications.PokerTracker:
                    {
                        var providerForPokerTracker = _dataProviderInfos.PostgresProviderInfo;

                        if (_databaseSettings.ProviderIsAvailable(providerForPokerTracker))
                            DataProvidersInfos.Add(providerForPokerTracker);
                        else
                            ShowUserWarningAboutUnavailableDataProvider(providerForPokerTracker.NiceName);

                        break;
                    }

                case PokerStatisticsApplications.PokerTell:
                    {
                        _databaseSettings.GetAvailableProviders()
                            .ForEach(dp => DataProvidersInfos.Add(dp));

                        break;
                    }
            }

            SelectedDataProviderInfo = DataProvidersInfos.FirstOrDefault();
        }

        void ShowUserWarningAboutUnavailableDataProvider(string providerName)
        {
            var msg = string.Format(Resources.Warning_DataProviderUnavailable, providerName, SelectedApplication);
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Publish(new UserMessageEventArgs(UserMessageTypes.Warning, msg));
        }
    }
}