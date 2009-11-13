namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.ObjectModel;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class ChooseDatabaseViewModel : SelectThenActOnItemViewModel
    {
        #region Constants and Fields

        protected readonly IDatabaseManager _databaseManager;

        protected readonly IEventAggregator _eventAggregator;

        readonly IDatabaseConnector _databaseConnector;

        #endregion

        #region Constructors and Destructors

        public ChooseDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager, IDatabaseConnector databaseConnector)
        {
            _databaseConnector = databaseConnector;
            _eventAggregator = eventAggregator;
            _databaseManager = databaseManager;
            AvailableItems = new ObservableCollection<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        #endregion

        #region Properties

        #endregion

        #region Implemented Interfaces

        #region IComboBoxDialogViewModel

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = _databaseManager.GetDatabaseInUse() ?? string.Empty;
            return this;
        }

        #endregion

        #endregion

        #region Methods

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

        #endregion
    }
}