namespace PokerTell.PokerHandParsers.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    public abstract class ThatHandHeaderParser
    {
        #region Constants and Fields

        const int GameId = 1;

        const int TournamentId = 2;

        HandHeaderParser _parser;

        #endregion

        #region Properties

        protected abstract string SiteName { get; }

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _parser = GetHandHeaderParser();
        }

        [Test]
        public void Parse_EmptyString_IsValidIsFalse()
        {
            _parser.Parse(string.Empty);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_StringContainsSiteOnly_IsValidIsFalse()
        {
            _parser.Parse(SiteName);
            Assert.That(_parser.IsValid, Is.False);
        }

        [Test]
        public void Parse_ValidNoLimitHoldemCashGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidNoLimitHoldemCashGameHeader(GameId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidNoLimitHoldemCashGameHeader_SetsTournamentIdToZero()
        {
            _parser.Parse(ValidNoLimitHoldemCashGameHeader(GameId));
            Assert.That(_parser.TournamentId, Is.EqualTo(0));
        }

        [Test]
        public void Parse_ValidNoLimitHoldemCashGameHeader_IsValidIsTrue()
        {
            _parser.Parse(ValidNoLimitHoldemCashGameHeader(GameId));
            Assert.That(_parser.IsValid, Is.True);
        }

        [Test]
        public void Parse_ValidLimitHoldemCashGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidLimitHoldemCashGameHeader(GameId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidPotLimitHoldemCashGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidPotLimitHoldemCashGameHeader(GameId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidNoLimitHoldemTournamentGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidNoLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidNoLimitHoldemTournamentGameHeader_IsTournamentIsTrue()
        {
            _parser.Parse(ValidNoLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.IsTournament, Is.True);
        }

        [Test]
        public void Parse_ValidNoLimitHoldemTournamentGameHeader_ExtractsTournamentId()
        {
            _parser.Parse(ValidNoLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.TournamentId, Is.EqualTo(TournamentId));
        }

         [Test]
        public void Parse_ValidLimitHoldemTournamentGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidLimitHoldemTournamentGameHeader_ExtractsTournamentId()
        {
            _parser.Parse(ValidLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.TournamentId, Is.EqualTo(TournamentId));
        }

        [Test]
        public void Parse_ValidPotLimitHoldemTournamentGameHeader_ExtractsGameId()
        {
            _parser.Parse(ValidPotLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.GameId, Is.EqualTo(GameId));
        }

        [Test]
        public void Parse_ValidPotLimitHoldemTournamentGameHeader_ExtractsTournamentId()
        {
            _parser.Parse(ValidPotLimitHoldemTournamentGameHeader(GameId, TournamentId));
            Assert.That(_parser.TournamentId, Is.EqualTo(TournamentId));
        }

        [Test]
        public void FindAllHeaders_EmptyString_ReturnsEmptyMatches()
        {
            var headers = _parser.FindAllHeaders(string.Empty);
            Assert.That(headers.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAllHeaders_StringContainsSiteOnly_ReturnsEmptyMatches()
        {
            var headers = _parser.FindAllHeaders(SiteName);
            Assert.That(headers.Count, Is.EqualTo(0));
        }

        [Test]
        public void FindAllHeaders_ValidNoLimitCashGameHeader_ReturnsOneMatch()
        {
            var headers = _parser.FindAllHeaders(ValidNoLimitHoldemCashGameHeader(GameId));
            Assert.That(headers.Count, Is.EqualTo(1));
        }

        [Test]
        public void FindAllHeaders_TwoValidNoLimitCashGameHeaders_ReturnsTwoMatches()
        {
            var handHistories = ValidNoLimitHoldemCashGameHeader(GameId) + "\n" +
                                ValidNoLimitHoldemCashGameHeader(GameId + 1);
            var headers = _parser.FindAllHeaders(handHistories);
            Assert.That(headers.Count, Is.EqualTo(2));
        }

        [Test]
        public void FindAllHeaders_TwoValidNoLimitCashGameHeaders_SecondMatchIndexIsGreaterThanFirstMatchIndex()
        {
            var handHistories = ValidNoLimitHoldemCashGameHeader(GameId) + "\n" +
                                ValidNoLimitHoldemCashGameHeader(GameId + 1);
            var headers = _parser.FindAllHeaders(handHistories);
            Assert.That(headers[1].HeaderMatch.Index, Is.GreaterThan(headers[0].HeaderMatch.Index));
        }

        #endregion

        #region Methods

        protected abstract HandHeaderParser GetHandHeaderParser();

        protected abstract string ValidNoLimitHoldemCashGameHeader(ulong gameId);

        protected abstract string ValidLimitHoldemCashGameHeader(ulong gameId);

        protected abstract string ValidPotLimitHoldemCashGameHeader(ulong gameId);

        protected abstract string ValidNoLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId);

        protected abstract string ValidLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId);

        protected abstract string ValidPotLimitHoldemTournamentGameHeader(ulong gameId, ulong tournamentId);

        #endregion
    }
}