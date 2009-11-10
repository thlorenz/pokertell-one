namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IComboBoxDialogViewModel
    {
        IList<string> AvailableItems { get; }

        ICommand CommitActionCommand { get; }

        string SelectedItem { get; set; }

        string Title { get; }

        string ActionName { get; }

        IComboBoxDialogViewModel DetermineSelectedItem();
    }
}