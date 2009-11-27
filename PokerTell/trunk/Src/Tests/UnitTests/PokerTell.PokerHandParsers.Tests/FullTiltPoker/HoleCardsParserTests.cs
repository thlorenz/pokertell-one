namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class HoleCardsParserTests : Tests.HoleCardsParserTests
    {
        protected override HoleCardsParser GetHoleCardsParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.HoleCardsParser();
        }

        protected override string HeroHoleCardsFor(string playerName, string holeCards)
        {
            // Dealt to Hero [Jc Jh]
            return string.Format("Dealt to {0} [{1}]", playerName, holeCards);
        }

        protected override string MuckedCardsFor(string playerName, string holeCards)
        {
            // FCHIRO (small blind) mucked [Jh Kc] 
            return string.Format("({0} (small blind) mucked [{1}]", playerName, holeCards);
        }

        protected override string ShowedCardsFor(string playerName, string holeCards)
        {
            // 007hitman showed [Qh Qd] 
            return string.Format("({0} (small blind) showed [{1}]", playerName, holeCards);
        }
    }
}