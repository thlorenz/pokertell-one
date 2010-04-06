namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public interface ITableOverlayViewModel : IFluentInterface
    {
        IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; }

        IList<IPlayerOverlayViewModel> PlayerOverlays { get; }

        ITableOverlaySettingsViewModel OverlaySettings { get; }

        IGameHistoryViewModel GameHistory { get; }

        IOverlaySettingsAidViewModel OverlaySettingsAid { get; }

        IOverlayBoardViewModel Board { get; }

        ICommand ShowLiveStatsWindowCommand { get; }

        bool ShowOverlayDetails { get; set; }

        ICommand HideOverlayDetailsCommand { get; }

        ICommand ShowGameHistoryWindowCommand { get; }

        bool DetailedStatisticsIsSelected { get; set; }

        bool GameHistoryIsSelected { get; set; }

        ICommand ShowGameHistoryCommand { get; }

        ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board);

        ITableOverlayViewModel InitializeWith(ISeatMapper seatMapper, ITableOverlaySettingsViewModel overlaySettings, IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, string heroName, int showHoleCardsDuration);

        event Action ShowLiveStatsWindowRequested;

        event Action ShowGameHistoryWindowRequested;

        ITableOverlayViewModel HideAllPlayers();
    }
}