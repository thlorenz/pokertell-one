namespace PokerTell.User
{
    using System.Windows;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;

    public class UserMessageHandler
    {
        #region Constructors and Destructors

        public UserMessageHandler(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<UserMessageEvent>().Subscribe(HandleUserMessageEvent);
        }

        #endregion

        #region Methods

        void HandleUserMessageEvent(UserMessageEventArgs userMessage)
        {
            MessageBox.Show(userMessage.UserMessage);
        }

        #endregion
    }
}