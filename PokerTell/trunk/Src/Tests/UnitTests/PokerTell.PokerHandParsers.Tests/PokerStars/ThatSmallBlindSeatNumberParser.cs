namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatSmallBlindSeatNumberParser : Tests.ThatSmallBlindSeatNumberParser
    {
        protected override SmallBlindSeatNumberParser GetSmallBlindPostionParser()
        {
            return new PokerHandParsers.PokerStars.SmallBlindSeatNumberParser();
        }

        protected override string ValidSmallBlindSeatNumber(int seat)
        {
            // Seat 2: William_Blak (small blind) folded on the Turn
            return string.Format("Seat {0}: SomeName (small blind)", seat);
        }
    }
}