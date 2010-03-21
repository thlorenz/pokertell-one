namespace PokerTell.PokerHandParsers.Tests.Base
{
    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    public abstract class SmallBlindPlayerNameParserTests
    {
        const string SmallBlindPlayerName = "smallBlindHolder";

        SmallBlindPlayerNameParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetSmallBlindPostionParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithoutValidSmallBlind_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithValidSmallBlind_IsValidIsTrue()
        {
            string validSmallBlindText = ValidSmallBlindSeatNumber(SmallBlindPlayerName);
            _parser.Parse(validSmallBlindText);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidSmallBlind_ExtractsSmallBlindPlayerName()
        {
            string validSmallBlindPlayerName = ValidSmallBlindSeatNumber(SmallBlindPlayerName);
            _parser.Parse(validSmallBlindPlayerName);
            Assert.That(_parser.SmallBlindPlayerName, Is.EqualTo(SmallBlindPlayerName));
        }

        protected abstract SmallBlindPlayerNameParser GetSmallBlindPostionParser();

        protected abstract string ValidSmallBlindSeatNumber(string name);
    }
}