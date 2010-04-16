namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class BlindsParser : Base.BlindsParser, IPokerStarsBlindsParser 
    {
        const string PokerStarsBlindsPattern =
            @"\(" + SharedPatterns.RatioPattern + @"/" + SharedPatterns.Ratio2Pattern;

        protected override string BlindsPattern
        {
            get { return PokerStarsBlindsPattern; }
        }

    }
}