namespace PokerTell.PokerHandParsers.Interfaces.Parsers
{
    using System;

    using PokerTell.Infrastructure.Interfaces;

    public interface ITimeStampParser : IFluentInterface
    {
        bool IsValid { get; }

        DateTime TimeStamp { get; }

        ITimeStampParser Parse(string handHistory);
    }

    public interface IPokerStarsTimeStampParser : ITimeStampParser
    {
    }

    public interface IFullTiltPokerTimeStampParser : ITimeStampParser
    {
    }
}