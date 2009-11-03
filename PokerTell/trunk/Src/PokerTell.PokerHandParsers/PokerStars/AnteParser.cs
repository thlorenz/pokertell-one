namespace PokerTell.PokerHandParsers.PokerStars
{
    public class AnteParser : Base.AnteParser
    {
        const string PokerStarsAntePattern = @"posts the ante " + SharedPatterns.RatioPattern;

        protected override string AntePattern
        {
            get { return PokerStarsAntePattern; }
        }
    }
}