namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface IStreetsParser : IFluentInterface
    {
        string Flop { get; }

        bool HasFlop { get; }

        bool HasRiver { get; }

        bool HasTurn { get; }

        bool IsValid { get; }

        string Preflop { get; }

        string River { get; }

        string Turn { get; }

        IStreetsParser Parse(string handHistory);
    }

    public interface IPokerStarsStreetsParser : IStreetsParser
    {
    }

    public interface IFullTiltPokerStreetsParser : IStreetsParser
    {
    }
}