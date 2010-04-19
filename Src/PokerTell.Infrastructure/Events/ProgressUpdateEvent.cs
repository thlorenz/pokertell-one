namespace PokerTell.Infrastructure.Events
{
    using Microsoft.Practices.Composite.Presentation.Events;

    public class ProgressUpdateEvent : CompositePresentationEvent<ProgressUpdateEventArgs>
    {
    }

    public class ProgressUpdateEventArgs
    {
        readonly ProgressTypes _progressType;

        readonly int _percentCompleted;

        public ProgressUpdateEventArgs(ProgressTypes progressType, int status)
        {
            _percentCompleted = status;
            _progressType = progressType;
        }

        public int PercentCompleted
        {
            get { return _percentCompleted; }
        }

        public ProgressTypes ProgressType
        {
            get { return _progressType; }
        }
    }

    public enum ProgressTypes
    {
        HandHistoriesDirectoryImport,
        DatabaseImport
    }
}