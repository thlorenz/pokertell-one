namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IRaiseReactionStatistics
    {
        IDictionary<int, int> CountsDictionary { get; }

        IDictionary<ActionTypes, IDictionary<int, IList<IConvertedPokerHand>>> HandsDictionary { get; }

        IDictionary<ActionTypes, IDictionary<int, int>> PercentagesDictionary { get; }

        string ToString();
    }
}