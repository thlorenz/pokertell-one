namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System.Collections.Generic;
    using System.Windows;

    using Machine.Specifications;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    // Resharper disable InconsistentNaming
    public abstract class HarringtonMViewModelSpecs
    {
        protected static IHarringtonMViewModel _sut;

        Establish specContext = () => { _sut = new HarringtonMViewModel(); };

        public class Ctx_Intialized_with_Positions_And_Seat_1 : HarringtonMViewModelSpecs
        {
            protected static IList<Point> _harringtonMPositions;

            protected const double left = 1.0;

            protected const double top = 2.0;

            protected const int seat = 1;

            Establish context = () => {
                _harringtonMPositions = new[] { new Point(), new Point(left, top) };
                _sut.InitializeWith(_harringtonMPositions, seat);
            };
        }

        [Subject(typeof(HarringtonMViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1 : Ctx_Intialized_with_Positions_And_Seat_1
        {
            It Left_returns_left_of_seat_number_1 = () => _sut.Left.ShouldEqual(left);

            It Top_returns_top_of_seat_number_1 = () => _sut.Top.ShouldEqual(top);
        }

        [Subject(typeof(HarringtonMViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1_settingLeft : Ctx_Intialized_with_Positions_And_Seat_1
        {
            const double newLeft = 1.1;

            Because of = () => _sut.Left = newLeft;

            It should_set_X_for_position_for_seat_1
                = () => _harringtonMPositions[seat].X.ShouldEqual(newLeft);
        }

        [Subject(typeof(HarringtonMViewModel), "Position")]
        public class when_initialized_with_positions_and_seat_number_1_settingTop : Ctx_Intialized_with_Positions_And_Seat_1
        {
            const double newTop = 1.1;

            Because of = () => _sut.Top = newTop;

            It should_set_Y_for_position_for_seat_1
                = () => _harringtonMPositions[seat].Y.ShouldEqual(newTop);
        }
    }
}