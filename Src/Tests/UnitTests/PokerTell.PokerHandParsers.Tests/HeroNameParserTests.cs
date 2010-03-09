namespace PokerTell.PokerHandParsers.Tests
{
    using Base;

    using NUnit.Framework;

    using UnitTests.Tools;

    public abstract class HeroNameParserTests
    {
        const string HeroName = "some Hero";

        HeroNameParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetHeroNameParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser
                .Parse(string.Empty)
                .IsValid.ShouldBeFalse();
        }

        [Test]
        public void Parse_HandHistoryWithoutValidHeroNameIndication_IsValidIsFalse()
        {
            _parser
                .Parse("No indication here")
                .IsValid.ShouldBeFalse();
        }

        [Test]
        public void Parse_HandHistoryWithValidHeroName_IsValidIsTrue()
        {
            _parser
                .Parse(GetValidHeroNameSnippetFor(HeroName))
                .IsValid.ShouldBeTrue();
        }

        [Test]
        public void Parse_HandHistoryWithValidHeroName_ExtractsHeroName()
        {
            _parser
                .Parse(GetValidHeroNameSnippetFor(HeroName))
                .HeroName.ShouldBeEqualTo(HeroName);
        }

        protected abstract HeroNameParser GetHeroNameParser();

        protected abstract string GetValidHeroNameSnippetFor(string heroName);
    }
}