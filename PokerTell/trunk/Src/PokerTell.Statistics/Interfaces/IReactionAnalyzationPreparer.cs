namespace PokerTell.Statistics.Interfaces
{
    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IReactionAnalyzationPreparer
    {
        int HeroPosition { get; }

        IConvertedPokerRound Sequence { get; }

        int StartingActionIndex { get; }

        bool WasSuccessful { get; }

        string ToString();

        /// <summary>
        /// Prepares the analyzation, needs to be called before it's data becomes useful
        /// </summary>
        /// <param name="sequence"><see cref="ReactionAnalyzationPreparer.Sequence"/></param>
        /// <param name="playerPosition"><see cref="ReactionAnalyzationPreparer.HeroPosition"/></param>
        /// <param name="actionSequence">The ActionSequence whose last action's index will be idendified</param>
        IReactionAnalyzationPreparer PrepareAnalyzationFor(IConvertedPokerRound sequence, int playerPosition, ActionSequences actionSequence);
    }
}