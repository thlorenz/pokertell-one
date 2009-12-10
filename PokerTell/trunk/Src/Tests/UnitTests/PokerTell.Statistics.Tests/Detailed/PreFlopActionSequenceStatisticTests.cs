namespace PokerTell.Statistics.Tests.Detailed
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Detailed;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.Tests.Factories;
    using PokerTell.UnitTests.Tools;

    using UnitTests;

    [TestFixture]
    public class PreFlopActionSequenceStatisticTests : TestWithLog
    {
        #region Constants and Fields

        List<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        IActionSequenceStatistic _sut;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _analyzablePokerPlayers = new List<IAnalyzablePokerPlayer>();
        }

        [Test]
        public void UpdateWith_EmptyList_AllMatchingPlayersAreEmpty()
        {
            _sut = new PreFlopActionSequenceStatistic(ActionSequences.HeroB);

            _sut.UpdateWith(_analyzablePokerPlayers);

            foreach (var matchingPlayer in _sut.MatchingPlayers)
            {
                matchingPlayer.IsEmpty();
            }
        }

        [Test]
        public void UpdateWith_OneMatchingPlayer_MatchingPlayersForGivenPositionContainPlayer()
        {
            const long id = 1;
            const Streets street = Streets.PreFlop;
            const ActionSequences actionSequence = ActionSequences.HeroC;
            const StrategicPositions position = StrategicPositions.CO;

            _sut = new PreFlopActionSequenceStatistic(actionSequence);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, position);
            _analyzablePokerPlayers.Add(player);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[(int)position].DoesContain(player);
        }

        [Test]
        public void UpdateWith_OneMatchingPlayer_MatchingPlayersForOtherPositionDoNotContainPlayer()
        {
            const long id = 1;
            const Streets street = Streets.PreFlop;
            const ActionSequences actionSequence = ActionSequences.HeroC;
            const StrategicPositions position = StrategicPositions.CO;

            _sut = new PreFlopActionSequenceStatistic(actionSequence);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, position);
            _analyzablePokerPlayers.Add(player);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[(int)position + 1].DoesNotContain(player);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersForGivenPositionContainMatchingPlayer()
        {
            const long id = 1;
            const Streets street = Streets.PreFlop;
            const ActionSequences actionSequence = ActionSequences.HeroC;
            const StrategicPositions position = StrategicPositions.CO;

            _sut = new PreFlopActionSequenceStatistic(actionSequence);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, position);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, position);
            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[(int)position].DoesContain(matchingPlayer);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersForGivenPositionDoNotContainNotMatchingPlayer()
        {
            const long id = 1;
            const Streets street = Streets.PreFlop;
            const ActionSequences actionSequence = ActionSequences.HeroC;
            const StrategicPositions position = StrategicPositions.CO;

            _sut = new PreFlopActionSequenceStatistic(actionSequence);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, position);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, actionSequence + 1, street, position);
            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            _sut.UpdateWith(_analyzablePokerPlayers);

            _sut.MatchingPlayers[(int)position].DoesNotContain(notMatchingPlayer);
        }

        #endregion
    }
}