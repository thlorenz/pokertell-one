namespace PokerTell.User.ViewModels
{
    using System;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;

    using Properties;

    using Tools.WPF.ViewModels;

    public class StatusBarViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        readonly IEventAggregator _eventAggregator;

        string _databaseStatus;

        readonly IProgressViewModel _handHistoriesDirectoryImportProgress;

        #endregion

        #region Constructors and Destructors

        public StatusBarViewModel(IEventAggregator eventAggregator, IProgressViewModel handHistoriesDirectoryImportProgress)
        {
            _handHistoriesDirectoryImportProgress = handHistoriesDirectoryImportProgress;
            _eventAggregator = eventAggregator;

            const bool keepSubscriberReferenceAlive = true;
            _eventAggregator
                .GetEvent<DatabaseInUseChangedEvent>()
                .Subscribe(SetDatabaseStatus,
                           ThreadOption.UIThread,
                           keepSubscriberReferenceAlive);
            _eventAggregator
                .GetEvent<ProgressUpdateEvent>()
                .Subscribe(HandleProgressUpdateEvent, ThreadOption.UIThread, keepSubscriberReferenceAlive);
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

        #endregion

        #region Properties

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

        #endregion
    }
}