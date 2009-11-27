namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class TotalPotParserTests : Tests.TotalPotParserTests
    {
        protected override TotalPotParser GetTotalPotParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.TotalPotParser();
        }

        protected override string ValidCashGameTotalPot(double totalPot)
        {
            // Total pot $12 
            return string.Format("Total pot ${0}", totalPot);
        }

        protected override string ValidTournamentTotalPot(double totalPot)
        {
            // Total pot 10,460 
            return string.Format("Total pot {0}", totalPot);
        }
    }
}