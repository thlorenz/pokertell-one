namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatTotalSeatsParser
    {
        #region Constants and Fields

        const int TotalSeats = 9;

        TotalSeatsParser _parser;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetTotalSeatsParser();
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
            string validTotalSeats = ValidTotalSeats(TotalSeats);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidSmallBlindSeatNumber_ExtractsSmallBlindSeatNumber()
        {
            string validTotalSeats = ValidTotalSeats(TotalSeats);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.TotalSeats, Is.EqualTo(TotalSeats));
        }

        #endregion

        #region Methods

        protected abstract TotalSeatsParser GetTotalSeatsParser();

        protected abstract string ValidTotalSeats(int position);

        #endregion
    }
}