namespace PokerTell.LiveTracker.Tests.ViewModels
{
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using LiveTracker.ViewModels;

    using Machine.Specifications;

    using Moq;

    using Tools.Interfaces;

    // Resharper disable InconsistentNaming
    public class BoardViewModelSpecs
    {
        protected static IOverlayBoardViewModel _sut;

        protected static Mock<IBoardViewModel> _boardViewModel_Mock;

        protected static Mock<IDispatcherTimer> _timer_Stub; 

        Establish specContext = () => {
            _timer_Stub = new Mock<IDispatcherTimer>();
            _sut = new OverlayBoardViewModel(_boardViewModel_Mock.Object, _timer_Stub.Object);
        };


    }
}