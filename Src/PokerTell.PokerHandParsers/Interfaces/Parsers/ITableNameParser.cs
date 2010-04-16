namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using PokerTell.Infrastructure.Interfaces;

    public interface ITableNameParser : IFluentInterface
    {
        bool IsValid { get; }

        string TableName { get; }

        ITableNameParser Parse(string handHistory);
    }

    public interface IPokerStarsTableNameParser : ITableNameParser
    {
    }

    public interface IFullTiltPokerTableNameParser : ITableNameParser
    {
    }
}