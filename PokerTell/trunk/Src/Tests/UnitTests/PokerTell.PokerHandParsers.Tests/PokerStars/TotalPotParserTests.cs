namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class TotalPotParserTests : Tests.TotalPotParserTests
    {
        protected override TotalPotParser GetTotalPotParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.TotalPotParser();
        }

        protected override string ValidCashGameTotalPot(double totalPot)
        {
            // Total pot $1.75 
            return string.Format("Total pot ${0}", totalPot);
        }

        protected override string ValidTournamentTotalPot(double totalPot)
        {
            // Total pot 240 
            return string.Format("Total pot {0}", totalPot);
        }
    }
}