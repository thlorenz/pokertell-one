namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class ThatTotalSeatsParser : Tests.ThatTotalSeatsParser
    {
        protected override TotalSeatsParser GetTotalSeatsParser()
        {
            return new PokerHandParsers.PokerStars.TotalSeatsParser();
        }

        protected override string ValidTotalSeats(int totalSeats)
        {
            // Table 'Abastumani VII' 9-max 
            return string.Format("Table 'SomeName' {0}-max", totalSeats);
        }
    }
}