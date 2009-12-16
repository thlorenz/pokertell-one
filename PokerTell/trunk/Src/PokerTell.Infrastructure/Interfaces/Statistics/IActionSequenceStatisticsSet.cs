namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    using PokerHand;

    public interface IActionSequenceStatisticsSet : IFluentInterface
    {
        IActionSequenceStatisticsSet UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics { get; }

        int[] SumOfCountsByColumn { get; }

        int[] TotalCounts { get; }

        int[] CumulativePercentagesByRow { get; }
    }
}