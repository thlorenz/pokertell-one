namespace PokerTell.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    /// <summary>
    /// Contains a collection of Poker Hands and provides methods to maintain it
    /// </summary>
    public class PokerHands : IPokerHands
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<IPokerHand> _hands;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerHands"/> class. 
        /// Initializes the Poker Hand Collection
        /// </summary>
        public PokerHands()
        {
            _hands = new List<IPokerHand>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerHands"/> class. 
        /// Initializes the Poker Hand Collection and adds hands to it
        /// </summary>
        /// <param name="hands">
        /// Collection of hands to add
        /// </param>
        public PokerHands(List<IPokerHand> hands)
        {
            InitializeWith(hands);
        }

        public IPokerHands InitializeWith(List<IPokerHand> hands)
        {
            _hands = hands;

            return this;
        }

        #endregion

        #region Properties

        public int Count
        {
            get { return Hands.Count; }
        }

        /// <summary>
        /// The collection of Poker Hands
        /// </summary>
        public ReadOnlyCollection<IPokerHand> Hands
        {
            get { return _hands.AsReadOnly(); }
        }

        /// <summary>
        /// Last Hand in this collection
        /// </summary>
        public IPokerHand LastHand
        {
            get
            {
                if (Hands.Count > 0)
                {
                    return Hands[Hands.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Indexers

        public IPokerHand this[int index]
        {
            get { return _hands[index]; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a hand, but only if it is not in the Hand collection yet
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>true if it was added, false if it was already in the Hand collection</returns>
        public bool AddHand(IPokerHand hand)
        {
            try
            {
                if (hand == null)
                {
                    throw new ArgumentNullException("hand");
                }

                // Make sure it doesn't exist yet
                if (! HandExists(hand))
                {
                    _hands.Add(hand);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Ignore Hand", excep);
            }

            return false;
        }

        /// <summary>
        /// Adds a collection of hands to this collection
        /// </summary>
        /// <param name="hands">Hands to be added</param>
        public void AddHands(IPokerHands hands)
        {
            if (hands == null)
            {
                Log.Error("Unhandled", new ArgumentNullException("hands"));
            }
            else
            {
                foreach (IPokerHand iH in hands)
                {
                    AddHand(iH);
                }
            }
        }

        public IPokerHand GetHand(ulong gameId)
        {
            foreach (IPokerHand iH in this)
            {
                if (gameId == iH.GameId)
                {
                    return iH;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a Hand is already contained in this list
        /// </summary>
        /// <param name="givenHand">Hand to check for</param>
        /// <returns>true if the hand does exist, false if it doesn't</returns>
        public bool HandExists(IPokerHand givenHand)
        {
            try
            {
                if (givenHand == null)
                {
                    throw new ArgumentNullException("GivenHand");
                }

                foreach (IPokerHand pokerHand in this)
                {
                    if (givenHand.Equals(pokerHand))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Unhandled", excep);
                return false;
            }
            catch (Exception excep)
            {
                if (givenHand != null)
                {
                    excep.Data.Add("Looking for Hand ", givenHand.GameId);
                }

                Log.Error("Unhandled", excep);
                return false;
            }
        }

        /// <summary>
        /// Checks if a Hand with a certain GamID is already contained in this list
        /// </summary>
        /// <param name="givenGameId">GameID of Hand to check for</param>
        /// <returns>true if the hand does exist, false if it doesn't</returns>
        public bool HandExists(ulong givenGameId)
        {
            try
            {
                foreach (IPokerHand iH in this)
                {
                    if (givenGameId == iH.GameId)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception excep)
            {
                excep.Data.Add("Looking for Hand ", givenGameId);
                Log.Error("Returning false", excep);
                return false;
            }
        }

        /// <summary>
        /// Removes a hand at the index
        /// </summary>
        /// <param name="index">Index at which hand will be removed</param>
        public void RemoveHand(int index)
        {
            try
            {
                _hands.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException excep)
            {
                excep.Data.Add("Can't remove at index: " + index + "with total count: ", Hands.Count);
                Log.Error("Unhandled", excep);
            }
        }

        /// <summary>
        /// Removes a Poker Hand from the list
        /// </summary>
        /// <param name="handToRemove">Hand to be removed</param>
        /// <returns>true if removed successfully, otherwise false</returns>
        public bool RemoveHand(IPokerHand handToRemove)
        {
            try
            {
                if (handToRemove == null)
                {
                    throw new ArgumentNullException("HandToRemove");
                }

                return _hands.Remove(handToRemove);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Unhandled", excep);
            }

            return false;
        }

        public void Sort()
        {
            _hands.Sort();
        }

        /// <summary>
        /// Gives a string presentation of all Hands in this collection
        /// </summary>
        /// <returns>String presentation of all Hands in this collection</returns>
        public override string ToString()
        {
            string handsInfo = string.Empty;

            foreach (IPokerHand hand in this)
            {
                handsInfo += hand + "\n \n";
            }

            return handsInfo;
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        /// <summary>
        /// Enumerator for Hand collection
        /// </summary>
        /// <returns>Enumerator og the Hand List</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hands.GetEnumerator();
        }

        #endregion

        #endregion
    }
}