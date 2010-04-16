namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class StreetsParser : Base.StreetsParser, IFullTiltPokerStreetsParser 
    {

        const string FullTiltFlopPattern = @"\*\*\* FLOP \*\*\*";

        const string FullTiltSummaryPattern = @"\*\*\* SUMMARY \*\*\*";

        const string FullTiltTurnPattern = @"\*\*\* TURN \*\*\*";

        const string FullTiltRiverPattern = @"\*\*\* RIVER \*\*\*";


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