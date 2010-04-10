namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using PokerHand;

    public interface IPlayerStatisticsUpdater : IFluentInterface
    {
        void Update(IEnumerable<IPlayerStatistics> playerStatistics);

        event Action<IEnumerable<IPlayerStatistics>> FinishedUpdatingMultiplePlayerStatistics;

        /// <summary>
        /// Updates the given playerstatistics.
        /// If the worker is currently busy, it will unsubscribe from its completed event and create a new worker to perform the update.
        /// </summary>
        /// <param name="playerStatistics"></param>
        void Update(IPlayerStatistics playerStatistics);

        event Action<IPlayerStatistics> FinishedUpdatingPlayerStatistics;

        IPlayerIdentity PlayerThatIsCurrentlyUpdated { get; }
    }
}