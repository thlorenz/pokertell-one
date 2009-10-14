// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertedPokerAction.cs" company="">
// </copyright>
// <summary>
//   Contains Info about a Poker Action
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PokerTell.PokerHand.Analyzation
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Info about a Poker Action
    /// </summary>
    public class ConvertedPokerAction : PokerAction, IConvertedPokerAction
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerAction"/> class.
        /// </summary>
        public ConvertedPokerAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerAction"/> class.
        /// </summary>
        /// <param name="what">
        /// The what.
        /// </param>
        /// <param name="ratio">
        /// The ratio.
        /// </param>
        public ConvertedPokerAction(ActionTypes what, double ratio)
        {
            InitializeWith(what, ratio);
        }

        #endregion

        public IConvertedPokerAction InitializeWith(ActionTypes what, double ratio)
        {
            What = what;
            Ratio = ratio;
            
            return this;
        }
    }
}