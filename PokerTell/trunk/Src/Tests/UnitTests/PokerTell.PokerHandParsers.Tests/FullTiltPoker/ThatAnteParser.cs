namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    public class ThatAnteParser : Tests.ThatAnteParser
    {
        protected override AnteParser GetAnteParser()
        {
            return new PokerHandParsers.FullTiltPoker.AnteParser();
        }

        protected override string ValidCashGameAnte(double ante)
        {
            // Table 5 - 250/500 Ante 50 - No Limit Hold'em 
            return string.Format("Table 5 - 250/500 Ante ${0} - ", ante);
        }

        protected override string ValidTournamentAnte(double ante)
        {
            // Table 5 - 250/500 Ante 50 - No Limit Hold'em 
            return string.Format("Table 5 - 250/500 Ante {0} - ", ante);
        }
    }
}