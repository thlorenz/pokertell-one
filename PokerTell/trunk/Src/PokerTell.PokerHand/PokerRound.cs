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
    /// Defines one Round of Betting in a Poker Hand.
    /// </summary>
    public abstract class PokerRound : IPokerRound
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public PokerRound()
        {
            _actions = new List<IPokerAction>();
        }

        #endregion

        // Will contain all actions in this Betting Round
        #region Properties

        public ReadOnlyCollection<IPokerAction> Actions
        {
            get { return _actions.AsReadOnly(); }
        }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        public int Count
        {
            get
            {
                if (_actions != null)
                {
                    return _actions.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        protected List<IPokerAction> _actions { get; set; }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            int theHashCode = 0;
            foreach (PokerAction theAction in this)
            {
                theHashCode ^= theAction.GetHashCode();
            }

            return theHashCode;
        }

        public IPokerAction GetPokerActionAtIndex(int index)
        {
            if (Actions[index] != null)
            {
                return Actions[index];
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "PokerAction at index " + index + "is null" + "Count is " + _actions.Count);
            }
        }

        /// <summary>
        /// Removes an action at a certain index
        /// Currently only used by PokerHandConverter to remove posting actions
        /// </summary>
        /// <param name="Index">Index at which action will be removed</param>
        public void RemoveAction(int Index)
        {
            try
            {
                _actions.RemoveAt(Index);
            }
            catch (IndexOutOfRangeException excep)
            {
                excep.Data.Add("Index: " + Index + " Count: ", Count);
                Log.Error("Didn't remove the action", excep);
            }
        }

        /// <summary>
        /// Removes a given action
        /// </summary>
        /// <param name="theAction">Action to remove</param>
        /// <returns>true if it was removed</returns>
        public bool RemoveAction(IPokerAction theAction)
        {
            return _actions.Remove(theAction);
        }

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        public override string ToString()
        {
            string act_str = string.Empty;

            foreach (IPokerAction iA in this)
            {
                act_str = act_str + iA.ToString() + " ";
            }

            return act_str;
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable

        /// <summary>
        /// Inumerate the Round
        /// </summary>
        /// <returns>Enumerator of actions in round</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _actions.GetEnumerator();
        }

        #endregion

        #endregion
    }
}