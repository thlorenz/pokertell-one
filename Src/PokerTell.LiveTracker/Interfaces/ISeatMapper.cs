namespace PokerTell.LiveTracker.Interfaces
{
    using Infrastructure.Interfaces;

    public interface ISeatMapper : IFluentInterface
    {
        ISeatMapper InitializeWith(int totalSeats);

        ISeatMapper UpdateWith(int actualSeatOfHero);

        int Map(int seatToBeMapped, int preferredSeat);
    }
}