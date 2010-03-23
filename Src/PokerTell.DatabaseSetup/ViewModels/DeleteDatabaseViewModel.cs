namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Interfaces;
    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    public class DeleteDatabaseViewModel : SelectThenActOnItemViewModel
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEventAggregator _eventAggregator;

        readonly IDatabaseManager _databaseManager;

        public DeleteDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _eventAggregator = eventAggregator;

            AvailableItems = new ObservableCollection<string>(_databaseManager.GetAllPokerTellDatabases());
        }

        public DeleteDatabaseViewModel RemoveDatabaseInUseFromAvailableItems()
        {
            string nameOfDatabaseInUse = _databaseManager.GetDatabaseInUse();
            
            if (nameOfDatabaseInUse != null && AvailableItems.Contains(nameOfDatabaseInUse))
            {
                AvailableItems.Remove(nameOfDatabaseInUse);
            }

            return this;
        }

        public override string Title
        {
            get { return Resources.DeleteDatabaseViewModel_Title; }
        }

        public override string ActionName
        {
            get { return Infrastructure.Properties.Resources.Commands_Delete; }
        }

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = (AvailableItems.Count > 0 && AvailableItems[0] != null) ? AvailableItems[0] : string.Empty;
            return this;
        }

        protected override void CommitAction()
        {
            string message = string.Format(Resources.Warning_AllDataInDatabaseWillBeLost, SelectedItem);
            var userCommitAction = new UserConfirmActionEventArgs(DeleteDatabaseAndPublishInfoMessage, message);
            _eventAggregator.GetEvent<UserConfirmActionEvent>().Publish(userCommitAction);
        }

        protected void DeleteDatabaseAndPublishInfoMessage()
        {
            try
            {
                
            _databaseManager.DeleteDatabase(SelectedItem);
            PublishInfoMessage();
            AvailableItems.Remove(SelectedItem);
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                PublishErrorUserMessage(excep);
            }
        }

        void PublishErrorUserMessage(Exception excep)
        {
           _eventAggregator
               .GetEvent<UserMessageEvent>()
               .Publish(new UserMessageEventArgs(UserMessageTypes.Error, string.Format(Resources.Error_UnableToDeleteDatabase, SelectedItem), excep));
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DatabaseDeleted, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }
    }
}