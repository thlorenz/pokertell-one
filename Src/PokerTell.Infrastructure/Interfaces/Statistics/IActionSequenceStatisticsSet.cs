namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    using PokerHand;

    using Tools.Interfaces;

    public interface IActionSequenceStatisticsSet : IFluentInterface
    {
        IActionSequenceStatisticsSet UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics { get; }

        int[] SumOfCountsByColumn { get; }

        int[] TotalCounts { get; }

        int[] CumulativePercentagesByRow { get; }

        /// <summary>
        /// What kind of action did the player perform?
        /// </summary>
        ActionSequences ActionSequence { get; }

        /// <summary>
        /// Who do these statistics belong to?
        /// </summary>
        string PlayerName { get; }

        /// <summary>
        /// For what street do these statistics apply?
        /// </summary>
        Streets Street { get; }

        /// <summary>
        /// Was Player in position? Only applies to PostFlop.
        /// </summary>
        bool InPosition { get; }

        /// <summary>
        /// Was the pot raised when player acted? Only applies to Preflop.
        /// </summary>
        bool RaisedPot { get; }

        event Action<IActionSequenceStatisticsSet> StatisticsWereUpdated;
    }
}