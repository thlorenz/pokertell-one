namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class TotalPotParser : Base.TotalPotParser, IPokerStarsTotalPotParser
    {
        const string PokerStarsTotalPotPattern = @"Total pot " + SharedPatterns.RatioPattern;

        protected override string TotalPotPattern
        {
            get { return PokerStarsTotalPotPattern; }
        }
    }
}