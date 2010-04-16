namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.PokerHandParsers.Base;

    public interface IPlayerSeatsParser : IFluentInterface
    {
        bool IsValid { get; }

        IDictionary<int, PlayerSeatsParser.PlayerData> PlayerSeats { get; }

        IPlayerSeatsParser Parse(string handHistory);
    }

    public interface IPokerStarsPlayerSeatsParser : IPlayerSeatsParser
    {
    }

    public interface IFullTiltPokerPlayerSeatsParser : IPlayerSeatsParser
    {
    }
}