namespace PokerTell.User
{
    using System.Windows;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.User;

    using ViewModels;

    using Views;

    public class UserMessageViewFactory : IUserMessageViewFactory
    {
        public Window Create(UserMessageEventArgs userMessageEventArgs)
        {
            var viewModel = new UserMessageViewModel(userMessageEventArgs);
            return new UserMessageView(viewModel);
        }
    }
}