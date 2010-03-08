using System;

namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class StreetsParserTests : Tests.StreetsParserTests
    {
        #region Methods

        protected override string PreflopFlopAndSummary(string preflop, string flop)
        {
            return preflop + FlopOnly(flop) + SummaryOnly();
        }

        protected override string PreflopFlopTurnAndSummary(string preflop, string flop, string turn)
        {
            return preflop + FlopOnly(flop) + TurnOnly(turn) + SummaryOnly();
        }

        protected override string PreflopFlopTurnRiverAndSummary(string preflop, string flop, string turn, string river)
        {
            return preflop + FlopOnly(flop) + TurnOnly(turn) + RiverOnly(river) + SummaryOnly();
        }

        protected override StreetsParser GetStreetsParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.StreetsParser();
        }

        protected override string PreflopAndSummaryOnly(string preflop)
        {
            return preflop + SummaryOnly();
        }

        protected override string PreflopTurnAndSummaryOnly(string preflop)
        {
            return preflop + TurnOnly("someTurn") + SummaryOnly();
        }

        static string SummaryOnly()
        {
            return "*** SUMMARY ***";
        }

        static string FlopOnly(string flop)
        {
            /* 
              *** FLOP *** [Ac 3d Jh]
              McAsa76 checks
              Hero bets $1
           */
            return string.Format("*** FLOP *** [Ac 3d Jh]\n{0}", flop);
        }

        static string TurnOnly(string turn)
        {
            /*
             *** TURN *** [Ac 3d Jh] [Qh]
             McAsa76 checks
             Hero bets $2
            */
            return string.Format("*** TURN *** [Ac 3d Jh] [Qh]\n{0}", turn);
        }

        static string RiverOnly(string river)
        {
            /* 
            *** RIVER *** [Ac 3d Jh Qh] [Kh]
            McAsa76 bets $2.50
            Hero has 15 seconds left to act
            */
            return string.Format("*** RIVER *** [Ac 3d Jh Qh] [Kh]\n{0}", river);
        }

        #endregion
    }
}