namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Windows.Input;

    using Interfaces;

    using Tools.WPF;

    public abstract class SetThenActOnItemViewModel : ITextBoxDialogViewModel
    {
        #region Constants and Fields

        ICommand _commitActionCommand;

        #endregion

        #region Properties

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

        #endregion

        #region Methods

        protected abstract void CommitAction();

        #endregion
    }
}