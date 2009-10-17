namespace PokerTell.SessionReview
{
    using Microsoft.Practices.Composite.Presentation.Commands;

    public static class Commands
    {
        public static readonly CompositeCommand SaveSessionReviewCommand = new CompositeCommand(true);
    }
}