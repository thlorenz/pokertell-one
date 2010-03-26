namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;

    public interface IPlayerStatisticsUpdater : IFluentInterface
    {
        void Update(IEnumerable<IPlayerStatistics> playerStatistics);

        event Action<IEnumerable<IPlayerStatistics>> FinishedUpdatingPlayerStatistics;
    }
}