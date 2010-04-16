namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using System.Collections.Generic;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public interface IPlayerActionsParser : IFluentInterface
    {
        IList<IAquiredPokerAction> PlayerActions { get; }

        IPlayerActionsParser Parse(string streetHistory, string playerName);
    }

    public interface IPokerStarsPlayerActionsParser : IPlayerActionsParser
    {
    }

    public interface IFullTiltPokerPlayerActionsParser : IPlayerActionsParser
    {
    }
}