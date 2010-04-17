namespace PokerTell.Repository.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF.ViewModels;
    using Tools.FunctionalCSharp;

    public class DatabaseImportViewModel : NotifyPropertyChanged
    {
        public const string ApplicationPokerOffice = "PokerOffice";

        public const string ApplicationPokerTell = "PokerTell";

        public const string ApplicationPokerTracker = "PokerTracker";

        readonly IDatabaseConnector _databaseConnector;

        readonly IDatabaseSettings _databaseSettings;

        readonly IEmbeddedManagedDatabase _embeddedManagedDatabase;

        readonly IExternalManagedDatabase _externalManagedDatabase;

        string _selectedApplication;

        IDataProviderInfo _selectedDataProviderInfo;

        readonly IDataProviderInfos _dataProviderInfos;

        public DatabaseImportViewModel(
            IDataProviderInfos dataProviderInfos,
            IDatabaseSettings databaseSettings, 
            IDatabaseConnector databaseConnector, 
            IExternalManagedDatabase externalManagedDatabase, 
            IEmbeddedManagedDatabase embeddedManagedDatabase)
        {
            _dataProviderInfos = dataProviderInfos;
            _databaseSettings = databaseSettings;
            _databaseConnector = databaseConnector;
            _externalManagedDatabase = externalManagedDatabase;
            _embeddedManagedDatabase = embeddedManagedDatabase;

            DataProvidersInfos = new ObservableCollection<IDataProviderInfo>();
            DatabaseNames = new ObservableCollection<string>();

            SelectedApplication = ApplicationPokerTell;
        }

        void UpdatePokerTellDatabaseNamesForSelectedDataProvider()
        {
            DatabaseNames.Clear();

            _databaseConnector
                .InitializeWith(SelectedDataProviderInfo)
                .ConnectToServer();

            if (SelectedDataProviderInfo.IsEmbedded)
            {
                _embeddedManagedDatabase
                    .InitializeWith(_databaseConnector.DataProvider, SelectedDataProviderInfo)
                    .GetAllPokerTellDatabaseNames()
                    .ForEach(name => DatabaseNames.Add(name));
            }
            else
            {
                _externalManagedDatabase
                    .InitializeWith(_databaseConnector.DataProvider, SelectedDataProviderInfo)
                    .GetAllPokerTellDatabaseNames()
                    .ForEach(name => DatabaseNames.Add(name));
            }
        }

        public IList<IDataProviderInfo> DataProvidersInfos { get; protected set; }

        public IList<string> DatabaseNames { get; protected set; }

        public string SelectedApplication
        {
            get { return _selectedApplication; }
            set
            {
                _selectedApplication = value;
                RaisePropertyChanged(() => SelectedApplication);

                ShowTheAvailableDataProvidersForTheSelectedApplication();
            }
        }

        void ShowTheAvailableDataProvidersForTheSelectedApplication()
        {
            DataProvidersInfos.Clear();

            switch (SelectedApplication)
            {
                case ApplicationPokerOffice:
                    {
                        var providerForPokerOffice = _dataProviderInfos.MySqlProviderInfo;
                  
                        if (_databaseSettings.ProviderIsAvailable(providerForPokerOffice))
                            DataProvidersInfos.Add(providerForPokerOffice);

                        SelectedDataProviderInfo = providerForPokerOffice;
                        break;
                    }

                case ApplicationPokerTracker:
                    {
                        var providerForPokerTracker = _dataProviderInfos.PostgresProviderInfo;
                  
                        if (_databaseSettings.ProviderIsAvailable(providerForPokerTracker))
                            DataProvidersInfos.Add(providerForPokerTracker);

                        SelectedDataProviderInfo = providerForPokerTracker;
                        break;
                    }

                case ApplicationPokerTell:
                    {
                        _databaseSettings.GetAvailableProviders()
                            .ForEach(dp => DataProvidersInfos.Add(dp));

                        SelectedDataProviderInfo = DataProvidersInfos.First();

                        break;
                    }
            }
        }

        public IDataProviderInfo SelectedDataProviderInfo
        {
            get { return _selectedDataProviderInfo; }
            set
            {
                _selectedDataProviderInfo = value;
                RaisePropertyChanged(() => SelectedDataProviderInfo);

                if (SelectedDataProviderInfo != null)
                    if (SelectedApplication == ApplicationPokerTell)
                        UpdatePokerTellDatabaseNamesForSelectedDataProvider();
            }
        }

        public IList<string> SupportedApplications
        {
            get { return new List<string> { ApplicationPokerTell, ApplicationPokerOffice, ApplicationPokerTracker }; }
        }
    }
}