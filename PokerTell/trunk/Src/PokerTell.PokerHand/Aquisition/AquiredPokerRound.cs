// Date: 5/2/2009
namespace PokerTell.PokerHand.Aquisition
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Description of PokeRound.
    /// </summary>
    public class AquiredPokerRound : PokerRound<IAquiredPokerAction>, IAquiredPokerRound
    {
        #region Properties

        public double ChipsGained
        {
            get { return CalculateChipsGain(); }
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
    }
}