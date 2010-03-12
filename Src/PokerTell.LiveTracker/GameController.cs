namespace PokerTell.LiveTracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.PokerRooms;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public class GameController : IGameController
    {
        public GameController(
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatistics, 
            IConstructor<IPlayerStatistics> playerStatisticsMake, 
            IPlayerStatisticsUpdater playerStatisticsUpdater, 
            ITableOverlayManager tableOverlayManager)
        {
            _gameHistory = gameHistory;
            _pokerTableStatistics = pokerTableStatistics;
            _playerStatisticsMake = playerStatisticsMake;
            _playerStatisticsUpdater = playerStatisticsUpdater;
            _tableOverlayManager = tableOverlayManager;

            RegisterEvents();

            PlayerStatistics = new Dictionary<string, IPlayerStatistics>();
        }

        readonly IConstructor<IPlayerStatistics> _playerStatisticsMake;

        readonly IPokerTableStatisticsViewModel _pokerTableStatistics;

        readonly IGameHistoryViewModel _gameHistory;

        IWindowManager _liveStatsWindow;

        readonly IPlayerStatisticsUpdater _playerStatisticsUpdater;

        readonly ITableOverlayManager _tableOverlayManager;

        IWindowManager _tableOverlayWindow;

        public IDictionary<string, IPlayerStatistics> PlayerStatistics { get; protected set; }

        public bool IsLaunched { get; protected set; }

        public ILiveTrackerSettingsViewModel LiveTrackerSettings { get; set; }

        public event Action ShuttingDown = delegate { };

        public IGameController InitializeWith(IWindowManager liveStatsWindow, IWindowManager tableOverlayWindow)
        {
            _liveStatsWindow = liveStatsWindow;
            _tableOverlayWindow = tableOverlayWindow;
            return this;
        }

        public IGameController NewHand(IConvertedPokerHand convertedPokerHand)
        {
            if (! IsLaunched)
                Launch(convertedPokerHand);

            UpdateGameHistory(convertedPokerHand);

            if (LiveTrackerSettings.ShowTableOverlay)
                _tableOverlayManager.UpdateWith(convertedPokerHand);

            UpdatePlayerStatistics(convertedPokerHand);

            return this;
        }

        void UpdateGameHistory(IConvertedPokerHand convertedPokerHand)
        {
            _gameHistory.AddNewHand(convertedPokerHand);
        }

        void UpdatePlayerStatistics(IConvertedPokerHand convertedPokerHand)
        {
            var playerStatisticsToUpdate = new List<IPlayerStatistics>();
            convertedPokerHand.Players.ForEach(p => {
                if (!PlayerStatistics.ContainsKey(p.Name))
                {
                    var playerStatistics = _playerStatisticsMake.New;
                    playerStatistics.InitializePlayer(p.Name, convertedPokerHand.Site);
                    PlayerStatistics.Add(p.Name, playerStatistics);
                }

                playerStatisticsToUpdate.Add(PlayerStatistics[p.Name]);
            });

            _playerStatisticsUpdater.Update(playerStatisticsToUpdate);
        }

        void Launch(IConvertedPokerHand convertedPokerHand)
        {
            if (LiveTrackerSettings.ShowLiveStatsWindowOnStartup)
                SetupLiveStatsWindow();

            if (LiveTrackerSettings.ShowTableOverlay)
                SetupTableOverlayManager(convertedPokerHand);

            IsLaunched = true;
        }

        void SetupLiveStatsWindow()
        {
            _liveStatsWindow.DataContext = _pokerTableStatistics;
            _liveStatsWindow.Show();
        }

        void SetupTableOverlayManager(IConvertedPokerHand convertedPokerHand)
        {
            _tableOverlayManager.InitializeWith(_tableOverlayWindow, _gameHistory, _pokerTableStatistics, LiveTrackerSettings.ShowHoleCardsDuration, convertedPokerHand);
        }

        void RegisterEvents()
        {
            _playerStatisticsUpdater.FinishedUpdatingPlayerStatistics += _pokerTableStatistics.UpdateWith;
            _tableOverlayManager.TableClosed += () => {
                ShuttingDown();
                _liveStatsWindow.Dispose();
                _tableOverlayManager.Dispose();
            };
        }
    }
}