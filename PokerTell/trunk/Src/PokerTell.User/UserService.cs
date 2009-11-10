namespace PokerTell.User
{
    using System.Windows;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.User;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    public class UserService
    {
        readonly IUserMessageViewFactory _userMessageViewFactory;

        public UserService(IEventAggregator eventAggregator, IUserMessageViewFactory userMessageViewFactory)
        {
            _userMessageViewFactory = userMessageViewFactory;
            const bool keepMeAlive = true;
            eventAggregator.GetEvent<UserMessageEvent>().Subscribe(HandleUserMessageEvent, keepMeAlive);
        }

        void HandleUserMessageEvent(UserMessageEventArgs userMessage)
        {
            var userMessagView = _userMessageViewFactory.Create(userMessage);
            userMessagView.Owner = Application.Current.MainWindow;
            userMessagView.ShowDialog();
        }
    }
}