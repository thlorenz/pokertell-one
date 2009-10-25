namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// PokerRound used for analysing a Poker Hand
    /// </summary>
    [XmlInclude(typeof(ConvertedPokerAction))]
    public class ConvertedPokerRound : PokerRound<IConvertedPokerAction>, IConvertedPokerRound
    {
        #region Constructors and Destructors

        public ConvertedPokerRound()
        {
            Actions = new List<IConvertedPokerAction>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Necessary for XmlSerialization
        /// </summary>
        /// <param name="obj"></param>
        public void Add(object obj)
        {
            if (obj != null)
            {
                var action = (IConvertedPokerAction)obj;

                Add(action);
            }
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
    }
}