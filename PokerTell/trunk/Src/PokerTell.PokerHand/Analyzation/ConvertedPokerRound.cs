namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// PokerRound used for analysing a Poker Hand
    /// </summary>
    [Serializable]
    [XmlInclude(typeof(ConvertedPokerAction))]
    public class ConvertedPokerRound : IConvertedPokerRound
    {
        #region Constants and Fields

        readonly PokerRound _pokerRound;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerRound()
        {
            _pokerRound = new PokerRound();
            Actions = new List<IConvertedPokerAction>();
        }

        #endregion

        #region Properties

        public IList<IConvertedPokerAction> Actions { get; set; }

        /// <summary>
        /// Number of actions in this round
        /// </summary>
        [XmlIgnore]
        public int Count
        {
            get { return Actions.Count; }
        }

        #endregion

        #region Indexers

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

        #endregion

        #region Public Methods

        public void Add(object obj)
        {
            if (obj != null)
            {
                Add((IConvertedPokerAction)obj);
            }
        }

        public override bool Equals(object obj)
        {
            return _pokerRound.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _pokerRound.GetHashCode();
        }

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerRound

        /// <summary>
        /// The add action.
        /// </summary>
        /// <param name="theAction">
        /// The the action.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public IConvertedPokerRound Add(IConvertedPokerAction theAction)
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

        public override string ToString()
        {
            return _pokerRound.ToString();
        }

        #endregion

        #region IEnumerable

        public IEnumerator GetEnumerator()
        {
            return Actions.GetEnumerator();
        }

        #endregion

        #endregion
    }
}