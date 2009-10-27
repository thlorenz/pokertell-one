// Date: 5/2/2009
namespace PokerTell.PokerHand.Aquisition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    /// <summary>
    /// Description of PokeRound.
    /// </summary>
    public class AquiredPokerRound : IAquiredPokerRound
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AquiredPokerRound()
        {
            Actions = new List<IAquiredPokerAction>();
        }
        
        #region Properties

        public double ChipsGained
        {
            get { return CalculateChipsGain(); }
        }

        public IList<IAquiredPokerAction> Actions { get; set; }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        public int Count
        {
            get { return Actions != null ? Actions.Count : 0; }
        }

        #endregion

        #region Public Methods

        public IAquiredPokerRound AddAction(IAquiredPokerAction theAction)
        {
            if (theAction != null)
            {
                Actions.Add(theAction);
            }
            else
            {
                throw new ArgumentNullException("theAction");
            }

            return this;
        }

        #endregion

        #region Methods

        private double CalculateChipsGain()
        {
            double currentChips = 0;
            foreach (IAquiredPokerAction action in Actions)
            {
                currentChips += action.ChipsGained;
            }

            return currentChips;
        }

        #endregion

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="action">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public IAquiredPokerRound Add(IAquiredPokerAction action)
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

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IAquiredPokerAction this[int index]
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
            foreach (IAquiredPokerAction action in Actions)
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
        public bool RemoveAction(IAquiredPokerAction theAction)
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
    }
}