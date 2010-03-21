namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Base;

    public class GameTypeParserTests : Base.GameTypeParserTests
    {
        protected override string LimitGameHeader()
        {
            return "Full Tilt Poker Game #15705378958: Table Mascot (6 max) - $8/$16 - Limit Hold'em - 15:15:09 ET - 2009/10/31";
        }

        protected override string PotLimitGameHeader()
        {
            return "Full Tilt Poker Game #15664982116: Table Bicycle - $0.10/$0.25 - Pot Limit Hold'em - 17:28:11 ET - 2009/10/29";
        }

        protected override string NoLimitGameHeader()
        {
            return "Full Tilt Poker Game #15074886673: $2 + $0.25 Rebuy (110401163), Table 54 - 40/80 - No Limit Hold'em - 14:25:01 ET - 2009/10/02";
        }

        protected override GameTypeParser GetGameTypeParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.GameTypeParser();
        }
    }
}