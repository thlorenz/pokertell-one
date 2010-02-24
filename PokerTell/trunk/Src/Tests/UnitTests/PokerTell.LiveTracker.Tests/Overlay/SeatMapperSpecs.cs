namespace PokerTell.LiveTracker.Tests.Overlay
{
    using Interfaces;

    using LiveTracker.Overlay;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming
    public abstract class SeatMapperSpecs
    {
        protected static ISeatMapper _sut;

        Establish specContext = () => { _sut = new SeatMapper().InitializeWith(6); };

        [Subject(typeof(SeatMapper), "Map")]
        public class when_initialized_with_6_total_Seats_and_Heros_actual_Seat_is_updated_with_1 : SeatMapperSpecs
        {
            Because of = () => _sut.UpdateWith(1);

            It mapping_seat_1_with_preferred_Seat_1_returns_1 = () => _sut.Map(1, 1).ShouldEqual(1);

            It mapping_seat_2_with_preferred_Seat_1_returns_2 = () => _sut.Map(2, 1).ShouldEqual(2);

            It mapping_seat_6_with_preferred_Seat_1_returns_6 = () => _sut.Map(6, 1).ShouldEqual(6);

            It mapping_seat_1_with_preferred_Seat_2_returns_2 = () => _sut.Map(1, 2).ShouldEqual(2);

            It mapping_seat_2_with_preferred_Seat_2_returns_3 = () => _sut.Map(2, 2).ShouldEqual(3);

            It mapping_seat_6_with_preferred_Seat_2_returns_1 = () => _sut.Map(6, 2).ShouldEqual(1);

            It mapping_seat_1_with_preferred_Seat_4_returns_4 = () => _sut.Map(1, 4).ShouldEqual(4);

            It mapping_seat_2_with_preferred_Seat_4_returns_5 = () => _sut.Map(2, 4).ShouldEqual(5);

            It mapping_seat_6_with_preferred_Seat_4_returns_3 = () => _sut.Map(6, 4).ShouldEqual(3);
        }
    }
}