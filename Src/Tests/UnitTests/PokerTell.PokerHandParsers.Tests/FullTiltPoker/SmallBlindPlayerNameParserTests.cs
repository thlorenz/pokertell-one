namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class SmallBlindPlayerNameParserTests : Tests.SmallBlindPlayerNameParserTests
    {
        protected override SmallBlindPlayerNameParser GetSmallBlindPostionParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.SmallBlindPlayerNameParser();
        }

        protected override string ValidSmallBlindSeatNumber(string playerName)
        {
            // Jean Marron posts the small blind of 10
            return string.Format("{0} posts the small blind ", playerName);
        }
    }
}