namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    using Tools.FunctionalCSharp;

    public class RaiseReactionStatisticsBuilder : IRaiseReactionStatisticsBuilder
    {
        readonly IConstructor<IRaiseReactionAnalyzer> _raiseReactionAnalyzerMake;

        readonly IRaiseReactionStatistics _raiseReactionStatistics;

        readonly IRaiseReactionsAnalyzer _raiseReactionsAnalyzer;

        public RaiseReactionStatisticsBuilder(
            IRaiseReactionStatistics raiseReactionStatistics, 
            IRaiseReactionsAnalyzer raiseReactionsAnalyzer, 
            IConstructor<IRaiseReactionAnalyzer> raiseReactionAnalyzerMake)
        {
            _raiseReactionAnalyzerMake = raiseReactionAnalyzerMake;
            _raiseReactionsAnalyzer = raiseReactionsAnalyzer;
            _raiseReactionStatistics = raiseReactionStatistics;
        }

        public IRaiseReactionStatistics Build(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, 
            ActionSequences actionSequence, 
            Streets street, 
            bool considerOpponentsRaiseSize)
        {
            if (analyzablePokerPlayers.Count() < 1)
            {
                throw new ArgumentException("need at least one analyzable player");
            }

            analyzablePokerPlayers.ForEach(
                analyzablePlayer => _raiseReactionsAnalyzer.AnalyzeAndAdd(_raiseReactionAnalyzerMake.New, 
                                                                          analyzablePlayer, 
                                                                          street, 
                                                                          actionSequence, 
                                                                          considerOpponentsRaiseSize));

            return _raiseReactionStatistics.InitializeWith(_raiseReactionsAnalyzer);
        }

        public IRaiseReactionStatisticsBuilder InitializeWith(double[] raiseSizeKeys)
        {
            _raiseReactionsAnalyzer.InitializeWith(raiseSizeKeys);
            return this;
        }
    }
}