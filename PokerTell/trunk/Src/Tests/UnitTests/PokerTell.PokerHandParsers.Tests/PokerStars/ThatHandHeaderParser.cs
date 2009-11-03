namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using Base;

    public class ThatHandHeaderParser : Tests.ThatHandHeaderParser
    {
        #region Properties

        protected override string SiteName
        {
            get { return "PokerStars"; }
        }

        #endregion

        #region Methods

        protected override HandHeaderParser GetHandHeaderParser()
        {
            return new PokerHandParsers.PokerStars.HandHeaderParser();
        }

        protected override string LimitHoldemCashGameHeader(ulong gameId)
        {
            // PokerStars Game #34651658916:  Hold'em Limit 
            return string.Format("PokerStars Game #{0}: Hold'em Limit", gameId);
        }

        protected override string LimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // PokerStars Game #34651902827: Tournament #207820263, $5.00+$0.50 USD Hold'em Limit
            return string.Format(
                "PokerStars Game #{0}: Tournament #{1}, $5.00+$0.50 USD Hold'em Limit", gameId, tournamentId);
        }

        protected override string NoLimitHoldemCashGameHeader(ulong gameId)
        {
            // PokerStars Game #34651629485:  Hold'em No Limit 
            return string.Format("PokerStars Game #{0}:  Hold'em No Limit", gameId);
        }

        protected override string NoLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // PokerStars Game #34652222333: Tournament #207829937, $5.00+$0.50 USD Hold'em No Limit
            return string.Format(
                "PokerStars Game #{0}: Tournament #{1}, $5.00+$0.50 USD Hold'em No Limit", gameId, tournamentId);
        }

        protected override string PotLimitHoldemCashGameHeader(ulong gameId)
        {
            // PokerStars Game #34651705415:  Hold'em Pot Limit
            return string.Format("PokerStars Game #{0}: Hold'em Pot Limit", gameId);
        }

        protected override string PotLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // PokerStars Game #34652276963: Tournament #207800852, $1.00+$0.20 USD Hold'em Pot Limit
            return string.Format(
                "PokerStars Game #{0}: Tournament #{1}, $5.00+$0.50 USD Hold'em Pot Limit", gameId, tournamentId);
        }

        
        #endregion
    }
}