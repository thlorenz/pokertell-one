namespace PokerTell.User
{
    using System.Windows;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.User.ViewModels;
    using PokerTell.User.Views;

    public class UserService
    {
        #region Constructors and Destructors

        public UserService(IEventAggregator eventAggregator)
        {
            const bool keepMeAlive = true;
            eventAggregator
                .GetEvent<UserMessageEvent>()
                .Subscribe(HandleUserMessageEvent, ThreadOption.UIThread, keepMeAlive);
           
            eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(HandleUserConfirmActionEvent, keepMeAlive);
        }

        #endregion

        #region Methods

        static void HandleUserConfirmActionEvent(UserConfirmActionEventArgs userConfirmActionEventArgs)
        {
            var viewModel = new ConfirmActionViewModel(
                userConfirmActionEventArgs.ActionToConfirm, userConfirmActionEventArgs.Message);
            var userConfirmActionView = new ConfirmActionView(viewModel) { Owner = Application.Current.MainWindow };
            userConfirmActionView.ShowDialog();
        }

        static void HandleUserMessageEvent(UserMessageEventArgs userMessageEventArgs)
        {
            var viewModel = new UserMessageViewModel(userMessageEventArgs);
            var userMessageView = new UserMessageView(viewModel) { Owner = Application.Current.MainWindow };
            userMessageView.ShowDialog();
        }

        #endregion
    }
}