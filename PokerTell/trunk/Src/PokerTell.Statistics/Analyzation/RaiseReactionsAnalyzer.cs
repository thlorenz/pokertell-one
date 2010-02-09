namespace PokerTell.Statistics.Analyzation
{
    using System;
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;

    public class RaiseReactionsAnalyzer : IRaiseReactionsAnalyzer
    {
        readonly IReactionAnalyzationPreparer _reactionAnalyzationPreparer;

        public double[] RaiseSizeKeys { get; set; }

        readonly List<IRaiseReactionAnalyzer> _raiseReactionAnalyzers = new List<IRaiseReactionAnalyzer>();

        public IEnumerable<IRaiseReactionAnalyzer> RaiseReactionAnalyzers
        {
        get { return _raiseReactionAnalyzers; }
        }

        public IRaiseReactionsAnalyzer AnalyzeAndAdd(
            IRaiseReactionAnalyzer raiseReactionAnalyzer,
            IAnalyzablePokerPlayer analyzablePokerPlayer,
            Streets street,
            ActionSequences actionSequence)
        {
            _reactionAnalyzationPreparer.PrepareAnalyzationFor(
                analyzablePokerPlayer.Sequences[(int)street], analyzablePokerPlayer.Position, actionSequence);

            if (_reactionAnalyzationPreparer.WasSuccessful)
            {
                raiseReactionAnalyzer
                    .AnalyzeUsingDataFrom(analyzablePokerPlayer, _reactionAnalyzationPreparer, RaiseSizeKeys);
                if (raiseReactionAnalyzer.IsValidResult & raiseReactionAnalyzer.IsStandardSituation)
                {
                    _raiseReactionAnalyzers.Add(raiseReactionAnalyzer);
                }
            }

            return this;
        }

        public RaiseReactionsAnalyzer(IReactionAnalyzationPreparer reactionAnalyzationPreparer)
        {
            _reactionAnalyzationPreparer = reactionAnalyzationPreparer;
        }

        public IRaiseReactionsAnalyzer InitializeWith(double[] raiseSizeKeys)
        {
            RaiseSizeKeys = raiseSizeKeys;
            if (raiseSizeKeys.Length < 1)
            {
                throw new ArgumentException("need at least one raiseSize");
            }

            return this;
        }
    }
}