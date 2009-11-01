namespace PokerTell.PokerHandParsers
{
    public abstract class BlindsParser
    {
        public bool IsValid { get; protected set; }

        public double BigBlind { get; protected set; }

        public double SmallBlind { get; protected set; }

        public abstract BlindsParser Parse(string handHistory);
    }
}