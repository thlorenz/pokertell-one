namespace PokerTell.PokerHandParsers.Tests
{
    using NUnit.Framework;

    public abstract class ThatBoardParser
    {
        #region Constants and Fields

        const string Board = "As Kh 9d";

        BoardParser _parser;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetBoardParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithoutValidTableName_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithValidTableName_IsValidIsTrue()
        {
            var validTableName = ValidBoard(Board);
            _parser.Parse(validTableName);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidTableName_ExtractsBoard()
        {
            var validTableName = ValidBoard(Board);
            _parser.Parse(validTableName);
            Assert.That(_parser.Board, Is.EqualTo(Board));
        }

        #endregion

        #region Methods

        protected abstract string ValidBoard(string board);

        protected abstract BoardParser GetBoardParser();

        #endregion
    }
}