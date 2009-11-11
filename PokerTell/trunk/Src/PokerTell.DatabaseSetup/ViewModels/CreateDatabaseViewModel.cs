namespace PokerTell.DatabaseSetup.ViewModels
{
    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Properties;

    public class CreateDatabaseViewModel : SetThenActOnItemViewModel
    {
        readonly IEventAggregator _eventAggregator;

        readonly IDatabaseManager _databaseManager;

        public CreateDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _eventAggregator = eventAggregator;
        }

        public override string Title
        {
            get { return Resources.CreateDatabaseViewModel_Title; }
        }

        protected override void CommitAction()
        {
            if (_databaseManager.DatabaseExists(SelectedItem))
            {
                PublishDatabaseExistsWarningMessage();
                return;
            }

            _databaseManager.CreateDatabase(SelectedItem);
            PublishInfoMessage();
        }

        void PublishDatabaseExistsWarningMessage()
        {
            string message = string.Format(Resources.Warning_DatabaseExistsException);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Warning, message);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishInfoMessage()
        {
            string message = string.Format(Resources.Info_DatabaseCreated, SelectedItem, Resources.ChooseDatabaseViewModel_Title);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, message);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        public override string ActionName
        {
            get { return Resources.Commands_Create; }
        }
    }
}