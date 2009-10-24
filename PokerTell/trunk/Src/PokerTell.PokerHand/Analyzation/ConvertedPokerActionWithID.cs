// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="ConvertedPokerActionWithId.cs">
// </copyright>
// <summary>
//   Extends the functionality of PokerAction
//   It includes and identifier (e.g. PlayerId) as a property
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PokerTell.PokerHand.Analyzation
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Extends the functionality of PokerAction 
    /// It includes and identifier (e.g. PlayerId) as a property
    /// </summary>
    public class ConvertedPokerActionWithId : IEquatable<IConvertedPokerActionWithId>, IConvertedPokerActionWithId
    {
        IConvertedPokerAction _convertedAction;

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerActionWithId"/> class.
        /// </summary>
        public ConvertedPokerActionWithId()
            : this(new ConvertedPokerAction(), 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerActionWithId"/> class.
        /// </summary>
        /// <param name="convertedAction">
        /// The converted action.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        public ConvertedPokerActionWithId(IConvertedPokerAction convertedAction, int id)
        {
            InitializeWith(convertedAction, id);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        public int Id { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as ConvertedPokerActionWithId);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public override int GetHashCode()
        {
           return _convertedAction.GetHashCode() ^ Id;
        }

        public IConvertedPokerActionWithId InitializeWith(IConvertedPokerAction convertedAction, int id)
        {
            _convertedAction = convertedAction;
            
            Id = id;
           
            return this;
        }

        /// <summary>
        /// The amount connected to the action in relation to the pot
        /// for calling and betting or in relation to the amount to call for raising
        /// </summary>
        public double Ratio
        {
            get { return _convertedAction.Ratio; }
        }

        /// <summary>The kind of action (call, fold etc.)</summary>
        public ActionTypes What
        {
            get { return _convertedAction.What; }
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The to string.
        /// </returns>
        public override string ToString()
        {
            return string.Format("[{0}]{1}", Id, _convertedAction.ToString());
        }

        public IConvertedPokerAction InitializeWith(ActionTypes what, double ratio)
        {
            return _convertedAction.InitializeWith(what, ratio);
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<ConvertedPokerActionWithId>

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public bool Equals(IConvertedPokerActionWithId other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetHashCode().Equals(other.GetHashCode());
        }

        #endregion


        #endregion
    }
}