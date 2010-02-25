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

        public class Ctx_Intialized_with_Settings : OverlayBoardViewModelSpecs
        {
            protected static Mock<ITableOverlaySettingsViewModel> _overlaySettings_Mock;

            protected const double left = 1.0;

            protected const double top = 2.0;

            Establish context = () => {
                _overlaySettings_Mock = new Mock<ITableOverlaySettingsViewModel>();
                _overlaySettings_Mock.SetupGet(os => os.BoardPosition).Returns(new Point(left, top));
                    _sut.InitializeWith(_overlaySettings_Mock.Object);
                };
        }

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

        [Subject(typeof(OverlayBoardViewModel), "Settings Position Synchronization")]
        public class when_initialized_with_settings : Ctx_Intialized_with_Settings
        {
            It Left_returns_left_returned_by_settings_for_the_BoardPosition = () => _sut.Left.ShouldEqual(left);

            It Top_returns_top_returned_by_settings_for_the_BoardPosition = () => _sut.Top.ShouldEqual(top);
        }

        [Subject(typeof(OverlayBoardViewModel), "Settings Position Synchronization")]
        public class when_initialized_with_settings_setting_Left : Ctx_Intialized_with_Settings
        {
            const double newLeft = 1.1;

            Because of = () => _sut.Left = newLeft;

            It should_set_X_on_the_settings_for_the_BoardPosition
                = () => _overlaySettings_Mock.VerifySet(os => os.BoardPosition = new Point(newLeft, top));
        }

        [Subject(typeof(OverlayBoardViewModel), "Settings Position Synchronization")]
        public class when_initialized_with_settings_setting_Top : Ctx_Intialized_with_Settings
        {
            const double newTop = 1.1;

            Because of = () => _sut.Top = newTop;

            It should_set_Y_on_the_settings_for_the_BoardPosition
                = () => _overlaySettings_Mock.VerifySet(os => os.BoardPosition = new Point(left, newTop));
        }
        
    }
}