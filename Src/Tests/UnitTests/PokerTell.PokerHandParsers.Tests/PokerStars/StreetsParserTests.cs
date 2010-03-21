namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class StreetsParserTests : Base.StreetsParserTests
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
            return new PokerTell.PokerHandParsers.PokerStars.StreetsParser();
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
            * *** FLOP *** [6h 8d 4s]
            * Gryko13: checks 
            * barbadardo: checks 
           */
            return string.Format("*** FLOP ***\n{0}", flop);
        }

        static string TurnOnly(string turn)
        {
            /* *** TURN *** [6h 8d 4s] [Js]
             * Gryko13: bets $0.25
             * barbadardo: folds 
            */
            return string.Format("*** TURN ***\n{0}", turn);
        }

        static string RiverOnly(string river)
        {
            /* *** RIVER *** [4s 4d Tc Qc] [6c]
            * wozgene: checks 
            * onetouch22: checks 
            */
            return string.Format("*** RIVER ***\n{0}", river);
        }

        #endregion
    }
}