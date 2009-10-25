namespace PokerTell.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Serialization;

    using Analyzation;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Information about a player
    /// </summary>
    [Serializable]
    public class PokerPlayer<TRound> : IPokerPlayer<TRound>, IEnumerable, IComparable<IPokerPlayer<TRound>>
        where TRound : class
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        string _holecards;

        #endregion

        #region Properties

        /// <summary>
        /// Absolute seat number of player as stated in the Hand History
        /// </summary>
        public int AbsSeatNum { get; set; }

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        public int Count
        {
            get { return Rounds.Count; }
        }

        /// <summary>
        /// Players Hole Cards - set to "??" when unknown
        /// </summary>
        public string Holecards
        {
            get { return _holecards; }

            set { _holecards = string.IsNullOrEmpty(value) ? "?? ??" : value; }
        }

        /// <summary>
        /// Nickname of the player
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Id of player in Database
        /// </summary>
        [XmlAttribute]
        public long PlayerId { get; set; }

        public PokerPlayer()
        {
            Position = 4;
        }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        public IList<TRound> Rounds { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public TRound this[int index]
        {
            get { return Rounds[index]; }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="theStreet">
        /// The the street.
        /// </param>
        public TRound this[Streets theStreet]
        {
            get { return this[(int)theStreet]; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Necessary for XmlSerialization
        /// </summary>
        /// <param name="obj"></param>
        public void Add(object obj)
        {
            if (obj != null)
            {
                var action = (TRound)obj;
                Rounds.Add(action);
            }
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

            if (obj.GetType() != typeof(PokerPlayer<TRound>))
            {
                return false;
            }

            return Equals((PokerPlayer<TRound>)obj);
        }

        public bool Equals(PokerPlayer<TRound> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other._holecards, _holecards) && other.AbsSeatNum == AbsSeatNum && Equals(other.Name, Name) &&
                   other.PlayerId == PlayerId && other.Position == Position && Equals(other.Rounds, Rounds);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _holecards != null ? _holecards.GetHashCode() : 0;
                result = (result * 397) ^ AbsSeatNum;
                result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ PlayerId.GetHashCode();
                result = (result * 397) ^ Position;
                result = (result * 397) ^ (Rounds != null ? Rounds.GetHashCode() : 0);
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable<IPokerPlayer<TRound>>

        public int CompareTo(IPokerPlayer<TRound> other)
        {
            if (Position < other.Position)
            {
                return -1;
            }

            if (Position > other.Position)
            {
                return 1;
            }

            return 0;
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rounds.GetEnumerator();
        }

        #endregion

        #region IPokerPlayer<TRound>

        /// <summary>
        /// Gives string representation of Players info and actions
        /// </summary>
        /// <returns>String representation of Players info and actions</returns>
        public override string ToString()
        {
            try
            {
                return string.Format("[{3} {0} {1}] \t{2}\n", Position, Name, BettingRoundsToString(), Holecards);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning empty string", excep);
                return string.Empty;
            }
        }

        #endregion

        #endregion

        #region Methods

        protected internal string BettingRoundsToString()
        {
            string betting = string.Empty;
            try
            {
                // Iterate through rounds pre-flop to river
                foreach (object iB in this)
                {
                    betting += "| " + iB;
                }
            }
            catch (ArgumentNullException excep)
            {
                excep.Data.Add("betRoundCount = ", Rounds.Count);
                Log.Error("Returning betting Rounds I go so far", excep);
            }

            return betting;
        }

        /// <summary>
        /// Retrieves the PokerRound at the index 
        /// If it finds the PokerRound to be null it returns a default PokerRound
        /// </summary>
        protected TRound GetPokerRoundAtIndex(int index)
        {
            if (Rounds[index] != null)
            {
                return Rounds[index];
            }
            else
            {
                throw new IndexOutOfRangeException(
                    "Round of index " + index + "doesn't exist" + " Max is " + Rounds.Count);
            }
        }

        #endregion
    }
}