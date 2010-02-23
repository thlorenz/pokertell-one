namespace PokerTell.LiveTracker.Interfaces
{
    public interface ISeatMapper
    {
        ISeatMapper InitializeWith(int totalSeats);

        ISeatMapper UpdateWith(int actualSeatOfHero);

        int Map(int seatToBeMapped, int preferredSeat);
    }
}