namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class TotalSeatsParserTests : Base.TotalSeatsParserTests
    {
        protected override TotalSeatsParser GetTotalSeatsParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.TotalSeatsParser();
        }

        protected override string ValidTotalSeats(int totalSeats)
        {
            // Table 'Abastumani VII' 9-max 
            return string.Format("Table 'SomeName' {0}-max ", totalSeats);
        }
    }
}