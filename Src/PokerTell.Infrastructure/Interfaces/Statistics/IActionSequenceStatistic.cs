namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    using PokerHand;

    using Tools.Interfaces;

    public interface IActionSequenceStatistic : IFluentInterface
    {
        IActionSequenceStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        /// <summary>
        /// Compares all occurences for a certain Betsize/Position
        /// for all actions in a Stat Collection and gives percentage for each Betsize/Position
        /// </summary>
        int[] Percentages { get; }

        ActionSequences ActionSequence { get; }

        IList<IAnalyzablePokerPlayer>[] MatchingPlayers { get; }

        int TotalCounts { get;  }
    }
}