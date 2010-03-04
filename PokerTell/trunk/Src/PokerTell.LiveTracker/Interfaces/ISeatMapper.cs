namespace PokerTell.LiveTracker.Interfaces
{
    using Tools.Interfaces;

    public interface ISeatMapper : IFluentInterface
    {
        ISeatMapper InitializeWith(int totalSeats);

        ISeatMapper UpdateWith(int actualSeatOfHero);

        int Map(int seatToBeMapped, int preferredSeat);
    }
}