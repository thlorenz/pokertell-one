namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

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

        ICommand ShowLiveStatsWindowCommand { get; }

        ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board);

        ITableOverlayViewModel InitializeWith(ISeatMapper seatMapper, ITableOverlaySettingsViewModel overlaySettings, IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, string heroName, int showHoleCardsDuration);

        event Action ShowLiveStatsWindowRequested;
    }
}