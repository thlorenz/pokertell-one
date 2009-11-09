namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IComboBoxDialogViewModel
    {
        IList<string> AvailableItems { get; }

        ICommand SaveSettingsCommand { get; }

        string SelectedItem { get; set; }

        string Title { get; }
    }
}