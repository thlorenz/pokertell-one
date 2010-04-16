namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class BlindsParser : Base.BlindsParser, IFullTiltPokerBlindsParser 
    {
        const string FullTiltBlindsPattern =
            @" - " + SharedPatterns.RatioPattern + "/" + SharedPatterns.Ratio2Pattern + " ";

        protected override string BlindsPattern
        {
            get { return FullTiltBlindsPattern; }
        }
    }
}