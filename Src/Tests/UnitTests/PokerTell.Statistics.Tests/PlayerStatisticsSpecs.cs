namespace PokerTell.Statistics.Tests
{
    using System.Linq;

    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Repository;
    using Infrastructure.Interfaces.Statistics;

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

        [Subject(typeof(PlayerStatistics), "Changing Filter")]
        public class when_a_new_filter_is_applied : PlayerStatisticsSpecs
        {
            static bool filterChangedWasRaised;

            Establish context = () => _sut.FilterChanged += () => filterChangedWasRaised = true;

            Because of = () => _sut.Filter = new Mock<IAnalyzablePokerPlayersFilter>().Object;

            It should_let_me_know_that_it_was_changed = () => filterChangedWasRaised.ShouldBeTrue();
        }
    }
}