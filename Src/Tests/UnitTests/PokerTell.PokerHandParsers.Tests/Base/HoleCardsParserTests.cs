namespace PokerTell.PokerHandParsers.Tests.Base
{
    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    public abstract class HoleCardsParserTests
    {
        HoleCardsParser _parser;

        const string HoleCards = "As Ks";

        const string PlayerName = "hero";

        [SetUp]
        public void _Init()
        {
            _parser = GetHoleCardsParser();
        }

        [Test]
        public void Parse_EmptyString_HoleCardsAreEmpty()
        {
            _parser.Parse(string.Empty, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_NoHoleCards_HoleCardsAreEmpty()
        {
            _parser.Parse("contains no holeCards", PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_PlayerAsHeroHasHoleCards_ExtractsHoleCards()
        {
            string handHistory = HeroHoleCardsFor(PlayerName, HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(HoleCards));
        }

        [Test]
        public void Parse_OtherPlayerAsHeroHasHoleCards_HoleCardsAreEmpty()
        {
            string handHistory = HeroHoleCardsFor("otherPlayer", HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_PlayerMuckedHoleCards_ExtractsHoleCards()
        {
            string handHistory = MuckedCardsFor(PlayerName, HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(HoleCards));
        }

        [Test]
        public void Parse_OtherPlayerMuckedHoleCards_HoleCardsAreEmpty()
        {
            string handHistory = MuckedCardsFor("otherPlayer", HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Parse_PlayerShowedHoleCards_ExtractsHoleCards()
        {
            string handHistory = ShowedCardsFor(PlayerName, HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(HoleCards));
        }

        [Test]
        public void Parse_OtherPlayerShowedHoleCards_HoleCardsAreEmpty()
        {
            string handHistory = ShowedCardsFor("otherPlayer", HoleCards);
            _parser.Parse(handHistory, PlayerName);
            Assert.That(_parser.Holecards, Is.EqualTo(string.Empty));
        }
        
        protected abstract HoleCardsParser GetHoleCardsParser();

        protected abstract string HeroHoleCardsFor(string playerName, string holeCards);

        protected abstract string MuckedCardsFor(string playerName, string holeCards);
        
        protected abstract string ShowedCardsFor(string playerName, string holeCards);
    }
}