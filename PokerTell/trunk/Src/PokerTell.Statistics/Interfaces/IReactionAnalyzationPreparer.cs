namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Interfaces.PokerHand;

    public interface IReactionAnalyzationPreparer
    {
        int HeroPosition { get; }

        IConvertedPokerRound Sequence { get; }

        int StartingActionIndex { get; }

        bool WasSuccessful { get; }

        string ToString();
    }
}