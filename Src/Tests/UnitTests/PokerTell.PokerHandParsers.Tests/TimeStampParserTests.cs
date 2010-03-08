namespace PokerTell.PokerHandParsers.Tests
{
    using System;

    using Base;

    using NUnit.Framework;

    using UnitTests.Tools;

    public abstract class TimeStampParserTests
    {
        TimeStampParser _parser;

        DateTime _timeStamp;

        [SetUp]
        public void _Init()
        {
            _parser = GetTimeStampParser();
            _timeStamp = DateTime.MinValue.AddYears(1);
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithoutValidTimeStamp_IsValidIsFalse()
        {
            _parser.Parse("invalid");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_HandHistoryWithValidTimeStamp_IsValidIsTrue()
        {
            var validTimeStamp = ValidTimeStamp(_timeStamp);
            _parser.Parse(validTimeStamp);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_HandHistoryWithValidTimeStamp_ExtractsTimeStamp()
        {
            var validTimeStamp = ValidTimeStamp(_timeStamp);
            _parser.Parse(validTimeStamp);
            Assert.That(_parser.TimeStamp, Is.EqualTo(_timeStamp));
        }
        
        protected abstract TimeStampParser GetTimeStampParser();

        protected abstract string ValidTimeStamp(DateTime timeStamp);
    }
}