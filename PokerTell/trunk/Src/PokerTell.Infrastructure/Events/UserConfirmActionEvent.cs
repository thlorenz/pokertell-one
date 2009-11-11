namespace PokerTell.Infrastructure.Events
{
    using System;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class UserConfirmActionEvent : CompositePresentationEvent<UserConfirmActionEventArgs>
    {
    }

    public class UserConfirmActionEventArgs
    {
        public Action ActionToConfirm { get; private set; }

        public string Message { get; private set; }

        public UserConfirmActionEventArgs(Action actionToConfirm, string message)
        {
            ActionToConfirm = actionToConfirm;
            Message = message;
        }
    }
}