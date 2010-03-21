namespace PokerTell.PokerHandParsers.Tests.Base
{
    using NUnit.Framework;

    using PokerTell.PokerHandParsers.Base;

    public abstract class StreetsParserTests
    {
        const string Flop = "some Flop Contents";

        const string Preflop = "some Preflop Contents";

        const string River = "some River Contents";

        const string Turn = "some Turn Contents";

        StreetsParser _parser;

        bool HasFlopAndTurn
        {
            get { return _parser.HasFlop && _parser.HasTurn; }
        }

        bool HasFlopAndTurnAndRiver
        {
            get { return _parser.HasFlop && _parser.HasTurn && _parser.HasRiver; }
        }

        bool HasFlopOrTurnOrRiver
        {
            get { return _parser.HasFlop || _parser.HasTurn || _parser.HasRiver; }
        }

        bool HasTurnOrRiver
        {
            get { return _parser.HasTurn || _parser.HasRiver; }
        }

        [SetUp]
        public void _Init()
        {
            _parser = GetStreetsParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_EmptyString_StreetsAreEmpty()
        {
            _parser.Parse(string.Empty);
            bool streetsAreEmpty = string.IsNullOrEmpty(_parser.Preflop + _parser.Flop + _parser.Turn + _parser.River);
            Assert.That(streetsAreEmpty, Is.True);
        }

        [Test]
        public void Parse_WithoutSummary_IsValidIsFalse()
        {
            _parser.Parse("someStringMissingTheSummaryPattern");
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_WithPreflopAndSummaryOnly_ExtractsPreflop()
        {
            string handHistory = PreflopAndSummaryOnly(Preflop);
            _parser.Parse(handHistory);
            Assert.That(_parser.Preflop, Is.EqualTo(Preflop));
        }

        [Test]
        public void Parse_WithPreflopAndSummaryOnly_HasFlopTurnRiverAreFalse()
        {
            string handHistory = PreflopAndSummaryOnly(Preflop);
            _parser.Parse(handHistory);
            Assert.That(HasFlopOrTurnOrRiver, Is.False);
        }

        [Test]
        public void Parse_WithPreflopAndSummaryOnly_IsValidIsTrue()
        {
            string handHistory = PreflopAndSummaryOnly(Preflop);
            _parser.Parse(handHistory);
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_ExtractedFlopDoesNotContainPreflop()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(_parser.Flop, Is.Not.StringContaining(Preflop));
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_ExtractedPreflopDoesNotContainFlop()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(_parser.Preflop, Is.Not.StringContaining(Flop));
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_ExtractsFlop()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(_parser.Flop, Is.StringContaining(Flop));
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_ExtractsPreflop()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(_parser.Preflop, Is.StringContaining(Preflop));
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_HasFlopIsTrue()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(_parser.HasFlop, Is.True);
        }

        [Test]
        public void Parse_WithPreflopFlopAndSummary_HasTurnRiverAreFalse()
        {
            string handHistory = PreflopFlopAndSummary(Preflop, Flop);
            _parser.Parse(handHistory);
            Assert.That(HasTurnOrRiver, Is.False);
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractedFlopDoesNotContainOtherStreets(
            [Values(Preflop, Turn)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);
            Assert.That(_parser.Flop, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractedPreflopDoesNotContainOtherStreets(
            [Values(Flop, Turn)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);
            Assert.That(_parser.Preflop, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractedTurnDoesNotContainOtherStreets(
            [Values(Preflop, Flop)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);
            Assert.That(_parser.Turn, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractsFlop()
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);

            Assert.That(_parser.Flop, Is.StringContaining(Flop));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractsPreflop()
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);

            Assert.That(_parser.Preflop, Is.StringContaining(Preflop));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnAndSummary_ExtractsTurn()
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);

            Assert.That(_parser.Turn, Is.StringContaining(Turn));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnAndSummary_HasFlopTurnAreTrue()
        {
            string handHistory = PreflopFlopTurnAndSummary(Preflop, Flop, Turn);
            _parser.Parse(handHistory);

            Assert.That(HasFlopAndTurn, Is.True);
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractedFlopDoesNotContainOtherStreets(
            [Values(Preflop, Turn, River)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);
            Assert.That(_parser.Flop, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractedPreflopDoesNotContainOtherStreets(
            [Values(Flop, Turn, River)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);
            Assert.That(_parser.Preflop, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractedRiverDoesNotContainOtherStreets(
            [Values(Preflop, Flop, Turn)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(_parser.River, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        [Sequential]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractedTurnDoesNotContainOtherStreets(
            [Values(Preflop, Flop, River)] string otherStreet)
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);
            Assert.That(_parser.Turn, Is.Not.StringContaining(otherStreet));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractsFlop()
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(_parser.Flop, Is.StringContaining(Flop));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractsPreflop()
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(_parser.Preflop, Is.StringContaining(Preflop));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractsRiver()
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(_parser.River, Is.StringContaining(River));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_ExtractsTurn()
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(_parser.Turn, Is.StringContaining(Turn));
        }

        [Test]
        public void Parse_WithPreflopFlopTurnRiverAndSummary_HasFlopTurnRiverAreTrue()
        {
            string handHistory = PreflopFlopTurnRiverAndSummary(Preflop, Flop, Turn, River);
            _parser.Parse(handHistory);

            Assert.That(HasFlopAndTurnAndRiver, Is.True);
        }

        [Test]
        public void Parse_WithPreflopTurnAndSummaryOnly_HasFlopTurnRiverAreFalse()
        {
            string handHistory = PreflopTurnAndSummaryOnly(Preflop);
            _parser.Parse(handHistory);

            Assert.That(HasFlopOrTurnOrRiver, Is.False);
        }

        protected abstract StreetsParser GetStreetsParser();

        protected abstract string PreflopAndSummaryOnly(string preflop);

        protected abstract string PreflopFlopAndSummary(string preflop, string flop);

        protected abstract string PreflopFlopTurnAndSummary(string preflop, string flop, string turn);

        protected abstract string PreflopFlopTurnRiverAndSummary(string preflop, string flop, string turn, string river);

        protected abstract string PreflopTurnAndSummaryOnly(string preflop);
    }
}