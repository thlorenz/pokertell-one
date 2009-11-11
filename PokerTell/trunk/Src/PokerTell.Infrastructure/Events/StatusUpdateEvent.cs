namespace PokerTell.Infrastructure.Events
{
    using Microsoft.Practices.Composite.Presentation.Events;

    public class StatusUpdateEvent : CompositePresentationEvent<StatusUpdateEventArgs>
    {
    }

    public class StatusUpdateEventArgs
    {
        readonly StatusTypes _statusType;

        readonly string _status;

        public StatusUpdateEventArgs(StatusTypes statusType, string status)
        {
            _status = status;
            _statusType = statusType;
        }

        public string Status
        {
            get { return _status; }
        }

        public StatusTypes StatusType
        {
            get { return _statusType; }
        }
    }

    public enum StatusTypes
    {
        DatabaseConnection
    }
}