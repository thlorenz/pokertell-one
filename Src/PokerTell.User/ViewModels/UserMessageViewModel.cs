namespace PokerTell.User.ViewModels
{
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Events;

    public class UserMessageViewModel
    {
        readonly UserMessageEventArgs _userMessageEventArgs;

        public UserMessageViewModel(UserMessageEventArgs userMessageEventArgs)
        {
            _userMessageEventArgs = userMessageEventArgs;
        }

        public bool ContainsDetails
        {
            get { return !string.IsNullOrEmpty(Details); }
        }

        public string Details
        {
            get
            {
                return _userMessageEventArgs.Exception != null
                           ? string.Format("Source: {0}\nMessage: {1}", _userMessageEventArgs.Exception.Source, _userMessageEventArgs.Exception.Message)
                           : string.Empty;
            }
        }

        public string Message
        {
            get { return _userMessageEventArgs.UserMessage; }
        }

        public string Title
        {
            get { return string.Format("{0} {1}", ApplicationProperties.ApplicationName, _userMessageEventArgs.MessageType); }
        }
    }
}