namespace PokerTell.PokerHandParsers.Tests.Base
{
    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    using UnitTests.Tools;

    public abstract class GameTypeParserTests
    {
        GameTypeParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetGameTypeParser();
        }

        [Test]
        public void Parsing_InvalidHeader_IsValidIsFalse()
        {
            _parser
                .Parse("invalid header")
                .IsValid
                .ShouldBeFalse();
        }

        [Test]
        public void Parsing_LimitGameHeader_IsValidIsTrue()
        {
            _parser
                .Parse(LimitGameHeader())
                .IsValid
                .ShouldBeTrue();
        }

        [Test]
        public void Parsing_LimitGameHeader_ExtractsLimit()
        {
            _parser
                .Parse(LimitGameHeader())
                .GameType
                .ShouldBeEqualTo(GameTypes.Limit);
        }

        [Test]
        public void Parsing_PotLimitGameHeader_IsValidIsTrue()
        {
            _parser
                .Parse(PotLimitGameHeader())
                .IsValid
                .ShouldBeTrue();
        }

        [Test]
        public void Parsing_PotLimitGameHeader_ExtractsPotLimit()
        {
            _parser
                .Parse(PotLimitGameHeader())
                .GameType
                .ShouldBeEqualTo(GameTypes.PotLimit);
        }

        [Test]
        public void Parsing_NoLimitGameHeader_IsValidIsTrue()
        {
            _parser
                .Parse(NoLimitGameHeader())
                .IsValid
                .ShouldBeTrue();
        }

        [Test]
        public void Parsing_NoLimitGameHeader_ExtractsNoLimit()
        {
            _parser
                .Parse(NoLimitGameHeader())
                .GameType
                .ShouldBeEqualTo(GameTypes.NoLimit);
        }

        protected abstract string LimitGameHeader();

        protected abstract string PotLimitGameHeader();

        protected abstract string NoLimitGameHeader();

        protected abstract GameTypeParser GetGameTypeParser();
    }
}