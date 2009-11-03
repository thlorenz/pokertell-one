namespace PokerTell.PokerHandParsers.Tests
{
    using Base;

    using NUnit.Framework;

    public abstract class ThatSmallBlindPlayerNameParser
    {
        #region Constants and Fields

        const string SmallBlindPlayerName = "smallBlindHolder";

        SmallBlindPlayerNameParser _parser;

        #endregion

        #region Public Methods

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

        #endregion

        #region Methods

        protected abstract SmallBlindPlayerNameParser GetSmallBlindPostionParser();

        protected abstract string ValidSmallBlindSeatNumber(string name);

        #endregion
    }
}