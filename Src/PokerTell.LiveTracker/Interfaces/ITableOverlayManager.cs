namespace PokerTell.LiveTracker.Interfaces
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    public interface ITableOverlayManager : IDisposable
    {
        ITableOverlayManager InitializeWith(
            IWindowManager tableOverlayWindow,
            IGameHistoryViewModel gameHistory,
            IPokerTableStatisticsViewModel pokerTableStatistics,
            int showHoleCardsDuration,
            IConvertedPokerHand firstHand);

        ITableOverlayManager UpdateWith(IConvertedPokerHand newHand);

        string HeroName { get; }

        event Action TableClosed;
    }
}