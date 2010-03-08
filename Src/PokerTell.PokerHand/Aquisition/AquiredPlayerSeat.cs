//Date: 5/5/2009

namespace PokerTell.PokerHand.Aquisition
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Description of AquiredPlayerSeat.
    /// </summary>
    public struct AquiredPlayerSeat : IEquatable<AquiredPlayerSeat>, IAquiredPlayerSeat
    {
        #region Constants and Fields

        /// <summary>Name of the Player in the seat</summary>
        public string PlayerName { get;  private set; }

        /// <summary>Number of the seat</summary>
        public int SeatNumber { get; private set; }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AquiredPlayerSeat"/> struct. 
        /// Constructs Seat with given info
        /// </summary>
        /// <param name="playerName">
        /// Name of Player
        /// </param>
        /// <param name="seatNumber">
        /// Number of Seat
        /// </param>
        public AquiredPlayerSeat(string playerName, int seatNumber)
            : this()
        {
            InitializeWith(playerName, seatNumber);
        }

        public void InitializeWith(string playerName, int seatNumber)
        {
            PlayerName = playerName;
            SeatNumber = seatNumber;
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IAquiredPlayerSeat)
            {
                return Equals((AquiredPlayerSeat)obj); // use Equals method below
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return PlayerName.GetHashCode() ^ SeatNumber.GetHashCode();
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<AquiredPlayerSeat>

        public bool Equals(AquiredPlayerSeat other)
        {
            return GetHashCode().Equals(other.GetHashCode());
        }

        #endregion

        #endregion
    }
}