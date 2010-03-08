namespace PokerTell.Statistics.Interfaces
{
    using Analyzation;

    using Infrastructure.Interfaces.PokerHand;

    using PokerTell.Infrastructure.Enumerations.PokerHand;

    public interface IRaiseReactionAnalyzer
    {
        /// <summary>
        /// Analyzes the data given by the analyzation preparer.
        /// </summary>
        /// <param name="analyzablePokerPlayer">Player whose data os examined</param>
        /// <param name="analyzationPreparer">Provides StartingIndex (Hero's original action) and Sequence</param>
        /// <param name="raiseSizeKeys">Raise sizes to which the Opponent Raise size should be normalized to</param>
        IRaiseReactionAnalyzer AnalyzeUsingDataFrom(IAnalyzablePokerPlayer analyzablePokerPlayer, IReactionAnalyzationPreparer analyzationPreparer, bool considerOpponentsRaiseSize, double[] raiseSizeKeys);

        /// <summary>
        /// Indicates how hero reacted to a raise by the opponent (e.g. fold)
        /// </summary>
        ActionTypes HeroReactionType { get; }

        /// <summary>
        /// Situation is considered standard if no more raises occurred after the original raise of the opponent or
        /// the hero's reaction occurred before these additional raises and thus didn't influence his decision.
        /// </summary>
        bool IsStandardSituation { get; }

        /// <summary>
        /// Is only true if an opponents raise after the action of the hero and a reaction to this 
        /// raise by the hero were found.
        /// </summary>
        bool IsValidResult { get; }

        /// <summary>
        /// Indicates the ratio of the opponents raise to which the hero reacted
        /// </summary>
        int ConsideredRaiseSize { get; }

        IAnalyzablePokerPlayer AnalyzablePokerPlayer { get; }

        
    }
}