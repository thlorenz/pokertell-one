namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    public interface IDetailedStatistic
    {
        IDetailedStatistic UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers);

        IList<int>[] HandIdsLists { get; }

        /// <summary>
        /// Compares all occurences for a certain Betsize/Position
        /// for all actions in a Stat Collection and gives percentage for each Betsize/Position
        /// </summary>
        int[] Percentages { get; set; }
    }
}