namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.ObjectModel;

    using Interfaces;

    public abstract class SelectThenActOnItemViewModel : SetThenActOnItemViewModel, IComboBoxDialogViewModel
    {
        public ObservableCollection<string> AvailableItems { get; protected set; }

        public abstract IComboBoxDialogViewModel DetermineSelectedItem();
    }
}