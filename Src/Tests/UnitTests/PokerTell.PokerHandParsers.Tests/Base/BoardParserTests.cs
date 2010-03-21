namespace PokerTell.PokerHandParsers.Tests.Base
{
    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    public abstract class BoardParserTests
    {
        const string Board = "As Kh 9d";

        BoardParser _parser;

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

        protected abstract string ValidBoard(string board);

        protected abstract BoardParser GetBoardParser();
    }
}