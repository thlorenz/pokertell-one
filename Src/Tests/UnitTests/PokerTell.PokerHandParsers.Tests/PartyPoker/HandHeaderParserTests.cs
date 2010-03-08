namespace PokerTell.PokerHandParsers.Tests.PartyPoker
{
    using System;

    using Base;

    public class HandHeaderParserTests : Tests.HandHeaderParserTests
    {
        protected override string SiteName
        {
            get { return "Party Poker"; }
        }

        protected override HandHeaderParser GetHandHeaderParser()
        {
            return new PokerTell.PokerHandParsers.PartyPoker.HandHeaderParser();
        }

        protected override string NoLimitHoldemCashGameHeader(ulong gameId)
        {
            // ***** Hand History for Game 8560848979 ***** // $10 USD NL Texas Hold'em
            return string.Format("***** Hand History for Game {0} ***** \n$10 USD NL Texas Hold'em", gameId);
        }

        protected override string LimitHoldemCashGameHeader(ulong gameId)
        {
            // Simply "Texas Hold'em" means Limit
            // ***** Hand History for Game 8440527606 ***** // 0.25/0.50 Texas Hold'em Game Table (Limit) 
            return string.Format(
                "***** Hand History for Game {0} ***** \n0.25/0.50 Texas Hold'em Game Table (Limit)", gameId);
        }

        protected override string PotLimitHoldemCashGameHeader(ulong gameId)
        {
          // ***** Hand History for Game 8386494168 ***** // $200 USD PL Texas Hold'em 
            return string.Format("***** Hand History for Game {0} ***** \n$200 USD PL Texas Hold'em", gameId);
        }

        protected override string NoLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // ***** Hand History for Game 8553835070 ***** // NL Texas Hold'em $1 USD Buy-in Trny: 48196117 
            return string.Format(
                "***** Hand History for Game {0} ***** \nNL Texas Hold'em $1 USD Buy-in Trny: {1}", gameId, tournamentId);
        }

        protected override string LimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
            // Simply "Texas Hold'em" means Limit
            // ***** Hand History for Game 5789158787 ***** // Texas Hold em Trny:32401475
            return string.Format(
                "***** Hand History for Game {0} ***** // Texas Hold em Trny:{1}", gameId, tournamentId);
        }

        protected override string PotLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId)
        {
           // ***** Hand History for Game 7730261003 ***** // PL Texas Hold'em  Trny: 43983063 
            return string.Format(
                "***** Hand History for Game {0} ***** \nPL Texas Hold'em  Trny: {1}", gameId, tournamentId);
        }
    }
}