namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class GameTypeParserTests : Base.GameTypeParserTests
    {
        protected override string LimitGameHeader()
        {
            return "Game #41482453439:  Hold'em Limit ($0.02/$0.04 USD)";
        }

        protected override string PotLimitGameHeader()
        {
            return "Game #41482418820:  Hold'em Pot Limit ($0.50/$1.00 USD";
        }

        protected override string NoLimitGameHeader()
        {
            return "Game #41301732332:  Hold'em No Limit ($0.50/$1.00 USD)";
        }

        protected override GameTypeParser GetGameTypeParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.GameTypeParser();
        }
    }
}