namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.ObjectModel;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public sealed class DeleteDatabaseViewModel : SelectThenActOnItemViewModel
    {
        readonly IEventAggregator _eventAggregator;

        readonly IDatabaseManager _databaseManager;

        #region Constructors and Destructors

        public DeleteDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _eventAggregator = eventAggregator;
            AvailableItems = new ObservableCollection<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        public DeleteDatabaseViewModel RemoveDatabaseInUseFromAvailableItems()
        {
            string databaseInUse = _databaseManager.GetDatabaseInUse();
            if (AvailableItems.Contains(databaseInUse))
            {
                AvailableItems.Remove(databaseInUse);
            }

            return this;
        }

        #endregion

        #region Properties

        public override string Title
        {
            get { return Resources.DeleteDatabaseViewModel_Title; }
        }

        public override string ActionName
        {
            get { return Resources.Commands_Delete; }
        }
        #endregion

        #region Public Methods

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = AvailableItems[0] ?? string.Empty;
            return this;
        }

        #endregion

        #region Methods

        protected override void CommitAction()
        {
            string message = string.Format(Resources.Warning_AllDataInDatabaseWillBeLost, SelectedItem);
            var userCommitAction = new UserConfirmActionEventArgs(DeleteDatabaseAndPublishInfoMessage, message);
            _eventAggregator.GetEvent<UserConfirmActionEvent>().Publish(userCommitAction);
        }

        void DeleteDatabaseAndPublishInfoMessage()
        {
            _databaseManager.DeleteDatabase(SelectedItem);
            PublishInfoMessage();
            AvailableItems.Remove(SelectedItem);
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DatabaseDeleted, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        #endregion
    }
}