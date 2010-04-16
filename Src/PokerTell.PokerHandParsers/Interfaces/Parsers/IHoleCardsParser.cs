namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface IHoleCardsParser : IFluentInterface
    {
        string Holecards { get; }

        IHoleCardsParser Parse(string handHistory, string playerName);
    }

    public interface IPokerStarsHoleCardsParser : IHoleCardsParser
    {
    }

    public interface IFullTiltPokerHoleCardsParser : IHoleCardsParser
    {
    }
}