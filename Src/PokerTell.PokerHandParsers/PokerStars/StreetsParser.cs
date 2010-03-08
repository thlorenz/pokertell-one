namespace PokerTell.PokerHandParsers.PokerStars
{
    public class StreetsParser : Base.StreetsParser
    {
        #region Constants and Fields

        const string PokerStarsFlopPattern = @"\*\*\* FLOP \*\*\*";

        const string PokerStarsSummaryPattern = @"\*\*\* SUMMARY \*\*\*";

        const string PokerStarsTurnPattern = @"\*\*\* TURN \*\*\*";

        const string PokerStarsRiverPattern = @"\*\*\* RIVER \*\*\*";

        #endregion

        protected override string FlopPattern
        {
            get { return PokerStarsFlopPattern; }
        }

        protected override string SummaryPattern
        {
            get { return PokerStarsSummaryPattern; }
        }

        protected override string TurnPattern
        {
            get { return PokerStarsTurnPattern; }
        }

        protected override string RiverPattern
        {
            get {  return PokerStarsRiverPattern; }
        }
    }
}