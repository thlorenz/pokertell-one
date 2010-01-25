namespace PokerTell.Statistics.Interfaces
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    public interface IReactionAnalyzationPreparer
    {
        ActionSequences ActionSequence { get; set; }

        IConvertedPokerHand ConvertedHand { get; }

        int HeroIndex { get; }

        string HeroName { get; set; }

        IConvertedPokerRound Sequence { get; }

        int StartingActionIndex { get; }

        Streets Street { get; set; }

        bool WasSuccessful { get; }

        string ToString();
    }
}