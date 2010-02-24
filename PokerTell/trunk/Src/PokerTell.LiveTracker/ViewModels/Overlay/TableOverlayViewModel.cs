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
        readonly IOverlayBoardViewModel _boardViewModel;

        int _showHoleCardsDuration;

        IEnumerable<IConvertedPokerPlayer> _convertedPokerPlayers;

        ISeatMapper _seatMapper;

        public TableOverlayViewModel(IOverlayBoardViewModel boardViewModel)
        {
            _boardViewModel = boardViewModel;
        }

        public IOverlayBoardViewModel BoardViewModel
        {
            get { return _boardViewModel; }
        }

        public ITableOverlayViewModel InitializeWith(
            ISeatMapper seatMapper, 
            ITableOverlaySettingsViewModel overlaySettings, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, 
            IList<IPlayerOverlayViewModel> playerOverlays, 
            int showHoleCardsDuration)
        {
            _seatMapper = seatMapper;
            _showHoleCardsDuration = showHoleCardsDuration;
            PokerTableStatisticsViewModel = pokerTableStatisticsViewModel;
            GameHistory = gameHistory;
            PlayerOverlays = playerOverlays;
            OverlaySettings = overlaySettings;

            for (int seat = 0; seat < playerOverlays.Count; seat++)
            {
                playerOverlays[seat].InitializeWith(overlaySettings, seat);
            }

            OverlaySettings.PreferredSeatChanged += UpdatePlayerOverlays;

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
            if (!string.IsNullOrEmpty(board) && _convertedPokerPlayers.Any(p => !string.IsNullOrEmpty(p.Holecards)))
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
                var playerSeat = index + 1;

                var pokerPlayer = _convertedPokerPlayers.FirstOrDefault(p => p.SeatNumber == playerSeat);

                int mappedSeatIndex = GetMappedSeatIndexFor(playerSeat);

                UpdatePlayerOverlayFor(pokerPlayer, mappedSeatIndex);
            }
        }

        int GetMappedSeatIndexFor(int playerSeat)
        {
            var mappedSeat = _seatMapper.Map(playerSeat, OverlaySettings.PreferredSeat);
            return mappedSeat - 1;
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