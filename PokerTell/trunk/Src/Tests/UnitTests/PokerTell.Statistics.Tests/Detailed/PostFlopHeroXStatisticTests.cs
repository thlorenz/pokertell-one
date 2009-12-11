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

    public class PostFlopHeroXStatisticTests : TestWithLog
    {
        List<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        [SetUp]
        public void _Init()
        {
            _analyzablePokerPlayers = new List<IAnalyzablePokerPlayer>();
        }

        [Test]
        public void UpdateWith_EmptyList_AllMatchingPlayersAreEmpty()
        {
            var sut = new PostFlopHeroXStatistic(Streets.Flop, false);

            sut.UpdateWith(_analyzablePokerPlayers);

            foreach (var matchingPlayer in sut.MatchingPlayers)
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

            var sut = new PostFlopHeroXStatistic(street, inPosition);

            var player = Samples.AnalyzablePokerPlayerWith(id, ActionSequences.HeroX, street, inPosition);
            _analyzablePokerPlayers.Add(player);

            sut.UpdateWith(_analyzablePokerPlayers);

            sut.MatchingPlayers[0].DoesContain(player);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersContainMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;

            var sut = new PostFlopHeroXStatistic(street, inPosition);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, ActionSequences.HeroX, street, inPosition);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, ActionSequences.HeroX + 1, street, inPosition);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            sut.UpdateWith(_analyzablePokerPlayers);

            sut.MatchingPlayers[0].DoesContain(matchingPlayer);
        }

        [Test]
        public void UpdateWith_TwoPlayersOneMatchingPlayer_MatchingPlayersDoNotContainNotMatchingPlayer()
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;

            var sut = new PostFlopHeroXStatistic(street, inPosition);

            var matchingPlayer = Samples.AnalyzablePokerPlayerWith(id, ActionSequences.HeroX, street, inPosition);
            var notMatchingPlayer = Samples.AnalyzablePokerPlayerWith(id + 1, ActionSequences.HeroX + 1, street, inPosition);

            _analyzablePokerPlayers.Add(matchingPlayer);
            _analyzablePokerPlayers.Add(notMatchingPlayer);

            sut.UpdateWith(_analyzablePokerPlayers);

            sut.MatchingPlayers[0].DoesNotContain(notMatchingPlayer);
        }

        [Test, Sequential]
        public void UpdateWith_PlayerWithHeroXOppBHeroReacts_PlayerIsIncludedInMatchingPlayers(
            [Values(ActionSequences.HeroXOppBHeroF, ActionSequences.HeroXOppBHeroC, ActionSequences.HeroXOppBHeroR)]
            ActionSequences actionSequence)
        {
            const long id = 1;
            const bool inPosition = false;
            const Streets street = Streets.Flop;

            var sut = new PostFlopHeroXStatistic(street, inPosition);

            var player = Samples.AnalyzablePokerPlayerWith(id, actionSequence, street, inPosition);
            _analyzablePokerPlayers.Add(player);

            sut.UpdateWith(_analyzablePokerPlayers);

            sut.MatchingPlayers[0].DoesContain(player);
        }
    }
}