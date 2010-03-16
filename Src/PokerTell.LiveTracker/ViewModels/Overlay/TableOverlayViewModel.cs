namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces;
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

        readonly IConstructor<IPlayerOverlayViewModel> _playerOverlayViewModelMake;

        public TableOverlayViewModel(
            IOverlayBoardViewModel boardViewModel, 
            IOverlaySettingsAidViewModel overlaySettingsAid, 
            IConstructor<IPlayerOverlayViewModel> playerOverlayViewModelMake)
        {
            _board = boardViewModel;
            OverlaySettingsAid = overlaySettingsAid;
            _playerOverlayViewModelMake = playerOverlayViewModelMake;
        }

        public IOverlayBoardViewModel Board
        {
            get { return _board; }
        }

        public IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; protected set; }

        public IList<IPlayerOverlayViewModel> PlayerOverlays { get; protected set; }

        public ITableOverlaySettingsViewModel OverlaySettings { get; protected set; }

        public IGameHistoryViewModel GameHistory { get; protected set; }

        public IOverlaySettingsAidViewModel OverlaySettingsAid { get; protected set; }

        public ITableOverlayViewModel InitializeWith(
            ISeatMapper seatMapper, 
            ITableOverlaySettingsViewModel overlaySettings, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, 
            int showHoleCardsDuration)
        {
            _seatMapper = seatMapper;
            _showHoleCardsDuration = showHoleCardsDuration;
            PokerTableStatisticsViewModel = pokerTableStatisticsViewModel;
            GameHistory = gameHistory;

            CreatePlayerOverlays(overlaySettings.TotalSeats);

            InitializeOverlaySettings(overlaySettings);

            RegisterEvents();

            return this;
        }

        void RegisterEvents()
        {
            PokerTableStatisticsViewModel.PlayersStatisticsWereUpdated += UpdatePlayerStatistics; 
        }

        void CreatePlayerOverlays(int totalSeats)
        {
            PlayerOverlays = new List<IPlayerOverlayViewModel>();
            foreach (var _ in 1.To(totalSeats))
            {
                PlayerOverlays.Add(_playerOverlayViewModelMake.New);
            }
        }

        public ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board)
        {
            _convertedPokerPlayers = pokerPlayers;

            if (pokerPlayers.Count() <= 0)
                throw new ArgumentException("Need at least one player");

            UpdatePlayerOverlayStatuses();

            ShowBoardAndHoleCards(board);

            return this;
        }

        void InitializeOverlaySettings(ITableOverlaySettingsViewModel overlaySettings)
        {
            OverlaySettings = overlaySettings;

            Board.InitializeWith(OverlaySettings.BoardPosition);
            OverlaySettingsAid.InitializeWith(OverlaySettings);
            for (int seat = 0; seat < PlayerOverlays.Count; seat++)
            {
                PlayerOverlays[seat].InitializeWith(OverlaySettings, seat);
            }

            OverlaySettings.PreferredSeatChanged += () => {
                UpdatePlayerOverlayStatuses();
                UpdatePlayerStatistics();
            };
        }

        void ShowBoardAndHoleCards(string board)
        {
            if (!string.IsNullOrEmpty(board) && _convertedPokerPlayers.Any(p => !string.IsNullOrEmpty(p.Holecards) && !p.Holecards.Contains("?")))
            {
                Board.UpdateWith(board);
                Board.HideBoardAfter(_showHoleCardsDuration);
                PlayerOverlays.ForEach(po => po.ShowHoleCardsFor(_showHoleCardsDuration));
            }
        }

        void UpdatePlayerOverlayStatuses()
        {
            for (int index = 0; index < PlayerOverlays.Count; index++)
            {
                var playerSeat = index + 1;

                var pokerPlayer = _convertedPokerPlayers.FirstOrDefault(p => p.SeatNumber == playerSeat);

                int mappedSeatIndex = GetMappedSeatIndexFor(playerSeat);

                UpdatePlayerOverlayStatusFor(pokerPlayer, mappedSeatIndex);
            }
        }

        int GetMappedSeatIndexFor(int playerSeat)
        {
            var mappedSeat = _seatMapper.Map(playerSeat, OverlaySettings.PreferredSeat);
            return mappedSeat - 1;
        }

        void UpdatePlayerOverlayStatusFor(IConvertedPokerPlayer pokerPlayer, int index)
        {
            if (pokerPlayer == null)
            {
                PlayerOverlays[index].UpdateStatusWith(null);
                return;
            }

            PlayerOverlays[index].UpdateStatusWith(pokerPlayer);
        }

        void UpdatePlayerStatistics()
        {
            PlayerOverlays.ForEach(po => {
                var playerStatisticsViewModel = PokerTableStatisticsViewModel.GetPlayerStatisticsViewModelFor(po.PlayerName);
                po.UpdateStatisticsWith(playerStatisticsViewModel);
            });
        }
    }
}