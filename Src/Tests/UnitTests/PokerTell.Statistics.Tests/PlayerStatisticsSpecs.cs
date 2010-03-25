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
        };
    }
}