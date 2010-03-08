namespace PokerTell.User.ViewModels
{
    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Events;

    public class UserMessageViewModel
    {
        #region Constants and Fields

        readonly UserMessageEventArgs _userMessageEventArgs;

        #endregion

        #region Constructors and Destructors

        public UserMessageViewModel(UserMessageEventArgs userMessageEventArgs)
        {
            _userMessageEventArgs = userMessageEventArgs;
        }

        #endregion

        #region Properties

        public bool ContainsDetails
        {
            get { return !string.IsNullOrEmpty(Details); }
        }

        public string Details
        {
            get
            {
                return _userMessageEventArgs.Exception != null
                           ? _userMessageEventArgs.Exception.Message
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

        #endregion
    }
}