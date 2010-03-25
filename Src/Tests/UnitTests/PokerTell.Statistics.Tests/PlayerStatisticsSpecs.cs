namespace PokerTell.Statistics.Tests
{
    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;

    using Moq;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class PlayerStatisticsSpecs
    {
        protected static PlayerStatisticsSut _sut;

        protected static IEventAggregator _eventAggregator;

        protected static Mock<IRepository> _repository_Stub;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _repository_Stub = new Mock<IRepository>();

            _sut = new PlayerStatisticsSut(_eventAggregator, _repository_Stub.Object);
            _sut
                .InitializePlayer("someName", "someSite")
                .UpdateStatistics();
        };

        [Subject(typeof(PlayerStatistics), "UpdateStatisticsWith")]
        public class when_updating_statistics_with_2_analyzable_players : PlayerStatisticsSpecs
        {
            const int firstHandId = 1;
            const int secondHandId = 2;
            
            static Mock<IAnalyzablePokerPlayer> firstPlayer;
            static Mock<IAnalyzablePokerPlayer> secondPlayer;

            static ActionSequences[] actionSequences_Stub;

            Establish context = () => {
                actionSequences_Stub = new ActionSequences[(int) Streets.River + 1];

                firstPlayer = new Mock<IAnalyzablePokerPlayer>();
                firstPlayer
                    .SetupGet(ap => ap.ActionSequences)
                    .Returns(actionSequences_Stub);
                firstPlayer
                    .SetupGet(ap => ap.HandId)
                    .Returns(firstHandId);
                
                secondPlayer = new Mock<IAnalyzablePokerPlayer>();
                secondPlayer
                    .SetupGet(ap => ap.ActionSequences)
                    .Returns(actionSequences_Stub);
                secondPlayer
                    .SetupGet(ap => ap.HandId)
                    .Returns(secondHandId);
            };

            Because of = () => _sut.UpdateStatisticsWith_Invoke(new[] { firstPlayer.Object, secondPlayer.Object });

            It should_add_the_hand_Id_of_the_first_player_to_AllHandIds = () => _sut.AllHandIds.ShouldContain(firstHandId);

            It should_add_the_hand_Id_of_the_second_player_to_AllHandIds = () => _sut.AllHandIds.ShouldContain(secondHandId);
        }
    }
}