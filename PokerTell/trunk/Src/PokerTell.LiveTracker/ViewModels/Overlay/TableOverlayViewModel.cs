namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF.ViewModels;

    public class TableOverlayViewModel : NotifyPropertyChanged, ITableOverlayViewModel
    {
        readonly IOverlayBoardViewModel _board;

        int _showHoleCardsDuration;

        IEnumerable<IConvertedPokerPlayer> _convertedPokerPlayers;

        ISeatMapper _seatMapper;

        public TableOverlayViewModel(IOverlayBoardViewModel boardViewModel, IOverlaySettingsAidViewModel overlaySettingsAid)
        {
            _board = boardViewModel;
            OverlaySettingsAid = overlaySettingsAid;
        }

        public IOverlayBoardViewModel Board
        {
            get { return _board; }
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

            InitializeOverlaySettings(overlaySettings);

            return this;
        }

        public void InitializeOverlaySettings(ITableOverlaySettingsViewModel overlaySettings)
        {
//            OverlaySettings.InitializeWith(
//                overlaySettings.TotalSeats,
//                overlaySettings.ShowPreFlop,
//                overlaySettings.ShowFlop,
//                overlaySettings.ShowTurn,
//                overlaySettings.ShowRiver,
//                overlaySettings.ShowHarringtonM,
//                overlaySettings.StatisticsPanelWidth,
//                overlaySettings.StatisticsPanelHeight,
//                overlaySettings.Background.ColorString,
//                overlaySettings.OutOfPositionForeground.ColorString,
//                overlaySettings.InPositionForeground.ColorString,
//                overlaySettings.PreferredSeat,
//                overlaySettings.PlayerStatisticsPanelPositions,
//                overlaySettings.HarringtonMPositions,
//                overlaySettings.HoleCardsPositions,
//                overlaySettings.BoardPosition);

            OverlaySettings = overlaySettings;

            Board.InitializeWith(OverlaySettings.BoardPosition);
            OverlaySettingsAid.InitializeWith(OverlaySettings);
            for (int seat = 0; seat < PlayerOverlays.Count; seat++)
            {
                PlayerOverlays[seat].InitializeWith(OverlaySettings, seat);
            }

            OverlaySettings.PreferredSeatChanged += UpdatePlayerOverlays;
            OverlaySettings.RaisePropertyChangedForAllProperties();
            
            RaisePropertyChanged(() => PlayerOverlays);
            RaisePropertyChanged(() => OverlaySettings);
            RaisePropertyChanged(() => OverlaySettingsAid);
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
                Board.UpdateWith(board);
                Board.HideBoardAfter(_showHoleCardsDuration);
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

        public IOverlaySettingsAidViewModel OverlaySettingsAid { get; protected set; }
    }
}