namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    public interface IHandHistoryRow
    {
        string Flop { get; }

        IHoleCardsViewModel HoleCards { get; }

        string M { get; }

        string PlayerName { get; }

        string Position { get; }

        string Preflop { get; }

        string River { get; }

        string Turn { get; }
    }
}