namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF;

    public class ChooseDatabaseViewModel : IComboBoxDialogViewModel
    {
        #region Constants and Fields

        readonly IList<string> _availableDatabases;

        protected readonly IDatabaseManager _databaseManager;

        ICommand _commitActionCommand;

        protected readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public ChooseDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
        {
            _eventAggregator = eventAggregator;
            _databaseManager = databaseManager;
            _availableDatabases = new List<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        public virtual IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = _databaseManager.GetDatabaseInUse() ?? string.Empty;
            return this;
        }

        #endregion

        #region Properties

        public IList<string> AvailableItems
        {
            get { return _availableDatabases; }
        }

        public ICommand CommitActionCommand
        {
            get
            {
                return _commitActionCommand ?? (_commitActionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => CommitAction(), 
                        CanExecuteDelegate = arg => ! string.IsNullOrEmpty(SelectedItem)
                    });
            }
        }

        protected virtual void CommitAction()
        {
            _databaseManager.ChooseDatabase(SelectedItem);
            PublishInfoMessage();
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DatabaseChosen, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        public string SelectedItem { get; set; }

        public string Title
        {
            get { return Resources.ChooseDatabaseViewModel_Title; }
        }

        public string ActionName
        {
            get { return Resources.Commands_Save; }
        }

        #endregion
    }
}