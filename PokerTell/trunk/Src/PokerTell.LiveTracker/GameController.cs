namespace PokerTell.LiveTracker
{
    using System;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.LiveTracker.Interfaces;

    public class GameController : IGameController
    {
        readonly ITableOverlayViewModel _tableOverlay;

        readonly ITableAttacher _tableAttacher;

        readonly IConstructor<IPlayerStatistics> _playerStatisticsMake;

        readonly IPokerTableStatisticsViewModel _pokerTableStatistics;

        readonly IGameHistoryViewModel _gameHistory;

        readonly ILayoutManager _layoutManager;

        public GameController(
            ILayoutManager layoutManager, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatistics, 
            IConstructor<IPlayerStatistics> playerStatisticsMake, 
            ITableAttacher tableAttacher,
            ITableOverlayViewModel tableOverlay)
        {
            _layoutManager = layoutManager;
            _gameHistory = gameHistory;
            _pokerTableStatistics = pokerTableStatistics;
            _playerStatisticsMake = playerStatisticsMake;
            _tableAttacher = tableAttacher;
            _tableOverlay = tableOverlay;
        }

        public ILiveTrackerSettings LiveTrackerSettings { get; set; }

        public IGameController NewHand(IConvertedPokerHand convertedPokerHand)
        {
            throw new NotImplementedException();
        }

        public event Action ShuttingDown = delegate { };
    }
}