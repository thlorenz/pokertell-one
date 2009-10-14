// Date: 5/2/2009
namespace PokerTell.PokerHand.Aquisition
{
    using System;

    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Description of PokeRound.
    /// </summary>
    public class AquiredPokerRound : PokerRound, IAquiredPokerRound
    {
        #region Properties

        public double ChipsGained
        {
            get { return CalculateChipsGain(); }
        }

        #endregion

        #region Indexers

        public IAquiredPokerAction this[int index]
        {
            get { return (IAquiredPokerAction)GetPokerActionAtIndex(index); }
        }

        #endregion

        #region Public Methods

        public IAquiredPokerRound AddAction(IAquiredPokerAction theAction)
        {
            if (theAction != null)
            {
                _actions.Add(theAction);
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
    }
}