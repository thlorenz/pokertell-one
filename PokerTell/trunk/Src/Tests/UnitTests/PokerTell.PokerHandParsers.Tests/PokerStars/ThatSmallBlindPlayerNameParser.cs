namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatSmallBlindPlayerNameParser : Tests.ThatSmallBlindPlayerNameParser
    {
        protected override SmallBlindPlayerNameParser GetSmallBlindPostionParser()
        {
            return new PokerHandParsers.PokerStars.SmallBlindPlayerNameParser();
        }

        protected override string ValidSmallBlindSeatNumber(string playerName)
        {
            // SeabrookNutz: posts small blind 10
            return string.Format("{0}: posts small blind", playerName);
        }
    }
}