namespace PokerTell.LiveTracker.Interfaces
{
    using System;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    public interface ITableOverlayManager : IFluentInterface, IDisposable
    {
        ITableOverlayManager InitializeWith(IGameHistoryViewModel gameHistory, IPokerTableStatisticsViewModel pokerTableStatistics, int showHoleCardsDuration, IConvertedPokerHand firstHand);

        ITableOverlayManager UpdateWith(IConvertedPokerHand newHand);

        string HeroName { get; }

        event Action TableClosed;

        event Action ShowLiveStatsWindowRequested;

        event Action ShowGameHistoryWindowRequested;
    }
}