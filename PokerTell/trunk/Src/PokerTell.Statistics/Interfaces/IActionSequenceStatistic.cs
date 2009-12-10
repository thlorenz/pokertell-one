namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IActionSequenceStatistic
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