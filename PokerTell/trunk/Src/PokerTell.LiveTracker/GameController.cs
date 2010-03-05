namespace PokerTell.LiveTracker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF.Interfaces;

    public class GameController : IGameController
    {
        readonly ITableOverlayViewModel _tableOverlay;

        readonly IOverlayToTableAttacher _overlayToTableAttacher;

        readonly IConstructor<IPlayerStatistics> _playerStatisticsMake;

        readonly IPokerTableStatisticsViewModel _pokerTableStatistics;

        readonly IGameHistoryViewModel _gameHistory;

        readonly ILayoutManager _layoutManager;

        readonly ISeatMapper _seatMapper;

        IWindowManager _tableOverlayWindow;

        IWindowManager _liveStatsWindow;

        readonly IPlayerStatisticsUpdater _playerStatisticsUpdater;

        public string HeroName { get; protected set; }

        public IDictionary<string, IPlayerStatistics> PlayerStatistics { get; protected set; }

        public bool IsLaunched { get; protected set; }

        public string PokerSite { get; protected set; }

        public GameController(
            ILayoutManager layoutManager, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatistics, 
            IConstructor<IPlayerStatistics> playerStatisticsMake, 
            IPlayerStatisticsUpdater playerStatisticsUpdater, 
            ISeatMapper seatMapper, 
            IOverlayToTableAttacher overlayToTableAttacher, 
            ITableOverlayViewModel tableOverlay)
        {
            _layoutManager = layoutManager;
            _gameHistory = gameHistory;
            _pokerTableStatistics = pokerTableStatistics;
            _playerStatisticsMake = playerStatisticsMake; 
            _playerStatisticsUpdater = playerStatisticsUpdater;
            _seatMapper = seatMapper;
            _overlayToTableAttacher = overlayToTableAttacher;
            _tableOverlay = tableOverlay;

            RegisterEvents();

            PlayerStatistics = new Dictionary<string, IPlayerStatistics>();
        }

        public ILiveTrackerSettings LiveTrackerSettings { get; set; }

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

            _gameHistory.AddNewHand(convertedPokerHand);

            UpdateSeatMapper(convertedPokerHand);

            _tableOverlay.UpdateWith(convertedPokerHand.Players, convertedPokerHand.Board);

            UpdatePlayerStatistics(convertedPokerHand);

            return this;
        }

        void RegisterEvents()
        {
            _playerStatisticsUpdater.FinishedUpdatingPlayerStatistics += _pokerTableStatistics.UpdateWith;
            _overlayToTableAttacher.TableClosed += () => {
              ShuttingDown();
              _tableOverlayWindow.Dispose();
              _liveStatsWindow.Dispose();
              _overlayToTableAttacher.Dispose();
            };
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

        void UpdateSeatMapper(IConvertedPokerHand convertedPokerHand)
        {
            var actualSeatOfHero = convertedPokerHand.Players.First(p => p.Name == HeroName).SeatNumber;
            _seatMapper.UpdateWith(actualSeatOfHero);
        }

        void Launch(IConvertedPokerHand convertedPokerHand)
        {
            HeroName = convertedPokerHand.HeroName;

            PokerSite = convertedPokerHand.Site;

            if (LiveTrackerSettings.ShowLiveStatsWindowOnStartup)
                SetupLiveStatsWindow();

            if (LiveTrackerSettings.ShowTableOverlay)
                SetupTableOverlayWindow(convertedPokerHand);

            IsLaunched = true;
        }

        void SetupLiveStatsWindow()
        {
            _liveStatsWindow
                .CreateWindow()
                .DataContext = _pokerTableStatistics;
            _liveStatsWindow.Show();
        }

        void SetupTableOverlayWindow(IConvertedPokerHand convertedPokerHand)
        {
            _seatMapper.InitializeWith(convertedPokerHand.TotalSeats);
            var overlaySettings = _layoutManager.Load(convertedPokerHand.Site, convertedPokerHand.TotalSeats);
            overlaySettings.SaveChanges += () => _layoutManager.Save(overlaySettings, convertedPokerHand.Site);
            overlaySettings.UndoChanges += revertTo => revertTo(_layoutManager.Load(convertedPokerHand.Site, convertedPokerHand.TotalSeats));

            _tableOverlay.InitializeWith(_seatMapper, overlaySettings, _gameHistory, _pokerTableStatistics, LiveTrackerSettings.ShowHoleCardsDuration);

            _tableOverlayWindow
                .CreateWindow()
                .DataContext = _tableOverlay;
            _tableOverlayWindow.Show();

            _overlayToTableAttacher.InitializeWith(_tableOverlayWindow, convertedPokerHand.Site, convertedPokerHand.TableName);
        }

        public event Action ShuttingDown = delegate { };
    }
}