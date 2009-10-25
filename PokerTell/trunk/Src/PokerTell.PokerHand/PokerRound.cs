namespace PokerTell.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Defines one Round of Betting in a Poker Hand.
    /// </summary>
    public abstract class PokerRound<TAction>
        where TAction : class
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public PokerRound()
        {
            Actions = new List<TAction>();
        }

        #endregion

        #region Properties

        public IList<TAction> Actions { get; set; }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        public int Count
        {
            get { return Actions != null ? Actions.Count : 0; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public TAction this[int index]
        {
            get { return Actions[index]; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public IEnumerator GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        public override int GetHashCode()
        {
            int theHashCode = 0;
            foreach (TAction action in Actions)
            {
                theHashCode ^= action.GetHashCode();
            }

            return theHashCode;
        }

        /// <summary>
        /// Removes an action at a certain index
        /// Currently only used by PokerHandConverter to remove posting actions
        /// </summary>
        /// <param name="index">Index at which action will be removed</param>
        public void RemoveAction(int index)
        {
            try
            {
                Actions.RemoveAt(index);
            }
            catch (IndexOutOfRangeException excep)
            {
                excep.Data.Add("index: " + index + " Count: ", Count);
                Log.Error("Didn't remove the action", excep);
            }
        }

        /// <summary>
        /// Removes a given action
        /// </summary>
        /// <param name="theAction">Action to remove</param>
        /// <returns>true if it was removed</returns>
        public bool RemoveAction(TAction theAction)
        {
            return Actions.Remove(theAction);
        }

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        public override string ToString()
        {
            string actStr = string.Empty;

            foreach (IPokerAction iA in Actions)
            {
                actStr = actStr + iA.ToString() + " ";
            }

            return actStr;
        }

        #endregion
    }
}