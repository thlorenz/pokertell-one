namespace PokerTell.PokerHandParsers.PokerStars
{
    using Interfaces.Parsers;

    public class AnteParser : Base.AnteParser, IPokerStarsAnteParser 
    {
        const string PokerStarsAntePattern = @"posts the ante " + SharedPatterns.RatioPattern;

        protected override string AntePattern
        {
            get { return PokerStarsAntePattern; }
        }
    }
}