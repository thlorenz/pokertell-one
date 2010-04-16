namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface ISmallBlindPlayerNameParser : IFluentInterface
    {
        bool IsValid { get; }

        string SmallBlindPlayerName { get; }

        ISmallBlindPlayerNameParser Parse(string handHistory);
    }

    public interface IPokerStarsSmallBlindPlayerNameParser : ISmallBlindPlayerNameParser
    {
    }

    public interface IFullTiltPokerSmallBlindPlayerNameParser : ISmallBlindPlayerNameParser
    {
    }
}