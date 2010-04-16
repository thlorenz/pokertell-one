namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class TotalPotParser : Base.TotalPotParser, IFullTiltPokerTotalPotParser
    {
        const string FullTiltTotalPotPattern = @"Total pot " + SharedPatterns.RatioPattern;

        protected override string TotalPotPattern
        {
            get { return FullTiltTotalPotPattern; }
        }
    }
}