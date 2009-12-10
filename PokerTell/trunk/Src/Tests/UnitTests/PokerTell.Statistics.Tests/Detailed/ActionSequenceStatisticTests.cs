namespace PokerTell.Statistics.Tests.Detailed
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Detailed;

    using UnitTests.Tools;

    public class ActionSequenceStatisticTests
    {
        ActionSequenceStatisticMock _sut;

        StubBuilder _stub;

        [SetUp]
        public void _Init()
        {
            _stub = new StubBuilder();
            _sut = new ActionSequenceStatisticMock();
        }

        [Test]
        public void TotalCounts_AllMatchingPlayersAreEmpty_ReturnsZero()
        {
            _sut.SetMatchingPlayers(new[]
                {
                    new List<IAnalyzablePokerPlayer>(), new List<IAnalyzablePokerPlayer>()
                }).CalculateCounts();

            _sut.TotalCounts.IsEqualTo(0);
        }

        [Test]
        public void TotalCounts_FirstMatchingPlayersEmptySecondMatchingPlayersContainOne_ReturnsOne()
        {
            _sut.SetMatchingPlayers(new[]
                {
                    new List<IAnalyzablePokerPlayer>(),
                    new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() }
                }).CalculateCounts();

            _sut.TotalCounts.IsEqualTo(1);
        }

        [Test]
        public void TotalCounts_FirstMatchingPlayersContainOneSecondMatchingPlayersContainOne_ReturnsTwo()
        {
            _sut.SetMatchingPlayers(new[]
                {
                    new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() },
                    new List<IAnalyzablePokerPlayer> { _stub.Out<IAnalyzablePokerPlayer>() }
                }).CalculateCounts();

            _sut.TotalCounts.IsEqualTo(2);
        }

        [Test]
        public void UpdateWith_Always_ExtractsMatchingPlayers()
        {
            _sut.UpdateWith(_stub.Out<IEnumerable<IAnalyzablePokerPlayer>>());

            _sut.MatchingPlayersWereExtracted.IsTrue();
        }

        [Test]
        public void UpdateWith_Always_CalculatesTotalCounts()
        {
            _sut.UpdateWith(_stub.Out<IEnumerable<IAnalyzablePokerPlayer>>());

            _sut.TotalCountsWereCalculated.IsTrue();
        }
    }
    
    class ActionSequenceStatisticMock : ActionSequenceStatistic
    {
        public ActionSequenceStatisticMock()
            : base(ActionSequences.NonStandard, Streets.Flop, 0)
        {
            MatchingPlayersWereExtracted = false;
            TotalCountsWereCalculated = false;
        }

        protected override void ExtractMatchingPlayers(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            MatchingPlayersWereExtracted = true;
        }

        internal bool MatchingPlayersWereExtracted { get; set; }

        internal bool TotalCountsWereCalculated { get; set; }

        public ActionSequenceStatisticMock SetMatchingPlayers(IList<IAnalyzablePokerPlayer>[] matchingPlayers)
        {
            MatchingPlayers = matchingPlayers;

            return this;
        }

        public ActionSequenceStatisticMock CalculateCounts()
        {
            base.CalculateTotalCounts();
            return this;
        }

        protected override void CalculateTotalCounts()
        {
            TotalCountsWereCalculated = true;
        }
    }

}