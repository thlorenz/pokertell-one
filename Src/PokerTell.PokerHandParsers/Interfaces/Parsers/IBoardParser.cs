namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using Base;

    using Infrastructure.Interfaces;

    public interface IBoardParser : IFluentInterface
    {
        string Board { get; }

        bool IsValid { get; }

        BoardParser Parse(string handHistory);
    }

    public interface IPokerStarsBoardParser : IBoardParser
    {
    }

    public interface IFullTiltPokerBoardParser : IBoardParser
    {
    }
}