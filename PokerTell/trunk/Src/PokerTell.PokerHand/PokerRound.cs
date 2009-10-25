namespace PokerTell.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml.Serialization;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Defines one Round of Betting in a Poker Hand.
    /// </summary>
    public class PokerRound : IPokerRound
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public PokerRound()
        {
            Actions = new List<IPokerAction>();
        }

        #endregion

        #region Properties
        public IList<IPokerAction> Actions { get; set; }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        [XmlIgnore]
        public int Count
        {
            get { return Actions != null ? Actions.Count : 0; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            int theHashCode = 0;
            foreach (PokerAction theAction in Actions)
            {
                theHashCode ^= theAction.GetHashCode();
            }

            return theHashCode;
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
                Actions.RemoveAt(Index);
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
            return Actions.Remove(theAction);
        }

        #endregion

        #region Implemented Interfaces

        #region IPokerRound

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

        /// <summary>
        /// To enable XmlSerialization
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IPokerRound Add(object obj)
        {
            if (obj != null && obj is IPokerAction)
            {
                Add(obj);
            }

            return this;
        }

        #endregion

        #endregion
    }
}