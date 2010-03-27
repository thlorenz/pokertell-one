namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Reflection;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;
    using Infrastructure.Interfaces.Repository;

    using log4net;

    using Microsoft.Practices.Composite.Events;

    using Properties;

    public class CreateDatabaseViewModel : SetThenActOnItemViewModel
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            try
            {
                _databaseManager.CreateDatabase(SelectedItem.Trim());
               
                PublishInfoMessage();
            }
            catch (Exception excep)
            {
                Log.Error(excep);
                PublishUnableToCreateDatabaseErrorMessage(excep);
            }
        }

        void PublishUnableToCreateDatabaseErrorMessage(Exception excep)
        {
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Publish(new UserMessageEventArgs(UserMessageTypes.Error, string.Format(Resources.Error_UnableToCreateDatabase, SelectedItem), excep));
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
            get { return Infrastructure.Properties.Resources.Commands_Create; }
        }
    }
}