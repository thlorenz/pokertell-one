namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
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

        bool _showOverlayDetails;

        public bool ShowOverlayDetails
        {
            get { return _showOverlayDetails; }
            set
            {
                _showOverlayDetails = value;
                RaisePropertyChanged(() => ShowOverlayDetails);
            }
        }

        public event Action ShowLiveStatsWindowRequested = delegate { };

        public event Action ShowGameHistoryWindowRequested = delegate { };

        bool _detailedStatisticsIsSelected;

        public bool DetailedStatisticsIsSelected
        {
            get { return _detailedStatisticsIsSelected; }
            set
            {
                _detailedStatisticsIsSelected = value;
                RaisePropertyChanged(() => DetailedStatisticsIsSelected);
            }
        }

        bool _gameHistoryIsSelected;

        public bool GameHistoryIsSelected
        {
            get { return _gameHistoryIsSelected; }
            set
            {
                _gameHistoryIsSelected = value;
                RaisePropertyChanged(() => GameHistoryIsSelected);
            }
        }

        public ITableOverlayViewModel HideAllPlayers()
        {
            PlayerOverlays.ForEach(po => po.UpdateStatusWith(null));
            return this;
        }

        ICommand _showLiveStatsWindowCommand;

        string _heroName;

        public ICommand ShowLiveStatsWindowCommand
        {
            get
            {
                return _showLiveStatsWindowCommand ?? (_showLiveStatsWindowCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => ShowLiveStatsWindowRequested()
                    });
            }
        }

        ICommand _showGameHistoryWindowCommand;

        public ICommand ShowGameHistoryWindowCommand
        {
            get
            {
                return _showGameHistoryWindowCommand ?? (_showGameHistoryWindowCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => ShowGameHistoryWindowRequested()
                    });
            }
        }

        ICommand _showGameHistoryCommand;

        public ICommand ShowGameHistoryCommand
        {
            get
            {
                return _showGameHistoryCommand ?? (_showGameHistoryCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            ShowOverlayDetails = true;
                            GameHistoryIsSelected = true;
                        },
                    });
            }
        }

        ICommand _hideOverlayDetailsCommand;

        public ICommand HideOverlayDetailsCommand
        {
            get
            {
                return _hideOverlayDetailsCommand ?? (_hideOverlayDetailsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg =>  ShowOverlayDetails = false
                    });
            }
        }

        public ITableOverlayViewModel InitializeWith(
            ISeatMapper seatMapper, 
            ITableOverlaySettingsViewModel overlaySettings, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatisticsViewModel, 
            string heroName, 
            int showHoleCardsDuration)
        {
            _seatMapper = seatMapper;
            GameHistory = gameHistory;
            PokerTableStatisticsViewModel = pokerTableStatisticsViewModel;
            _heroName = heroName;
            _showHoleCardsDuration = showHoleCardsDuration;

            CreatePlayerOverlays(overlaySettings.TotalSeats);

            InitializeOverlaySettings(overlaySettings);

            RegisterEvents();

            return this;
        }

        void RegisterEvents()
        {
            PokerTableStatisticsViewModel.PlayersStatisticsWereUpdated += UpdatePlayerStatistics;
            PokerTableStatisticsViewModel.UserSelectedStatisticsSet += _ => {
                ShowOverlayDetails = true;
                DetailedStatisticsIsSelected = true;
            };

            PokerTableStatisticsViewModel.UserBrowsedAllHands += _ => {
                ShowOverlayDetails = true;
                DetailedStatisticsIsSelected = true;
            };

            PlayerOverlays.ForEach(
                po => po.FilterAdjustmentRequested += playerStatisticsViewModel => PokerTableStatisticsViewModel.DisplayFilterAdjustmentPopup(playerStatisticsViewModel));
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
            if (!string.IsNullOrEmpty(board) && _convertedPokerPlayers.Any(p => !string.IsNullOrEmpty(p.Holecards) && p.Name != _heroName))
            {
                Board.UpdateWith(board);
                Board.HideBoardAfter(_showHoleCardsDuration);
                PlayerOverlays.ForEach(po => {
                    if (po.PlayerName != _heroName)
                        po.ShowHoleCardsFor(_showHoleCardsDuration);
                });
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