namespace PokerTell.PokerHandParsers.Tests
{
    using Base;

    using NUnit.Framework;

    [TestFixture]
    public abstract class HeroNameParserTests
    {
        const string HeroName = "some hero";

        HeroNameParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetHeroNameParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        protected abstract HeroNameParser GetHeroNameParser();
    }
}