namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    public interface ITableOverlayViewModel : IFluentInterface
    {
        IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; }

        IList<IPlayerOverlayViewModel> PlayerOverlays { get; }

        ITableOverlaySettingsViewModel OverlaySettings { get; }

        IGameHistoryViewModel GameHistory { get; }

        IOverlaySettingsAidViewModel OverlaySettingsAid { get; }

        IOverlayBoardViewModel Board { get; }

        ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board);

        ITableOverlayViewModel InitializeWith(ISeatMapper seatMapper, ITableOverlaySettingsViewModel overlaySettings, IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, int showHoleCardsDuration);
    }
}