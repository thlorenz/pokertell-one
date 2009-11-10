namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Microsoft.Practices.Composite.Events;

    using Properties;

    using Tools.WPF;

    public class ClearDatabaseViewModel : ChooseDatabaseViewModel
    {
        public ClearDatabaseViewModel(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
            : base(eventAggregator, databaseManager)
        {
        }

        public override IComboBoxDialogViewModel DetermineSelectedItem()
        {
            SelectedItem = string.Empty;
            return this;
        }

        protected override void CommitAction()
        {
            
        }
       
    }
}