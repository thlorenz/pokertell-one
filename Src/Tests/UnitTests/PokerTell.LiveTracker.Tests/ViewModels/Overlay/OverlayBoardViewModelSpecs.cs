namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Windows;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using LiveTracker.ViewModels.Overlay;

    using Machine.Specifications;

    using Moq;

    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class OverlayBoardViewModelSpecs
    { 
        protected static IOverlayBoardViewModel _sut;

        protected static Mock<IBoardViewModel> _boardViewModel_Mock;

        protected static Mock<IDispatcherTimer> _timer_Stub;

        Establish specContext = () => {
            _boardViewModel_Mock = new Mock<IBoardViewModel>();
            _timer_Stub = new Mock<IDispatcherTimer>();
            _sut = new OverlayBoardViewModel(_boardViewModel_Mock.Object, _timer_Stub.Object);
        };

        [Subject(typeof(OverlayBoardViewModel), "HideBoardAfter")]
        public class when_told_to_hide_board_after_2_seconds : OverlayBoardViewModelSpecs
        {
            Because of = () => _sut.HideBoardAfter(2);

            It shoud_set_timer_interval_to_2s = () => _timer_Stub.VerifySet(t => t.Interval = TimeSpan.FromSeconds(2));

            It should_start_the_timer = () => _timer_Stub.Verify(t => t.Start());
        }

        [Subject(typeof(OverlayBoardViewModel), "HideBoardAfter")]
        public class when_the_timer_ticks : OverlayBoardViewModelSpecs
        {
            Because of = () => _timer_Stub.Raise(t => t.Tick += null, null, null);

            It should_make_the_boardviewodel_invisible = () => _boardViewModel_Mock.VerifySet(b => b.Visible = false);

            It should_stop_the_timer = () => _timer_Stub.Verify(t => t.Stop());
        }

        [Subject(typeof(OverlayBoardViewModel), "IDisposable")]
        public class when_it_is_disposed : OverlayBoardViewModelSpecs
        {
            Because of = () => _sut.Dispose();

            It should_stop_the_timer_to_prevent_memory_leaks = () => _timer_Stub.Verify(t => t.Stop());
        }
    }
}