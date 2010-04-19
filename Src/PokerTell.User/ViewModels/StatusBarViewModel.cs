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

        readonly IProgressViewModel _handHistoriesDirectoryImportProgress;

        string _databaseStatus;

        public StatusBarViewModel(IEventAggregator eventAggregator, IProgressViewModel handHistoriesDirectoryImportProgress)
        {
            _handHistoriesDirectoryImportProgress = handHistoriesDirectoryImportProgress;
            _eventAggregator = eventAggregator;

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

        public IProgressViewModel HandHistoriesDirectoryImportProgress
        {
            get { return _handHistoriesDirectoryImportProgress; }
        }

        void HandleProgressUpdateEvent(ProgressUpdateEventArgs arg)
        {
            if (arg.ProgressType.Equals(ProgressTypes.HandHistoriesDirectoryImport))
            {
                _handHistoriesDirectoryImportProgress.PercentCompleted = arg.PercentCompleted;
            }
        }

        void SetDatabaseStatus(IDataProvider dataProvider)
        {
            DatabaseStatus = string.Format(Resources.Status_ConnectedTo, dataProvider.DatabaseName);
        }
    }
}