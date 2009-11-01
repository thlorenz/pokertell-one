namespace PokerTell.PokerHandParsers
{
    using System;

    public abstract class TimeStampParser
    {
        public bool IsValid { get; protected set; }

        public DateTime TimeStamp { get; protected set; }

        public abstract TimeStampParser Parse(string handHistory);
    }
}