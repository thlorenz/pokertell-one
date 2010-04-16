namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;

    public interface IGameTypeParser : IFluentInterface
    {
        GameTypes GameType { get; }

        bool IsValid { get; }

        IGameTypeParser Parse(string handHistory);
    }

    public interface IPokerStarsGameTypeParser : IGameTypeParser
    {
    }

    public interface IFullTiltPokerGameTypeParser : IGameTypeParser
    {
    }
}