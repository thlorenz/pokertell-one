namespace PokerTell.User.ViewModels
{
    using System;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;

    using Tools.WPF.ViewModels;

    public class StatusBarViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        readonly IEventAggregator _eventAggregator;

        string _databaseStatus;

        #endregion

        #region Constructors and Destructors

        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            const bool keepSubscriberReferenceAlive = true;
            _eventAggregator
                .GetEvent<StatusUpdateEvent>()
                .Subscribe(arg => DatabaseStatus = arg.Status, 
                           ThreadOption.UIThread, 
                           keepSubscriberReferenceAlive, 
                           arg => arg.StatusType == StatusTypes.DatabaseConnection);
        }

        #endregion

        #region Properties

        public string DatabaseStatus
        {
            get { return _databaseStatus ?? string.Empty; }
            private set
            {
                _databaseStatus = value;
                Console.WriteLine(_databaseStatus);
                RaisePropertyChanged(() => DatabaseStatus);
            }
        }

        #endregion
    }
}