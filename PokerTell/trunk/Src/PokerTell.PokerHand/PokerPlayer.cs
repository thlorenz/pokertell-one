namespace PokerTell.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Information about a player
    /// </summary>
    public class PokerPlayer : IPokerPlayer, IEnumerable, IComparable<IPokerPlayer>
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
        public string Name { get; set; }

        /// <summary>
        /// Id of player in Database
        /// </summary>
        public long PlayerId { get; set; }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        public IList<IAquiredPokerRound> Rounds { get; set; }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IPokerPlayer))
            {
                return false;
            }

            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable<IPokerPlayer>

        public int CompareTo(IPokerPlayer other)
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

        #region IPokerPlayer

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
                foreach (IPokerRound iB in this)
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
        protected IPokerRound GetPokerRoundAtIndex(int index)
        {
            if (Rounds[index] != null)
            {
                return Rounds[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Round of index " + index + "doesn't exist" + " Max is " + Rounds.Count);
            }
        }

        #endregion
    }
}