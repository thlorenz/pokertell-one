/*
 * User: Thorsten Lorenz
 * Date: 6/23/2009
 * 
 */
namespace PokerTell.LiveTracker.Overlay
{
    using Interfaces;

    /// <summary>
    /// Description of OverlayMapper.
    /// </summary>
    public class SeatMapper : ISeatMapper
    {
        private int _preferredSeat;
        private int _totalSeats;

        int _actualSeatOfHero;
      
        public ISeatMapper InitializeWith(int totalSeats)
        {
            _totalSeats = totalSeats;
            return this;
        }

        public ISeatMapper UpdateWith(int actualSeatOfHero)
        {
            _actualSeatOfHero = actualSeatOfHero;
            return this;
        }

        public int Map(int seatToBeMapped, int preferredSeat)
        {
            _preferredSeat = preferredSeat;
            int offset = GetOffsetFor(_actualSeatOfHero);
            return MapUsingOffset(seatToBeMapped, offset);
        }

        /// <summary>
        /// Determines the seat offset, by substracting the Absolute Seat from the Preferred Seat
        /// </summary>
        /// <para>For instance if the Hero sits in seat 5 but prefers
        /// seat 4, the offset will be 4 - 5 = -1</para>
        /// <param name="actualSeat">Absolute Seat of Hero</param>
        /// <returns>The offset</returns>
        private int GetOffsetFor(int actualSeat)
        {
            // Return 0 if user has no preferred seat or his seat wasn't found
            if ((_preferredSeat > 0) && (actualSeat > 0)) {
                return actualSeat - _preferredSeat;
            }

            return 0;
        }

        /// <summary>
        /// Maps a seat to another one using the previously calculated Offset
        /// </summary>
        /// <param name="actualSeat">Seat number from Hand History</param>
        /// <param name="offset">Previously calculated Offset</param>
        /// <returns>Physical Seatnumber at the table that the data should be mapped to</returns>
        private int MapUsingOffset(int actualSeat, int offset)
        {
            int x = actualSeat - offset;

            if (x < 1) {
                x = _totalSeats + x;
            }
            else if (x > _totalSeats) {
                x = x - _totalSeats;
            }

            return x;
        }
    }
}