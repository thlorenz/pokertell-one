namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.PokerHandParsers.Base;

    public interface IHandHeaderParser : IFluentInterface
    {
        ulong GameId { get; }

        bool IsTournament { get; }

        bool IsValid { get; }

        ulong TournamentId { get; }

        IList<HandHeaderParser.HeaderMatchInformation> FindAllHeaders(string handHistories);

        IHandHeaderParser Parse(string handHistory);
    }

    public interface IPokerStarsHandHeaderParser : IHandHeaderParser
    {
    }

    public interface IFullTiltPokerHandHeaderParser : IHandHeaderParser
    {
    }
}