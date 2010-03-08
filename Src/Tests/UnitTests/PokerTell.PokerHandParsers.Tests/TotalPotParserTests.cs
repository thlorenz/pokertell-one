namespace PokerTell.PokerHandParsers.Tests
{
    using Base;

    using NUnit.Framework;

    public abstract class TotalPotParserTests
    {
        #region Constants and Fields

        const double TotalPot = 1.0;

        TotalPotParser _parser;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetTotalPotParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithoutValidTotalPot_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithValidCashGameTotalPot_IsValidIsTrue()
        {
            string validTotalSeats = ValidCashGameTotalPot(TotalPot);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidCashGameTotalPot_ExtractsTotalPot()
        {
            string validTotalSeats = ValidCashGameTotalPot(TotalPot);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.TotalPot, Is.EqualTo(TotalPot));
        }

        [Test]
        public void Parse_HandHistoryWithValidTournamentTotalPot_ExtractsTotalPot()
        {
            string validTotalSeats = ValidTournamentTotalPot(TotalPot);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.TotalPot, Is.EqualTo(TotalPot));
        }

        #endregion

        #region Methods

        protected abstract TotalPotParser GetTotalPotParser();

        protected abstract string ValidCashGameTotalPot(double totalPot);

        protected abstract string ValidTournamentTotalPot(double totalPot);

        #endregion
    }
}