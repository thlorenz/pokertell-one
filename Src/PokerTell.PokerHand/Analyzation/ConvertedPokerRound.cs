namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// PokerRound used for analysing a Poker Hand
    /// </summary>
    [Serializable]
    public class ConvertedPokerRound : IConvertedPokerRound
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        #region Constructors and Destructors

        public ConvertedPokerRound()
        {
            Actions = new List<IConvertedPokerAction>();
        }

        IList<IConvertedPokerAction> _actions;

        public IList<IConvertedPokerAction> Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        public int Count
        {
            get { return Actions != null ? Actions.Count : 0; }
        }

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerRound

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="action">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public IConvertedPokerRound Add(IConvertedPokerAction action)
        {
            if (action != null)
            {
                Actions.Add(action);
            }
            else
            {
                throw new ArgumentNullException("action");
            }

            return this;
        }

        #endregion

        #endregion

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IConvertedPokerAction this[int index]
        {
            get { return Actions[index]; }
        }

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
            foreach (IConvertedPokerAction action in Actions)
            {
                theHashCode ^= action.GetHashCode();
            }

            return theHashCode;
        }

        /// <summary>
        /// Gives a string representation of the round
        /// </summary>
        /// <returns>String representation of the round, including info about all actions</returns>
        public override string ToString()
        {
            string allActions = string.Empty;

            foreach (IConvertedPokerAction action in Actions)
            {
                allActions = allActions + action.ToString() + " ";
            }

            return allActions;
        }
    }
}