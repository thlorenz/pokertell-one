namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;

    public class TableOverlayViewModel : ITableOverlayViewModel
    {
        readonly IBoardViewModel _boardViewModel;

        int _showHoleCardsDuration;

        IEnumerable<IConvertedPokerPlayer> _convertedPokerPlayers;

        public TableOverlayViewModel(IBoardViewModel boardViewModel)
        {
            _boardViewModel = boardViewModel;
        }

        public IBoardViewModel BoardViewModel
        {
            get { return _boardViewModel; }
        }

        public ITableOverlayViewModel InitializeWith(ISeatMapper seatMapper, ITableOverlaySettingsViewModel overlaySettings, IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, IList<IPlayerOverlayViewModel> playerOverlays, int showHoleCardsDuration)
        {
            _showHoleCardsDuration = showHoleCardsDuration;
            PokerTableStatisticsViewModel = pokerTableStatisticsViewModel;
            GameHistory = gameHistory;
            PlayerOverlays = playerOverlays;
            OverlaySettings = overlaySettings;

            for (int seat = 0; seat < playerOverlays.Count; seat++)
            {
                playerOverlays[seat].InitializeWith(overlaySettings, seat);
            }

            return this;
        }

        public ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board)
        {
            _convertedPokerPlayers = pokerPlayers;

            if (pokerPlayers.Count() <= 0)
                throw new ArgumentException("Need at least one player");

            UpdatePlayerOverlays();

            ShowBoardAndHoleCards(board);

            return this;
        }

        void ShowBoardAndHoleCards(string board)
        {
            if (!string.IsNullOrEmpty(board) && _convertedPokerPlayers .Any(p => !string.IsNullOrEmpty(p.Holecards)))
            {
                BoardViewModel.UpdateWith(board);
                BoardViewModel.HideBoardAfter(_showHoleCardsDuration);
                PlayerOverlays.ForEach(po => po.ShowHoleCardsFor(_showHoleCardsDuration));
            }
        }

        void UpdatePlayerOverlays()
        {
            for (int index = 0; index < PlayerOverlays.Count; index++)
            {
                var playerOverlayIndex = index;
                var pokerPlayer = _convertedPokerPlayers .FirstOrDefault(p => p.SeatNumber == playerOverlayIndex + 1);

                UpdatePlayerOverlayFor(pokerPlayer, index);
            }
        }

        void UpdatePlayerOverlayFor(IConvertedPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer == null)
            {
                PlayerOverlays[index].UpdateWith(null, null);
                return;
            }

            var playerStatisticsViewModel = PokerTableStatisticsViewModel.GetPlayerStatisticsViewModelFor(pokerPlayer.Name);
            if (playerStatisticsViewModel == null)
            {
                PlayerOverlays[index].UpdateWith(null, null);
                return;
            }

            PlayerOverlays[index].UpdateWith(playerStatisticsViewModel, pokerPlayer);
        }

        public IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; protected set; }

        public IList<IPlayerOverlayViewModel> PlayerOverlays { get; protected set; }

        public ITableOverlaySettingsViewModel OverlaySettings { get; protected set; }

        public IGameHistoryViewModel GameHistory { get; protected set; }
    }
}