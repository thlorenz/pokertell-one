namespace PokerTell.LiveTracker.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Interfaces.LiveTracker;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class TableOverlayViewModel : NotifyPropertyChanged, ITableOverlayViewModel
    {
        readonly IOverlayBoardViewModel _board;

        readonly IConstructor<IPlayerOverlayViewModel> _playerOverlayViewModelMake;

        IEnumerable<IConvertedPokerPlayer> _convertedPokerPlayers;

        bool _detailedStatisticsIsSelected;

        bool _gameHistoryIsPoppedIn;

        bool _gameHistoryIsSelected;

        string _heroName;

        ICommand _hideOverlayDetailsCommand;

        ISeatMapper _seatMapper;

        ICommand _showGameHistoryCommand;

        int _showHoleCardsDuration;

        ICommand _showLiveStatsCommand;

        bool _showOverlayDetails;

        readonly ILiveTrackerSettingsViewModel _liveTrackerSettings;

        public TableOverlayViewModel(ILiveTrackerSettingsViewModel liveTrackerSettings, IOverlayBoardViewModel boardViewModel, IOverlaySettingsAidViewModel overlaySettingsAid, IConstructor<IPlayerOverlayViewModel> playerOverlayViewModelMake)
        {
            _liveTrackerSettings = liveTrackerSettings;
            _board = boardViewModel;
            OverlaySettingsAid = overlaySettingsAid;
            _playerOverlayViewModelMake = playerOverlayViewModelMake;

            GameHistoryIsPoppedIn = _liveTrackerSettings.GameHistoryIsPoppedIn;
        }

        public event Action ShowGameHistoryWindow = delegate { };

        public event Action ShowLiveStatsWindow = delegate { };

        public IOverlayBoardViewModel Board
        {
            get { return _board; }
        }

        public bool DetailedStatisticsIsSelected
        {
            get { return _detailedStatisticsIsSelected; }
            set
            {
                _detailedStatisticsIsSelected = value;
                RaisePropertyChanged(() => DetailedStatisticsIsSelected);
            }
        }

        public IGameHistoryViewModel GameHistory { get; protected set; }

        public bool GameHistoryIsPoppedIn
        {
            get { return _gameHistoryIsPoppedIn; }
            set
            {
                _gameHistoryIsPoppedIn = value;
                RaisePropertyChanged(() => GameHistoryIsPoppedIn);
            }
        }

        public bool GameHistoryIsSelected
        {
            get { return _gameHistoryIsSelected; }
            set
            {
                _gameHistoryIsSelected = value;
                RaisePropertyChanged(() => GameHistoryIsSelected);
            }
        }

        public ICommand HideOverlayDetailsCommand
        {
            get
            {
                return _hideOverlayDetailsCommand ?? (_hideOverlayDetailsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => ShowOverlayDetails = false
                    });
            }
        }

        public ITableOverlaySettingsViewModel OverlaySettings { get; protected set; }

        public IOverlaySettingsAidViewModel OverlaySettingsAid { get; protected set; }

        public IList<IPlayerOverlayViewModel> PlayerOverlays { get; protected set; }

        public IPokerTableStatisticsViewModel PokerTableStatisticsViewModel { get; protected set; }

        public ICommand ShowGameHistoryCommand
        {
            get
            {
                return _showGameHistoryCommand ?? (_showGameHistoryCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            if (GameHistoryIsPoppedIn)
                            {
                                ShowOverlayDetails = true;
                                GameHistoryIsSelected = true;
                            }
                            else
                            {
                                ShowGameHistoryWindow();
                            }
                        }, 
                    });
            }
        }

        public ICommand ShowLiveStatsWindowCommand
        {
            get
            {
                return _showLiveStatsCommand ?? (_showLiveStatsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => ShowLiveStatsWindow()
                    });
            }
        }

        public bool ShowOverlayDetails
        {
            get { return _showOverlayDetails; }
            set
            {
                _showOverlayDetails = value;
                RaisePropertyChanged(() => ShowOverlayDetails);
            }
        }

        public ITableOverlayViewModel HideAllPlayers()
        {
            PlayerOverlays.ForEach(po => po.UpdateStatusWith(null));
            return this;
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

        public ITableOverlayViewModel UpdateWith(IEnumerable<IConvertedPokerPlayer> pokerPlayers, string board)
        {
            _convertedPokerPlayers = pokerPlayers;

            if (pokerPlayers.Count() <= 0)
                throw new ArgumentException("Need at least one player");

            UpdatePlayerOverlayStatuses();

            ShowBoardAndHoleCards(board);

            return this;
        }

        void CreatePlayerOverlays(int totalSeats)
        {
            PlayerOverlays = new List<IPlayerOverlayViewModel>();
            foreach (var _ in 1.To(totalSeats))
            {
                PlayerOverlays.Add(_playerOverlayViewModelMake.New);
            }
        }

        int GetMappedSeatIndexFor(int playerSeat)
        {
            var mappedSeat = _seatMapper.Map(playerSeat, OverlaySettings.PreferredSeat);
            return mappedSeat - 1;
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

            GameHistory.PopMeIn += () => {
                GameHistoryIsPoppedIn = true;
                ShowOverlayDetails = true;
                GameHistoryIsSelected = true;

                UpdateLiveTrackerSettings();
            };

            GameHistory.PopMeOut += () => {
                ShowGameHistoryWindow();
                DetailedStatisticsIsSelected = true;
                GameHistoryIsPoppedIn = false;

                UpdateLiveTrackerSettings();
            };

            PlayerOverlays.ForEach(po => po.FilterAdjustmentRequested +=
                                         playerStatisticsViewModel =>
                                         PokerTableStatisticsViewModel.DisplayFilterAdjustmentPopup(playerStatisticsViewModel));
        }

        void UpdateLiveTrackerSettings()
        {
            _liveTrackerSettings
                .LoadSettings()
                .GameHistoryIsPoppedIn = GameHistoryIsPoppedIn;
            _liveTrackerSettings
                .SaveSettings();
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