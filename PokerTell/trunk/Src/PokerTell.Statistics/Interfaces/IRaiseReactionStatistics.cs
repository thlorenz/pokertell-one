namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IRaiseReactionStatistics
    {
        IDictionary<int, int> TotalCountsByColumnDictionary { get; }

        IDictionary<ActionTypes, IDictionary<int, IList<IAnalyzablePokerPlayer>>> AnalyzablePlayersDictionary { get; }

        IDictionary<ActionTypes, IDictionary<int, int>> PercentagesDictionary { get; }

        string ToString();

        IRaiseReactionStatistics InitializeWith(IRaiseReactionsAnalyzer raiseReactionsAnalyzer);
    }
}