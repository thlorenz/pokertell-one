namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.ObjectModel;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Interfaces;
    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public sealed class ClearDatabaseViewModel : SelectThenActOnItemViewModel
    {
        readonly IEventAggregator _eventAggregator;

        readonly IDatabaseManager _databaseManager;

        public ClearDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _eventAggregator = eventAggregator;
            AvailableItems = new ObservableCollection<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        public override string Title
        {
            get { return Resources.ClearDatabaseViewModel_Title; }
        }

        public override string ActionName
        {
            get { return Infrastructure.Properties.Resources.Commands_Clear; }
        }

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = (AvailableItems.Count > 0 && AvailableItems[0] != null) ? AvailableItems[0] : string.Empty;
            return this;
        }

        protected override void CommitAction()
        {
            string message = string.Format(Resources.Warning_AllDataInDatabaseWillBeLost, SelectedItem);
            var userCommitAction = new UserConfirmActionEventArgs(ClearDatabaseAndPublishInfoMessage, message);
            _eventAggregator.GetEvent<UserConfirmActionEvent>().Publish(userCommitAction);
        }

        void ClearDatabaseAndPublishInfoMessage()
        {
            _databaseManager.ClearDatabase(SelectedItem);
            PublishInfoMessage();
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DatabaseCleared, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }
    }
}