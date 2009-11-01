namespace PokerTell.PokerHand
{
    using System;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Information about a player
    /// </summary>
    [Serializable]
    public class PokerPlayer : IPokerPlayer
    {
        #region Constants and Fields

        protected string _holecards;

        [NonSerialized]
        int _seatNumber;

        [NonSerialized]
        long _playerId;

        [NonSerialized]
        int _position;

        #endregion

        #region Properties

        /// <summary>
        /// Absolute seat number of player as stated in the Hand History
        /// </summary>
        public int SeatNumber
        {
            get { return _seatNumber; }
            set { _seatNumber = value; }
        }

        /// <summary>
        /// Players Hole Cards - set to "??" when unknown
        /// </summary>
        public string Holecards
        {
            get { return _holecards; }

            set { _holecards = string.IsNullOrEmpty(value) ? "?? ??" : value; }
        }

        string _name;

        /// <summary>
        /// Nickname of the player
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Id of player in Database
        /// </summary>
        public long PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var pokerPlayer = (IPokerPlayer)obj;

            return Equals(pokerPlayer);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _holecards != null ? _holecards.GetHashCode() : 0;
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IPokerPlayer

        public override string ToString()
        {
            return string.Format(
                "Holecards: {0}, SeatNumber: {1}, Name: {2}, PlayerId: {3}, Position: {4}", 
                _holecards, 
                SeatNumber, 
                Name, 
                PlayerId, 
                Position);
        }

        #endregion

        #endregion

        #region Methods

        bool Equals(IPokerPlayer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.GetHashCode().Equals(GetHashCode());
        }

        #endregion
    }
}