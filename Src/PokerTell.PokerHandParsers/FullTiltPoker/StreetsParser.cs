namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class StreetsParser : Base.StreetsParser
    {
        #region Constants and Fields

        const string FullTiltFlopPattern = @"\*\*\* FLOP \*\*\*";

        const string FullTiltSummaryPattern = @"\*\*\* SUMMARY \*\*\*";

        const string FullTiltTurnPattern = @"\*\*\* TURN \*\*\*";

        const string FullTiltRiverPattern = @"\*\*\* RIVER \*\*\*";

        #endregion

        protected override string FlopPattern
        {
            get { return FullTiltFlopPattern; }
        }

        protected override string SummaryPattern
        {
            get { return FullTiltSummaryPattern; }
        }

        protected override string TurnPattern
        {
            get { return FullTiltTurnPattern; }
        }

        protected override string RiverPattern
        {
            get { return FullTiltRiverPattern; }
        }
    }
}