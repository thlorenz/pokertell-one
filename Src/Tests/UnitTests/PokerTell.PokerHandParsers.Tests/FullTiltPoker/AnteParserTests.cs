namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Base;

    public class AnteParserTests : Base.AnteParserTests
    {
        protected override AnteParser GetAnteParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.AnteParser();
        }

        protected override string ValidTournamentAnte(double ante)
        {
            // Table 5 - 250/500 Ante 50 - No Limit Hold'em 
            return string.Format("Table 5 - 250/500 Ante {0} - ", ante);
        }
    }
}