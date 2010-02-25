namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using LiveTracker.ViewModels.Overlay;

    using Machine.Specifications;

    using Moq;

    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class OverlayHoleCardsViewModelSpecs
    {
        protected static IOverlayHoleCardsViewModel _sut;
        
        protected static Mock<IHoleCardsViewModel> _holeCardsVM_Mock;

        protected static Mock<IDispatcherTimer> _timer_Stub;

        Establish specContext = () => {
            _holeCardsVM_Mock = new Mock<IHoleCardsViewModel>();
            _timer_Stub = new Mock<IDispatcherTimer>();
            _sut = new OverlayHoleCardsViewModel(_holeCardsVM_Mock.Object, _timer_Stub.Object);
        };

        public class Ctx_Intialized_with_Positions_And_Seat_1 : OverlayHoleCardsViewModelSpecs
        {
            protected static IList<Point> _holeCardPositions;

            protected const double left = 1.0;

            protected const double top = 2.0;

            protected const int seat = 1;

            Establish context = () => {
                _holeCardPositions = new[] { new Point(), new Point(left, top) }; 
                    _sut.InitializeWith(_holeCardPositions, seat);
                };
        }

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_told_to_hide_holecards_after_2_seconds : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _sut.HideHoleCardsAfter(2);

            It shoud_set_timer_interval_to_2s = () => _timer_Stub.VerifySet(t => t.Interval = TimeSpan.FromSeconds(2));

            It should_start_the_timer = () => _timer_Stub.Verify(t => t.Start());
        }

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_the_timer_ticks : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _timer_Stub.Raise(t => t.Tick += null, null, null);

            It should_make_the_holecards_invisible = () => _holeCardsVM_Mock.VerifySet(b => b.Visible = false);

            It should_stop_the_timer = () => _timer_Stub.Verify(t => t.Stop());
        }

        [Subject(typeof(OverlayHoleCardsViewModel), "HideHoleCardsAfter")]
        public class when_it_is_disposed : OverlayHoleCardsViewModelSpecs
        {
            Because of = () => _sut.Dispose();

            It should_stop_the_timer_to_prevent_memory_leaks = () => _timer_Stub.Verify(t => t.Stop());
        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1 : Ctx_Intialized_with_Positions_And_Seat_1
        {
            It Left_returns_left_of_seat_number_1 = () => _sut.Left.ShouldEqual(left);

            It Top_returns_top_of_seat_number_1 = () => _sut.Top.ShouldEqual(top);
        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1_settingLeft : Ctx_Intialized_with_Positions_And_Seat_1
        {
            const double newLeft = 1.1;

            Because of = () => _sut.Left = newLeft;

            It should_set_X_for_position_for_seat_1
                = () => _holeCardPositions[seat].X.ShouldEqual(newLeft);
        }

        [Subject(typeof(PlayerOverlayViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1_settingTop : Ctx_Intialized_with_Positions_And_Seat_1
        {
            const double newTop = 1.1;

            Because of = () => _sut.Top = newTop;

            It should_set_Y_for_position_for_seat_1
                = () => _holeCardPositions[seat].Y.ShouldEqual(newTop);
        }
    }
}