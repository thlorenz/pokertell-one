namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Windows.Input;

    using PokerTell.DatabaseSetup.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public abstract class SetThenActOnItemViewModel : NotifyPropertyChanged, ITextBoxDialogViewModel
    {
        ICommand _commitActionCommand;

        public abstract string ActionName { get; }

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

        public string SelectedItem { get; set; }

        public abstract string Title { get; }

        protected abstract void CommitAction();
    }
}