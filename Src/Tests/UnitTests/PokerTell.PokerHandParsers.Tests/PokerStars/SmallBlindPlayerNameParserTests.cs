namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    using Base;

    public class SmallBlindPlayerNameParserTests : Tests.SmallBlindPlayerNameParserTests
    {
        protected override SmallBlindPlayerNameParser GetSmallBlindPostionParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.SmallBlindPlayerNameParser();
        }

        protected override string ValidSmallBlindSeatNumber(string playerName)
        {
            // SeabrookNutz: posts small blind 10
            return string.Format("{0}: posts small blind", playerName);
        }
    }
}