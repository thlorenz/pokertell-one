namespace PokerTell.DatabaseSetup.Interfaces
{
    using System.Windows.Input;

    public interface ITextBoxDialogViewModel
    {
        ICommand CommitActionCommand { get; }

        string SelectedItem { get; set; }

        string Title { get; }

        string ActionName { get; }
    }
}