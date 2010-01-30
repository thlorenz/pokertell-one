namespace PokerTell.Statistics.Interfaces
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IRaiseReactionsAnalyzer
    {
        double[] RaiseSizeKeys { get; }

        IEnumerable<IRaiseReactionAnalyzer> RaiseReactionAnalyzers { get; }

        IRaiseReactionsAnalyzer AnalyzeAndAdd(
            IRaiseReactionAnalyzer raiseReactionAnalyzer,
            IAnalyzablePokerPlayer analyzablePokerPlayer,
            Streets street,
            ActionSequences actionSequence);
    }
}