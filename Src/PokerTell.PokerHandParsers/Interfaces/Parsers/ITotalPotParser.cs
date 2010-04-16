namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface ITotalPotParser : IFluentInterface
    {
        bool IsValid { get; }

        double TotalPot { get; }

        ITotalPotParser Parse(string handHistory);
    }

    public interface IPokerStarsTotalPotParser : ITotalPotParser
    {
    }

    public interface IFullTiltPokerTotalPotParser : ITotalPotParser
    {
    }
}