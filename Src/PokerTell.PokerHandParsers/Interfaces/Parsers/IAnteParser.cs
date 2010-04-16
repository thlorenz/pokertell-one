namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface IAnteParser : IFluentInterface
    {
        double Ante { get; }

        bool IsValid { get; }

        IAnteParser Parse(string handHistory);
    }

    public interface IPokerStarsAnteParser : IAnteParser
    {
    }

    public interface IFullTiltPokerAnteParser : IAnteParser
    {
    }
}