namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using Infrastructure.Interfaces;

    public interface IBlindsParser : IFluentInterface
    {
        double BigBlind { get; }

        bool IsValid { get; }

        double SmallBlind { get; }

        IBlindsParser Parse(string handHistory);
    }

    public interface IPokerStarsBlindsParser : IBlindsParser
    {
    }

    public interface IFullTiltPokerBlindsParser : IBlindsParser
    {
    }
}