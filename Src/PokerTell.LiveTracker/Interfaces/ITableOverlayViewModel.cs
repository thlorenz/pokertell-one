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

        bool DetailedStatisticsIsSelected { get; set; }

        bool GameHistoryIsSelected { get; set; }

        ICommand ShowGameHistoryCommand { get; }

        bool GameHistoryIsPoppedIn { get; set; }

        ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board);

        ITableOverlayViewModel InitializeWith(ISeatMapper seatMapper, ITableOverlaySettingsViewModel overlaySettings, IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, string heroName, int showHoleCardsDuration);

        event Action ShowLiveStatsWindow;

        event Action ShowGameHistoryWindow;

        ITableOverlayViewModel HideAllPlayers();
    }
}