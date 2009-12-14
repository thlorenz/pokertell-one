namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public interface IActionSequenceStatisticsSet : IFluentInterface
    {
        IActionSequenceStatisticsSet UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics { get; }

        int[] SumOfCountsByColumn { get; }

        int[] TotalCounts { get; }

        int[] CumulativePercentagesByRow { get; }
    }
}