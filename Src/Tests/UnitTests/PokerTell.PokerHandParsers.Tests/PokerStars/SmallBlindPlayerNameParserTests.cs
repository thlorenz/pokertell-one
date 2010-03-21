namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class SmallBlindPlayerNameParserTests : Base.SmallBlindPlayerNameParserTests
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