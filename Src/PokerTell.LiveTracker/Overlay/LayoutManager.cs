namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.FunctionalCSharp;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;
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

        const string OverlayDetailsPosition = "OverlayDetailsPosition";

        const string OverlayDetailsWidth = "OverlayDetailsWidth";

        const string OverlayDetailsHeight = "OverlayDetailsHeight";

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
                .FirstOrDefault(l => l.Attributes()
                                .Any(att => att.Name == TotalSeats && att.Value == seats.ToString()));

            if (xml == null)
                return DefaultSettingsFor(seats);
            
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
                    BoardPosition = Utils.GetPositionFrom(xml.Element(BoardPosition), 10, 10), 
                    PlayerStatisticsPanelPositions = Utils.GetPositionsFrom(xml.Element(PlayerStatisticsPanelPositions)), 
                    HarringtonMPositions = Utils.GetPositionsFrom(xml.Element(HarringtonMPositions)), 
                    HoleCardsPositions = Utils.GetPositionsFrom(xml.Element(HoleCardsPositions)), 
                    OverlayDetailsPosition = Utils.GetPositionFrom(xml.Element(OverlayDetailsPosition), 300, 200),
                    OverlayDetailsWidth = Utils.GetDoubleFrom(xml.Element(OverlayDetailsWidth), 400),
                    OverlayDetailsHeight = Utils.GetDoubleFrom(xml.Element(OverlayDetailsHeight), 200)
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
                layout.BoardPosition,
                layout.OverlayDetailsPosition,
                layout.OverlayDetailsWidth,
                layout.OverlayDetailsHeight);

            return os;
        }

        const int xMargin = 150;
        const int yMArgin = 50;

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
                    Utils.XElementForPositions(HoleCardsPositions, os.HoleCardsPositions),
                    new XElement(OverlayDetailsPosition, new XElement(Left, os.OverlayDetailsPosition.Left), new XElement(Top, os.OverlayDetailsPosition.Top)),
                    new XElement(OverlayDetailsWidth, os.OverlayDetailsWidth),
                    new XElement(OverlayDetailsHeight, os.OverlayDetailsHeight));
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

        static ITableOverlaySettingsViewModel DefaultSettingsFor(int seats)
        {
            return new TableOverlaySettingsViewModel()
                .InitializeWith(
                seats,
                true,
                true,
                false,
                false,
                true,
                125,
                40,
                "#690000FF",
                "#FFFFFFFF",
                "#FFFFFF00",
                0,
                DefaultPlayerStatisticsPositionsFor(seats),
                DefaultHarringtonMPositionsFor(seats),
                DefaultHoleCardsPositionsFor(seats),
                new PositionViewModel(350, 35),
                new PositionViewModel(220, 75),
                400,
                150);
        }

        static IList<IPositionViewModel> DefaultPlayerStatisticsPositionsFor(int seats)
        {
            return DefaultPositionsFor(seats);
        }

        static IList<IPositionViewModel> DefaultHoleCardsPositionsFor(int seats)
        {
            return DefaultPositionsFor(seats).Select(pos => (IPositionViewModel) new PositionViewModel(pos.Left + xMargin - 80, pos.Top)).ToList();
        }

        static IList<IPositionViewModel> DefaultHarringtonMPositionsFor(int seats)
        {
            return DefaultPositionsFor(seats).Select(pos => (IPositionViewModel) new PositionViewModel(pos.Left + xMargin - 80, pos.Top + 20)).ToList();
        }

        static IList<IPositionViewModel> DefaultPositionsFor(int seats)
        {
            const int y = yMArgin;

            var maxElemInFirstRow = seats < 4 ? seats : 4;

            int maxElemInSecondRow;
            if (seats <= 4)
                maxElemInSecondRow = 0;
            else
                maxElemInSecondRow = seats < 8 ? seats : 8;

            int maxElemInThirdRow;
            if (seats <= 4)
                maxElemInThirdRow = 0;
            else
                maxElemInThirdRow = seats < 10 ? seats : 10;

            var positions = new List<IPositionViewModel>();

            int col = 0;
            for (int seat = 0; seat < maxElemInFirstRow; seat++)
            {
                positions.Add(new PositionViewModel(col * xMargin, y));
                col++;
            }

            col = 0;
            for (int seat = maxElemInFirstRow; seat < maxElemInSecondRow; seat++)
            {
                positions.Add(new PositionViewModel(col * xMargin, y + yMArgin));
                col++;
            }

            col = 0;
            for (int seat = maxElemInSecondRow; seat < maxElemInThirdRow; seat++)
            {
                positions.Add(new PositionViewModel(col * xMargin, y + yMArgin + yMArgin));
                col++;
            }

            return positions;
        }

    }
}