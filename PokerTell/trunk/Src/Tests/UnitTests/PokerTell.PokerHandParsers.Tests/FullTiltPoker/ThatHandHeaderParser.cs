namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using System;

    public class ThatHandHeaderParser : Tests.ThatHandHeaderParser
    {
        #region Properties

        protected override string SiteName
        {
            get { return "Full Tilt Poker"; }
        }

        #endregion

        #region Methods

        protected override HandHeaderParser GetHandHeaderParser()
        {
            return new PokerHandParsers.FullTiltPoker.HandHeaderParser();
        }

        protected override string ValidLimitHoldemCashGameHeader(ulong gameId)
        {
            // Full Tilt Poker Game #15705378958: Table Mascot (6 max) - $8/$16 - Limit Hold'em 
            return string.Format("Full Tilt Poker Game #{0}: Table Mascot (6 max) - $8/$16 - Limit Hold'em", gameId);
        }

        protected override string ValidLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // Full Tilt Poker Game #8310873254: $200 Railbirds.com Freeroll  (63070483), Table 19 - 300/600 - Limit Hold'em 
            return string.Format(
                "Full Tilt Poker Game #{0}: $200 Railbirds.com Freeroll  ({1}), Table 19 - 300/600 - Limit Hold'em", gameId, tournamentId);
        }

        protected override string ValidNoLimitHoldemCashGameHeader(ulong gameId)
        {
            // Full Tilt Poker Game #15665278645: Table Tamworth - $0.10/$0.25 - No Limit Hold'em
            return string.Format("Full Tilt Poker Game #{0}: Table Tamworth - $0.10/$0.25 - No Limit Hold'em", gameId);
        }

        protected override string ValidNoLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // Full Tilt Poker Game #15194118553: $2 + $0.25 Sit & Go (111415980), Table 2 - 100/200 - No Limit Hold'em 
            return string.Format(
                "Full Tilt Poker Game #{0}: $2 + $0.25 Sit & Go ({1}), Table 2 - 100/200 - No Limit Hold'em ", gameId, tournamentId);
        }

        protected override string ValidPotLimitHoldemCashGameHeader(ulong gameId)
        {
            // Full Tilt Poker Game #15664982116: Table Bicycle - $0.10/$0.25 - Pot Limit Hold'em 
            return string.Format("Full Tilt Poker Game #{0}: Table Bicycle - $0.10/$0.25 - Pot Limit Hold'em", gameId);
        }

        protected override string ValidPotLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
           // Full Tilt Poker Game #15194118553: $2 + $0.25 Sit & Go (111415980), Table 2 - 100/200 - Pot Limit Hold'em 
            return string.Format(
                "Full Tilt Poker Game #{0}: $2 + $0.25 Sit & Go ({1}), Table 2 - 100/200 - Pot Limit Hold'em ", gameId, tournamentId);
        }

        #endregion
    }
}