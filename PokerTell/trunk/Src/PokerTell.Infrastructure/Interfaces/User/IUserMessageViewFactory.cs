namespace PokerTell.Infrastructure.Interfaces.User
{
    using System.Windows;

    using Events;

    public interface IUserMessageViewFactory
    {
        Window Create(UserMessageEventArgs userMessageEventArgs);
    }
}