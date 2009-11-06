namespace PokerTell.Infrastructure.Events
{
    using System;
    using System.Runtime.Serialization;

    using Microsoft.Practices.Composite.Presentation.Events;

    public class UserMessageEvent : CompositePresentationEvent<UserMessageEventArgs>
    {
    }


    public class UserMessageEventArgs
    {
        public UserMessageTypes MessageType { get; private set; }

        public Exception Exception { get; private set; }

        public string UserMessage { get; private set; }


         public UserMessageEventArgs(UserMessageTypes messageType, string userMessage)
             : this(messageType, userMessage, null)
         {
         }

        public UserMessageEventArgs(UserMessageTypes messageType, string userMessage , Exception exception)
        {
            MessageType = messageType;
            Exception = exception;
            UserMessage = userMessage;
        }

        public override string ToString()
        {
            return string.Format("MessageType: {0}\nUserMessage: {1}\nException:\n[{2}]", MessageType, UserMessage, Exception);
        }
    }


    public enum UserMessageTypes
    {
        Info,
        Warning,
        Error
    }
}