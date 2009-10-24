namespace PokerTell.SessionReview
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public static class Commands
    {
        public static readonly CompositeCommand SaveSessionReviewCommand = new CompositeCommand(true);
        public static readonly CompositeCommand CreateSessionReviewReportCommand = new CompositeCommand(true);
        public static readonly CompositeCommand SaveSessionReviewReportCommand = new CompositeCommand(true);
        public static readonly CompositeCommand PrintSessionReviewReportCommand = new CompositeCommand(true);
    }
}