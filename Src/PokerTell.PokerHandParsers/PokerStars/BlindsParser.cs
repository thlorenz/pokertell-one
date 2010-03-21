namespace PokerTell.PokerHandParsers.PokerStars
{
    public class BlindsParser : Base.BlindsParser
    {
        const string PokerStarsBlindsPattern =
            @"\(" + SharedPatterns.RatioPattern + @"/" + SharedPatterns.Ratio2Pattern;

        protected override string BlindsPattern
        {
            get { return PokerStarsBlindsPattern; }
        }

    }
}