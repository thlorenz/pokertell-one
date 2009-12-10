namespace PokerTell.Statistics.Tests.Detailed
{
    using System.Collections.Generic;

    using Factories;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using NUnit.Framework;

    using Statistics.Detailed;

    using UnitTests;
    using UnitTests.Tools;

    public class PostFlopActionSequenceWithoutIndexesStatisticTests : TestWithLog
    {

        IActionSequenceStatistic _sut;

        List<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        [SetUp]
        public void _Init()
        {
            _analyzablePokerPlayers = new List<IAnalyzablePokerPlayer>();
        }

        [Test]
        public void UpdateWith_EmptyList_AllMatchingPlayersAreEmpty()
        {
            _sut = new PostFlopActionSequenceWithoutIndexesStatistic(ActionSequences.HeroB, Streets.Flop, false);

            _sut.UpdateWith(_analyzablePokerPlayers);

            foreach (var matchingPlayer in _sut.MatchingPlayers)
            {
                matchingPlayer.IsEmpty();
            }
        }

        [Test]
        public void UpdateWith_OneMatchingPlayer_MatchingPlayersForGivenBetSizeContainPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.HeroB;

            _sut = new PostFlopActionSequenceWithoutIndexesStatistic(actionSequence, street, inPosition);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition);
            _analyzablePokerPlayers.Add(player);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[0].DoesContain(player);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersContainMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.HeroB;

            _sut = new PostFlopActionSequenceWithoutIndexesStatistic(actionSequence, street, inPosition);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, inPosition);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[0].DoesContain(matchingPlayer);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersDoNotContainNotMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;
            const ActionSequences actionSequence = ActionSequences.HeroB;

            _sut = new PostFlopActionSequenceWithoutIndexesStatistic(actionSequence, street, inPosition);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, inPosition);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[0].DoesNotContain(notMatchingPlayer);
        }
    }
}