namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.Interfaces;

    public interface ILayoutXDocumentHandler : IXDocumentHandler
    {
        string PokerSite { get; set; }
    }
}