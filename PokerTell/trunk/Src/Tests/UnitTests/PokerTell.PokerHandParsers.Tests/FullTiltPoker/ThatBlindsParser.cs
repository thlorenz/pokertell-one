namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class ThatBlindsParser : Tests.ThatBlindsParser
    {
        protected override string TournamentGameWithValidBlinds(double smallBlind, double bigBlind)
        {
            // Table 5 - 250/500 
            return string.Format("Table 5 - {0}/{1} ", smallBlind, bigBlind);  
        }

        protected override BlindsParser GetBlindsParser()
        {
            return new PokerHandParsers.FullTiltPoker.BlindsParser();
        }

        protected override string CashGameWithValidBlinds(double smallBlind, double bigBlind)
        {
            // Table Tamworth - $0.10/$0.25  
            return string.Format("Table Tamworth - ${0}/${1} ", smallBlind, bigBlind);
        }
    }
}