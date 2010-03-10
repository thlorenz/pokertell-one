namespace PokerTell.LiveTracker.Tests.Fakes
{
    using System.Xml.Linq;

    using Interfaces;

    using Tools.Interfaces;

    public class XDocumentHandlerMock : IXDocumentHandler
    {
        public void Save(XDocument xmlDoc)
        {
            DocumentWasSaved = true;
            SavedDocument = xmlDoc;
        }

        public XDocument SavedDocument { get; set; }

        public XDocument DocumentToLoad { get; set; }

        public bool DocumentWasLoaded { get; set; }

        public bool DocumentWasSaved { get; set; }

        public XDocument Load()
        {
            DocumentWasLoaded = true;
            return DocumentToLoad;
        }
    }

    public class LiveTrackerSettingsXDocumentHandlerMock : XDocumentHandlerMock, ILiveTrackerSettingsXDocumentHandler
    {
    }
}