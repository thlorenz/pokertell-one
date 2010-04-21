namespace PokerTell.User.ViewModels
{
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;
    using PokerTell.User.Interfaces;
    using PokerTell.User.Properties;

    using Tools.WPF.ViewModels;

    public class StatusBarViewModel : NotifyPropertyChanged
    {
        readonly IEventAggregator _eventAggregator;

        string _databaseStatus;

        public StatusBarViewModel(IEventAggregator eventAggregator, IProgressViewModel handHistoriesDirectoryImportProgress, IProgressViewModel databaseImportProgress)
        {
            _eventAggregator = eventAggregator;
            HandHistoriesDirectoryImportProgress = handHistoriesDirectoryImportProgress;
            DatabaseImportProgress = databaseImportProgress;

            const bool keepSubscriberReferenceAlive = true;
            _eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(SetDatabaseStatus, ThreadOption.UIThread, keepSubscriberReferenceAlive);

            _eventAggregator
                .GetEvent<ProgressUpdateEvent>()
                .Subscribe(HandleProgressUpdateEvent, ThreadOption.UIThread, keepSubscriberReferenceAlive);
        }

        public string DatabaseStatus
        {
            get { return _databaseStatus ?? string.Empty; }
            private set
            {
                _databaseStatus = value;
                RaisePropertyChanged(() => DatabaseStatus);
            }
        }

        public IProgressViewModel HandHistoriesDirectoryImportProgress { get; protected set; }

        public IProgressViewModel DatabaseImportProgress { get; protected set; }

        void HandleProgressUpdateEvent(ProgressUpdateEventArgs arg)
        {
            switch (arg.ProgressType)
            {
                case ProgressTypes.HandHistoriesDirectoryImport:
                    HandHistoriesDirectoryImportProgress.PercentCompleted = arg.PercentCompleted;
                    break;
                case ProgressTypes.DatabaseImport:
                    DatabaseImportProgress.PercentCompleted = arg.PercentCompleted;
                    break;
            }
        }

        void SetDatabaseStatus(IDataProvider dataProvider)
        {
            DatabaseStatus = string.Format(Resources.Status_ConnectedTo, dataProvider.DatabaseName);
        }
    }
}