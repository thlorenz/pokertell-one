namespace PokerTell.Statistics.Interfaces
{
    using Analyzation;

    public interface IValuedCard
    {
        CardRank Rank { get; }

        char Suit { get; }

        double Value { get; }
    }
}