namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class TotalSeatsParserTests : Tests.TotalSeatsParserTests
    {
        protected override TotalSeatsParser GetTotalSeatsParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.TotalSeatsParser();
        }

        protected override string ValidTotalSeats(int totalSeats)
        {
            // Table 'Abastumani VII' 9-max 
            return string.Format("Table 'SomeName' {0}-max", totalSeats);
        }
    }
}