namespace PokerTell.LiveTracker.Overlay
{
    using System.Linq;
    using System.Xml.Linq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.Xml;

    public class LayoutManager : ILayoutManager
    {
        const string Layout = "Layout";
        const string Layouts = "Layouts";

        const string ShowPreFlop = "ShowPreFlop";

        const string ShowFlop = "ShowFlop";

        const string ShowTurn = "ShowTurn";

        const string ShowRiver = "ShowRiver";

        const string ShowHarringtonM = "ShowHarringtonM";

        const string StatisticsPanelWidth = "StatisticsPanelWidth";

        const string StatisticsPanelHeight = "StatisticsPanelHeight";

        const string Background = "Background";

        const string OutOfPositionForeground = "OutOfPositionForeground";

        const string InPositionForeground = "InPositionForeground";

        const string PreferredSeat = "PreferredSeat";

        const string BoardPosition = "BoardPosition";

        const string PlayerStatisticsPanelPositions = "PlayerStatisticsPanelPositions";

        const string HarringtonMPositions = "HarringtonMPositions";

        const string HoleCardsPositions = "HoleCardsPositions";

        const string TotalSeats = "TotalSeats";

        const string Left = "Left";

        const string Top = "Top";

        readonly ILayoutXDocumentHandler _xDocumentHandler;

        public LayoutManager(ILayoutXDocumentHandler xDocumentHandler)
        {
            _xDocumentHandler = xDocumentHandler;
        }

        public ITableOverlaySettingsViewModel Load(string pokerSite, int seats)
        {
            _xDocumentHandler.PokerSite = pokerSite;

            var xmlDoc = _xDocumentHandler.Load();

            var xml = xmlDoc.Descendants(Layout)
                .First(l => l.Attributes()
                                .Any(att => att.Name == TotalSeats && att.Value == seats.ToString()));
            var layout = new
                {
                    ShowPreFlop = Utils.GetBoolFrom(xml.Element(ShowPreFlop), true), 
                    ShowFlop = Utils.GetBoolFrom(xml.Element(ShowFlop), true), 
                    ShowTurn = Utils.GetBoolFrom(xml.Element(ShowTurn), false), 
                    ShowRiver = Utils.GetBoolFrom(xml.Element(ShowRiver), false), 
                    ShowHarringtonM = Utils.GetBoolFrom(xml.Element(ShowHarringtonM), false), 
                    StatisticsPanelWidth = Utils.GetDoubleFrom(xml.Element(StatisticsPanelWidth), 100), 
                    StatisticsPanelHeight = Utils.GetDoubleFrom(xml.Element(StatisticsPanelHeight), 70), 
                    Background = Utils.GetStringFrom(xml.Element(Background), "#AA0000FF"), 
                    OutOfPositionForeground = Utils.GetStringFrom(xml.Element(OutOfPositionForeground), "White"), 
                    InPositionForeground = Utils.GetStringFrom(xml.Element(InPositionForeground), "Yellow"), 
                    PreferredSeat = Utils.GetIntFrom(xml.Element(PreferredSeat), 0), 
                    BoardPosition = Utils.GetPositionFrom(xml.Element(BoardPosition)), 
                    PlayerStatisticsPanelPositions = Utils.GetPositionsFrom(xml.Element(PlayerStatisticsPanelPositions)), 
                    HarringtonMPositions = Utils.GetPositionsFrom(xml.Element(HarringtonMPositions)), 
                    HoleCardsPositions = Utils.GetPositionsFrom(xml.Element(HoleCardsPositions)), 
                };

            var os = new TableOverlaySettingsViewModel()
                .InitializeWith(
                seats, 
                layout.ShowPreFlop, 
                layout.ShowFlop, 
                layout.ShowTurn, 
                layout.ShowRiver, 
                layout.ShowHarringtonM, 
                layout.StatisticsPanelWidth, 
                layout.StatisticsPanelHeight, 
                layout.Background, 
                layout.OutOfPositionForeground, 
                layout.InPositionForeground, 
                layout.PreferredSeat, 
                layout.PlayerStatisticsPanelPositions, 
                layout.HarringtonMPositions, 
                layout.HoleCardsPositions, 
                layout.BoardPosition);

            return os;
        }

        public static XElement CreateXElementFor(ITableOverlaySettingsViewModel os)
        {
            return
                new XElement(Layout, 
                    new XAttribute(TotalSeats, os.TotalSeats), 
                    new XElement(ShowPreFlop, os.ShowPreFlop), 
                    new XElement(ShowFlop, os.ShowFlop), 
                    new XElement(ShowTurn, os.ShowTurn), 
                    new XElement(ShowRiver, os.ShowRiver), 
                    new XElement(ShowHarringtonM, os.ShowHarringtonM), 
                    new XElement(StatisticsPanelWidth, os.StatisticsPanelWidth), 
                    new XElement(StatisticsPanelHeight, os.StatisticsPanelHeight), 
                    new XElement(Background, os.Background), 
                    new XElement(OutOfPositionForeground, os.OutOfPositionForeground), 
                    new XElement(InPositionForeground, os.InPositionForeground), 
                    new XElement(PreferredSeat, os.PreferredSeat), 
                    new XElement(BoardPosition, new XElement(Left, os.BoardPosition.Left), new XElement(Top, os.BoardPosition.Top)), 
                    Utils.XElementForPositions(PlayerStatisticsPanelPositions, os.PlayerStatisticsPanelPositions), 
                    Utils.XElementForPositions(HarringtonMPositions, os.HarringtonMPositions), 
                    Utils.XElementForPositions(HoleCardsPositions, os.HoleCardsPositions));
        }

        public ILayoutManager Save(ITableOverlaySettingsViewModel overlaySettings, string pokerSite)
        {
            var totalSeats = overlaySettings.TotalSeats;

            _xDocumentHandler.PokerSite = pokerSite;
            var xmlDoc = _xDocumentHandler.Load();

            var xmlLayout = xmlDoc.Descendants(Layout)
                .FirstOrDefault(l => l.Attributes().Any(att => att.Name == TotalSeats && att.Value == totalSeats.ToString()));

            if (xmlLayout != null)
            {
                // Layout exists -> replace
                xmlLayout.ReplaceWith(CreateXElementFor(overlaySettings));
            }
            else
            {
                // Layout not saved before
                xmlDoc.Element(Layouts).Add(CreateXElementFor(overlaySettings));
            }

            _xDocumentHandler.Save(xmlDoc);
            return this;
        }
    }
}