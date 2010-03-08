namespace PokerTell.LiveTracker.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.Statistics;

    using Tools.Interfaces;

    public interface IPlayerStatisticsUpdater : IFluentInterface
    {
        void Update(IEnumerable<IPlayerStatistics> playerStatistics);

        event Action<IEnumerable<IPlayerStatistics>> FinishedUpdatingPlayerStatistics;
    }
}