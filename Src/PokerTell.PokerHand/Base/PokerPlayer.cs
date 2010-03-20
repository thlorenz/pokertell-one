namespace PokerTell.PokerHand.Base
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Information about a player
    /// </summary>
    [Serializable]
    public class PokerPlayer : IPokerPlayer
    {
        const string UnknownHolecards = "";

        protected string _holecards = UnknownHolecards;

        [NonSerialized]
        int _seatNumber;

        [NonSerialized]
        int _position;

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

            set { _holecards = string.IsNullOrEmpty(value) ? UnknownHolecards : value; }
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
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((PokerPlayer)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _holecards != null ? _holecards.GetHashCode() : 0;
                result = (result * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format(
                "Holecards: {0}, SeatNumber: {1}, Name: {2}, Position: {3}", 
                _holecards, 
                SeatNumber, 
                Name, 
                Position);
        }

        bool Equals(PokerPlayer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other._holecards, _holecards) && Equals(other._name, _name);
        }
    }
}