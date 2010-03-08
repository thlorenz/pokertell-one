namespace PokerTell.User.ViewModels
{
    using System;
    using System.Windows.Input;

    using Tools.WPF;

    public class ConfirmActionViewModel
    {
        readonly Action _actionToExecute;

        public ConfirmActionViewModel(Action actionToExecute, string message)
        {
            _actionToExecute = actionToExecute;
            Message = message;
        }

        ICommand _confirmActionCommand;

        public ICommand ConfirmActionCommand
        {
            get
            {
                return _confirmActionCommand ?? (_confirmActionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _actionToExecute(),
                    });
            }
        }

        public string Message { get; private set; }
    }
}