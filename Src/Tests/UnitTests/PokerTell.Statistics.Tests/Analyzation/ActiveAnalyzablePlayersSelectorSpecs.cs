namespace PokerTell.Statistics.Tests.Analyzation
{
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Machine.Specifications;

    using Moq;

    using Statistics.Analyzation;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class ActiveAnalyzablePlayersSelectorSpecs
    {
        protected static Mock<IAnalyzablePokerPlayer> _foldingPlayer_Stub;

        protected static Mock<IAnalyzablePokerPlayer> _callingPlayer_Stub;

        protected static ActiveAnalyzablePlayersSelectorSut _sut;

        Establish specContext = () => {
           _foldingPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
           _foldingPlayer_Stub
               .SetupGet(p => p.ActionSequences)
               .Returns(new[] { ActionSequences.HeroF });

            _callingPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
            _callingPlayer_Stub
                .SetupGet(p => p.ActionSequences)
                .Returns(new[] { ActionSequences.HeroC });

            _sut = new ActiveAnalyzablePlayersSelectorSut();
        };

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_an_empty_list : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            Establish context = () => passedPlayers = new List<IAnalyzablePokerPlayer>();

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_an_empty_collection = () => returnedPlayers.ShouldBeEmpty();
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_a_list_containing_a_folding_player : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            Establish context = () => passedPlayers = new List<IAnalyzablePokerPlayer> { _foldingPlayer_Stub.Object };

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_an_empty_collection = () => returnedPlayers.ShouldBeEmpty();
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_a_list_containing_a_raised_folding_player : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            static Mock<IAnalyzablePokerPlayer> raisedFoldingPlayer_Stub;

            Establish context = () => {
                raisedFoldingPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
                raisedFoldingPlayer_Stub
                    .SetupGet(p => p.ActionSequences)
                    .Returns(new[] { ActionSequences.OppRHeroF });
                passedPlayers = new List<IAnalyzablePokerPlayer> { raisedFoldingPlayer_Stub.Object };
            };

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_an_empty_collection = () => returnedPlayers.ShouldBeEmpty();
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_a_list_containing_a_player_with_non_standard_actionsequence : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            static Mock<IAnalyzablePokerPlayer> nonStandardPlayer_Stub;

            Establish context = () => {
                nonStandardPlayer_Stub = new Mock<IAnalyzablePokerPlayer>();
                nonStandardPlayer_Stub
                    .SetupGet(p => p.ActionSequences)
                    .Returns(new[] { ActionSequences.NonStandard });
                passedPlayers = new List<IAnalyzablePokerPlayer> { nonStandardPlayer_Stub.Object };
            };

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_an_empty_collection = () => returnedPlayers.ShouldBeEmpty();
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_a_list_containing_a_calling_player : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            Establish context = () => passedPlayers = new List<IAnalyzablePokerPlayer> { _callingPlayer_Stub.Object };

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_a_collection_containing_only_the_calling_player = () => returnedPlayers.ShouldContainOnly(_callingPlayer_Stub.Object);
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_a_list_containing_a_calling_and_a_folding_player : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> passedPlayers;
            
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            Establish context = () => passedPlayers = new List<IAnalyzablePokerPlayer> { _callingPlayer_Stub.Object, _foldingPlayer_Stub.Object };

            Because of = () => returnedPlayers = _sut.SelectFrom(passedPlayers);

            It should_return_a_collection_containing_only_the_calling_player = () => returnedPlayers.ShouldContainOnly(_callingPlayer_Stub.Object);
        }


        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_non_null_playerstatistics : ActiveAnalyzablePlayersSelectorSpecs
        {
            static Mock<IPlayerStatistics> playerStatistics_Stub;

            static IEnumerable<IAnalyzablePokerPlayer> filteredPlayers;
            
            Establish context = () => {
                filteredPlayers = new List<IAnalyzablePokerPlayer> { _foldingPlayer_Stub.Object, _callingPlayer_Stub.Object };
  
                playerStatistics_Stub = new Mock<IPlayerStatistics>();
                playerStatistics_Stub
                    .SetupGet(ps => ps.FilteredAnalyzablePokerPlayers)
                    .Returns(filteredPlayers);
            };

            Because of = () => _sut.SelectFrom(playerStatistics_Stub.Object);

            It should_select_the_analyzable_players_from_the_filtered_analyzable_players_of_the_player_statistics = () => {
                    _sut.ActivePlayersWereSelectedFromAnalyzablePokerPlayers.ShouldBeTrue();
                    _sut.PlayersThatActivePlayersWereSelectedFrom.ShouldEqual(filteredPlayers);
                };
        }

        [Subject(typeof(ActiveAnalyzablePlayersSelector), "SelectFrom")]
        public class when_told_to_select_active_players_from_null_playerstatistics : ActiveAnalyzablePlayersSelectorSpecs
        {
            static IEnumerable<IAnalyzablePokerPlayer> returnedPlayers;

            Because of = () => returnedPlayers = _sut.SelectFrom((IPlayerStatistics) null);

            It should_not_select_the_analyzable_players_from_the_filtered_analyzable_players_of_the_player_statistics
                = () => _sut.ActivePlayersWereSelectedFromAnalyzablePokerPlayers.ShouldBeFalse();

            It should_return_empty = () => returnedPlayers.ShouldBeEmpty();
        }
    }

    public class ActiveAnalyzablePlayersSelectorSut : ActiveAnalyzablePlayersSelector
    {

        public IEnumerable<IAnalyzablePokerPlayer> PlayersThatActivePlayersWereSelectedFrom;

        public bool ActivePlayersWereSelectedFromAnalyzablePokerPlayers;

        public override IEnumerable<IAnalyzablePokerPlayer> SelectFrom(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            ActivePlayersWereSelectedFromAnalyzablePokerPlayers = true;
            PlayersThatActivePlayersWereSelectedFrom = analyzablePokerPlayers;
            return base.SelectFrom(analyzablePokerPlayers);
        }
    }
}