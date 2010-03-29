namespace PokerTell.PokerHandParsers.PokerStars
{
    public class StreetsParser : Base.StreetsParser
    {
        const string PokerStarsFlopPattern = @"\*\*\* FLOP \*\*\*";

        const string PokerStarsRiverPattern = @"\*\*\* RIVER \*\*\*";

        const string PokerStarsSummaryPattern = @"\*\*\* SUMMARY \*\*\*";

        const string PokerStarsTurnPattern = @"\*\*\* TURN \*\*\*";

        protected override string FlopPattern
        {
            get { return PokerStarsFlopPattern; }
        }

        protected override string RiverPattern
        {
            get { return PokerStarsRiverPattern; }
        }

        protected override string SummaryPattern
        {
            get { return PokerStarsSummaryPattern; }
        }

        protected override string TurnPattern
        {
            get { return PokerStarsTurnPattern; }
        }
    }
}