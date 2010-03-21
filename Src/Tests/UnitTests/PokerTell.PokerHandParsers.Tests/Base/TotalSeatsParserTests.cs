namespace PokerTell.PokerHandParsers.Tests.Base
{
    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    public abstract class TotalSeatsParserTests
    {
        TotalSeatsParser _parser;

        [SetUp]
        public void _Init()
        {
            _parser = GetTotalSeatsParser();
        }

        [Test]
        public virtual void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public virtual void Parse_HandHistoryWithoutValidTotalSeats_IsValidIsFalse()
        {
            _parser.Parse("this is invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        [Sequential]
        public void Parse_HandHistoryWithValidTotalSeats_IsValidIsTrue(
            [Values(2, 6, 9, 10)] int numberOfTotalSeats)
        {
            string validTotalSeats = ValidTotalSeats(numberOfTotalSeats);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        [Sequential]
        public void Parse_HandHistoryWithValidTotalSeats_ExtractsTotalSeats(
            [Values(2, 6, 9, 10)] int numberOfTotalSeats)
        {
            string validTotalSeats = ValidTotalSeats(numberOfTotalSeats);
            _parser.Parse(validTotalSeats);
            Assert.That(_parser.TotalSeats, Is.EqualTo(numberOfTotalSeats));
        }

        protected abstract TotalSeatsParser GetTotalSeatsParser();

        protected abstract string ValidTotalSeats(int position);
    }
}