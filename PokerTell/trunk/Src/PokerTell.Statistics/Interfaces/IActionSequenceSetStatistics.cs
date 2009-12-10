namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    public interface IActionSequenceSetStatistics
    {
        IActionSequenceSetStatistics UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics { get; }

        int[] SumOfCountsByColumn { get; }
    }
}