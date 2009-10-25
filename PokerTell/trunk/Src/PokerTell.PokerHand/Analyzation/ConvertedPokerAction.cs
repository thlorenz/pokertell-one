// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConvertedPokerAction.cs" company="">
// </copyright>
// <summary>
//   Contains Info about a Poker Action
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Xml.Serialization;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains Info about a Poker Action
    /// </summary>
    [Serializable]
    public class ConvertedPokerAction : IConvertedPokerAction
    {
        readonly PokerAction _pokerAction;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerAction"/> class.
        /// </summary>
        public ConvertedPokerAction()
        {
            _pokerAction = new PokerAction();
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
            : this()
        {
            InitializeWith(what, ratio);
        }

        #endregion

        public IConvertedPokerAction InitializeWith(ActionTypes what, double ratio)
        {
            _pokerAction.What = what;
            _pokerAction.Ratio = ratio;
            
            return this;
        }

        /// <summary>
        /// The amount connected to the action in relation to the pot
        /// for calling and betting or in relation to the amount to call for raising
        /// </summary>
        public double Ratio
        {
            get { return _pokerAction.Ratio; }
            set { _pokerAction.Ratio = value; }
        }

        /// <summary>The kind of action (call, fold etc.)</summary>
        public ActionTypes What
        {
            get { return _pokerAction.What; }
            set { _pokerAction.What = value; }
        }

        public override string ToString()
        {
            return _pokerAction.ToString();
        }

        public override bool Equals(object obj)
        {
            return _pokerAction.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _pokerAction.GetHashCode();
        }
    }
}