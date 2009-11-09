namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Interfaces.DatabaseSetup;

    using Properties;

    using Tools.WPF;

    public class ChooseDatabaseViewModel : IComboBoxDialogViewModel
    {
        #region Constants and Fields

        readonly IList<string> _availableDatabases;

        ICommand _saveSettingsCommand;

        readonly IDatabaseManager _databaseManager;

        #endregion

        #region Constructors and Destructors

        public ChooseDatabaseViewModel(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _availableDatabases = new List<string>(_databaseManager.GetAllPokerTellDatabases());
            SelectedItem = _availableDatabases.FirstOrDefault();
        }

        #endregion

        #region Properties

        public IList<string> AvailableItems
        {
            get { return _availableDatabases; }
        }

        public ICommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _databaseManager.ChooseDatabase(SelectedItem), 
                        CanExecuteDelegate = arg => ! string.IsNullOrEmpty(SelectedItem)
                    });
            }
        }

        public string SelectedItem { get; set; }

        public string Title
        {
            get { return Resources.ChooseDatabaseViewModel_Title; }
        }

        #endregion
    }
}