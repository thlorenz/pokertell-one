namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class HoleCardsParserTests : Base.HoleCardsParserTests
    {
        protected override HoleCardsParser GetHoleCardsParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.HoleCardsParser();
        }

        protected override string HeroHoleCardsFor(string playerName, string holeCards)
        {
            // Dealt to renniweg [Qd 2s]
            return string.Format("Dealt to {0} [{1}]", playerName, holeCards);
        }

        protected override string MuckedCardsFor(string playerName, string holeCards)
        {
            // windauer (button) mucked [7c 4c]
            return string.Format("({0} (button) mucked [{1}]", playerName, holeCards);
        }

        protected override string ShowedCardsFor(string playerName, string holeCards)
        {
            // loco choco23 (small blind) showed [2c Jc] 
            return string.Format("({0} (small blind) showed [{1}]", playerName, holeCards);
        }
    }
}