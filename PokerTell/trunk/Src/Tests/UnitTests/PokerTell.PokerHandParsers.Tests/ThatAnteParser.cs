namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatAnteParser
    {
        #region Constants and Fields

        const double Ante = 1.0;

        AnteParser _parser;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetAnteParser();
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
        public void Parse_HandHistoryWithValidCashGameAnte_IsValidIsTrue()
        {
            string validTotalSeats = ValidCashGameAnte(Ante);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidCashGameAnte_ExtractsAnte()
        {
            string validTotalSeats = ValidCashGameAnte(Ante);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.Ante, Is.EqualTo(Ante));
        }

        [Test]
        public void Parse_HandHistoryWithValidTournamentAnte_ExtractsAnte()
        {
            string validTotalSeats = ValidTournamentAnte(Ante);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.Ante, Is.EqualTo(Ante));
        }

        #endregion

        #region Methods

        protected abstract AnteParser GetAnteParser();

        protected abstract string ValidCashGameAnte(double ante);

        protected abstract string ValidTournamentAnte(double ante);

        #endregion
    }
}