namespace PokerTell.Statistics.Tests
{
    using System.Linq;

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
        const string Name = "somePlayer";

        const string Site = "someSite";

        protected static PlayerStatisticsSut _sut;

        protected static IEventAggregator _eventAggregator;

        protected static Mock<IRepository> _repository_Stub;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _repository_Stub = new Mock<IRepository>();

            _sut = new PlayerStatisticsSut(_eventAggregator, _repository_Stub.Object);
            _sut.InitializePlayer(Name, Site);
        };

        [Subject(typeof(PlayerStatistics), "UpdateStatisticsWith")]
        public class when_statistics_are_being_updated_with_new_data_or_because_a_filter_is_applied : PlayerStatisticsSpecs
        {
            static bool statisticsWereUpdatedWasRaised;

            Establish context = () => _sut.StatisticsWereUpdated += () => statisticsWereUpdatedWasRaised = true;

            Because of = () => _sut.UpdateStatisticsWith_Invoke(Enumerable.Empty<IAnalyzablePokerPlayer>());

            It should_let_me_know_that_it_was_updated = () => statisticsWereUpdatedWasRaised.ShouldBeTrue();
        }
    }
}