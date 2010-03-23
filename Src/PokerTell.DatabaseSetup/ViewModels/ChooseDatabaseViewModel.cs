namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.ObjectModel;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Interfaces;
    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ChooseDatabaseViewModel : SelectThenActOnItemViewModel
    {
        protected readonly IDatabaseManager _databaseManager;

        protected readonly IEventAggregator _eventAggregator;

        readonly IDatabaseConnector _databaseConnector;

        public ChooseDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager, IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
            _eventAggregator = eventAggregator;
            _databaseManager = databaseManager;
            AvailableItems = new ObservableCollection<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            var databaseInUse = _databaseManager.GetDatabaseInUse();

            if (databaseInUse != null)
                SelectedItem = databaseInUse;
            else
                SelectedItem = (AvailableItems.Count > 0 && AvailableItems[0] != null) ? AvailableItems[0] : string.Empty;

            return this;
        }

        protected override void CommitAction()
        {
            _databaseManager.ChooseDatabase(SelectedItem);
            PublishInfoMessage();
            _databaseConnector
                .InitializeFromSettings()
                .ConnectToDatabase();
        }

        void PublishInfoMessage()
        {
            string message = string.Format(Resources.Info_DatabaseChosen, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, message);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        public override string Title
        {
            get { return Resources.ChooseDatabaseViewModel_Title; }
        }

        public override string ActionName
        {
            get { return Infrastructure.Properties.Resources.Commands_Choose; }
        }
    }
}