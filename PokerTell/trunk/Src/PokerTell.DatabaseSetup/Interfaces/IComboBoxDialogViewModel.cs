namespace PokerTell.DatabaseSetup.Interfaces
{
    using System.Collections.ObjectModel;

    public interface IComboBoxDialogViewModel : ITextBoxDialogViewModel
    {
        ObservableCollection<string> AvailableItems { get; }

        IComboBoxDialogViewModel DetermineSelectedItem();
    }
}