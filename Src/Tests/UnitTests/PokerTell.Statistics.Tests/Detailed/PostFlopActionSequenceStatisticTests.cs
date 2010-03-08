namespace PokerTell.Statistics.Tests.Detailed
{
    using System.Collections.Generic;

    using Factories;

    using Infrastructure.Interfaces.Statistics;

    using NUnit.Framework;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Detailed;
    using PokerTell.UnitTests;
    using PokerTell.UnitTests.Tools;

    [TestFixture]
    public class PostFlopActionSequenceStatisticTests : TestWithLog
    {
        IActionSequenceStatistic _sut;

        const int indexesCount = 4;

        List<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        [SetUp]
        public void _Init()
        {
            _analyzablePokerPlayers = new List<IAnalyzablePokerPlayer>();
        }

        [Test]
        public void Constructor_InitializesCollections()
        {
            _sut = new PostFlopActionSequenceStatistic(ActionSequences.HeroB, Streets.Flop, false, indexesCount);

            _sut.Percentages.ShouldNotBeEmpty();
            _sut.MatchingPlayers.ShouldNotBeEmpty();
        }

        [Test]
        public void Constructor_InitializesAllMatchingPlayers()
        {
            _sut = new PostFlopActionSequenceStatistic(ActionSequences.HeroB, Streets.Flop, false, indexesCount);
            foreach (var matchingPlayer in _sut.MatchingPlayers)
            {
                matchingPlayer.ShouldNotBeNull();
            }
        }

        [Test]
        public void UpdateWith_EmptyList_AllMatchingPlayersAreEmpty()
        {
            _sut = new PostFlopActionSequenceStatistic(ActionSequences.HeroB, Streets.Flop, false, indexesCount);

            _sut.UpdateWith(_analyzablePokerPlayers);

            foreach (var matchingPlayer in _sut.MatchingPlayers)
            {
                matchingPlayer.ShouldBeEmpty();
            }
        }

        [Test]
        public void UpdateWith_OneMatchingPlayer_MatchingPlayersForGivenBetSizeContainPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.OppBHeroC;
            const int betSizeIndex = 1;

            _sut = new PostFlopActionSequenceStatistic(actionSequence, street, inPosition, indexesCount);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition, betSizeIndex);
            _analyzablePokerPlayers.Add(player);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[betSizeIndex].ShouldContain(player);
        }

        [Test]
        public void UpdateWith_OneMatchingPlayer_MatchingPlayersForOtherBetSizeDoNotContainPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.OppBHeroC;
            const int betSizeIndex = 1;

            _sut = new PostFlopActionSequenceStatistic(actionSequence, street, inPosition, indexesCount);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition, betSizeIndex);
            _analyzablePokerPlayers.Add(player);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[betSizeIndex + 1].ShouldNotContain(player);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersForGivenBetSizeContainMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.OppBHeroC;
            const int betSizeIndex = 1;

            _sut = new PostFlopActionSequenceStatistic(actionSequence, street, inPosition, indexesCount);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition, betSizeIndex);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, inPosition, betSizeIndex);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[betSizeIndex].ShouldContain(matchingPlayer);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersForGivenBetSizeDoNotContainNotMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.OppBHeroC;
            const int betSizeIndex = 1;

            _sut = new PostFlopActionSequenceStatistic(actionSequence, street, inPosition, indexesCount);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition, betSizeIndex);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, inPosition, betSizeIndex);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[betSizeIndex].ShouldNotContain(notMatchingPlayer);
        }
    }
}