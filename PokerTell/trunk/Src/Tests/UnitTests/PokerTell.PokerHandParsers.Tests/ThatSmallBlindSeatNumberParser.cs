namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatSmallBlindSeatNumberParser
    {
        #region Constants and Fields

        const int SmallBlindSeatNumber = 1;

        SmallBlindSeatNumberParser _parser;

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
        public void Parse_HandHistoryWithoutValidSmallBlindSeatNumber_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithValidSmallBlindSeatNumber_IsValidIsTrue()
        {
            string validSmallBlindSeatNumber = ValidSmallBlindSeatNumber(SmallBlindSeatNumber);
            _parser.Parse(validSmallBlindSeatNumber);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidSmallBlindSeatNumber_ExtractsSmallBlindSeatNumber()
        {
            string validSmallBlindSeatNumber = ValidSmallBlindSeatNumber(SmallBlindSeatNumber);
            _parser.Parse(validSmallBlindSeatNumber);
            Assert.That(_parser.SmallBlindSeatNumber, Is.EqualTo(SmallBlindSeatNumber));
        }

        #endregion

        #region Methods

        protected abstract SmallBlindSeatNumberParser GetSmallBlindPostionParser();

        protected abstract string ValidSmallBlindSeatNumber(int position);

        #endregion
    }
}