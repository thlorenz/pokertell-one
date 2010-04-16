namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface ITotalSeatsParser : IFluentInterface
    {
        bool IsValid { get; }

        int TotalSeats { get; }

        ITotalSeatsParser Parse(string handHistory);
    }

    public interface IPokerStarsTotalSeatsParser : ITotalSeatsParser
    {
    }

    public interface IFullTiltPokerTotalSeatsParser : ITotalSeatsParser
    {
    }
}