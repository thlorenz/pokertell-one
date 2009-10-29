namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatBlindsParser
    {
        #region Constants and Fields

        const double BigBlind = 1.0;

        const double SmallBlind = 0.5;

        BlindsParser _parser;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetBlindsParser();
        }

        [Test]
        public void Parse_CashGameWithValidBlinds_IsValidIsTrue()
        {
            string handHistory = CashGameWithValidBlinds(SmallBlind, BigBlind);
            _parser.Parse(handHistory);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_CashGameWithValidBlinds_ExtractsSmallBlind()
        {
            string handHistory = CashGameWithValidBlinds(SmallBlind, BigBlind);
            _parser.Parse(handHistory);
            Assert.That(_parser.SmallBlind, Is.EqualTo(SmallBlind));
        }

        [Test]
        public void Parse_CashGameWithValidBlinds_ExtractsBigBlind()
        {
            string handHistory = CashGameWithValidBlinds(SmallBlind, BigBlind);
            _parser.Parse(handHistory);
            Assert.That(_parser.BigBlind, Is.EqualTo(BigBlind));
        }

        [Test]
        public void Parse_TournamentGameWithValidBlinds_ExtractsSmallBlind()
        {
            string handHistory = TournamentGameWithValidBlinds(SmallBlind, BigBlind);
            _parser.Parse(handHistory);
            Assert.That(_parser.SmallBlind, Is.EqualTo(SmallBlind));
        }

        [Test]
        public void Parse_TournamentGameWithValidBlinds_ExtractsBigBlind()
        {
            string handHistory = TournamentGameWithValidBlinds(SmallBlind, BigBlind);
            _parser.Parse(handHistory);
            Assert.That(_parser.BigBlind, Is.EqualTo(BigBlind));
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithoutValidBlinds_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        #endregion

        #region Methods

        protected abstract string CashGameWithValidBlinds(double smallBlind, double bigBlind);

        protected abstract string TournamentGameWithValidBlinds(double smallBlind, double bigBlind);

        protected abstract BlindsParser GetBlindsParser();

        #endregion
    }
}