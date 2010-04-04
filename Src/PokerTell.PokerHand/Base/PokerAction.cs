namespace PokerTell.PokerHand.Base
{
    using System;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// General PokerAction
    /// </summary>
    [Serializable]
    public abstract class PokerAction : IPokerAction
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerAction"/> class. 
        /// Initializes an empty action
        /// This constructor is only usefull to create a dummy action in order to
        /// allow a method to access it's methods
        /// </summary>
        protected PokerAction()
        {
            What = ActionTypes.N;
            Ratio = 0;
        }

        double _ratio;

        /// <summary>
        /// The amount connected to the action in relation to the pot
        /// for calling and betting or in relation to the amount to call for raising
        /// </summary>
        public double Ratio
        {
            get { return _ratio; }
            set { _ratio = value; }
        }

        ActionTypes _what;

        /// <summary>The kind of action (call, fold etc.)</summary>
        public ActionTypes What
        {
            get { return _what; }
            set { _what = value; }
        }

        /// <summary>
        /// Converts a database entry about an action into an ActionTypes type
        /// </summary>
        /// <param name="theActionString">Action as found in the database</param>
        /// <returns>An Action What type defining the action</returns>
        public static ActionTypes GetActionWhatFromSql(string theActionString)
        {
            theActionString = theActionString.ToLower();
            try
            {
                if (theActionString.Contains("f"))
                {
                    return ActionTypes.F;
                }

                // fold
                if (theActionString.Contains("x"))
                {
                    return ActionTypes.X;
                }

                // check
                if (theActionString.Contains("c"))
                {
                    return ActionTypes.C;
                }

                // call
                if (theActionString.Contains("b"))
                {
                    return ActionTypes.B;
                }

                // bet
                if (theActionString.Contains("r"))
                {
                    return ActionTypes.R;
                }

                // raise
                if (theActionString.Contains("w"))
                {
                    return ActionTypes.W;
                }

                // win
                if (theActionString.Contains("a"))
                {
                    return ActionTypes.A;
                }

                // allin
                if (theActionString.Contains("p"))
                {
                    return ActionTypes.P;
                }

                throw new FormatException("Invalid Action string " + theActionString);
            }
            catch (FormatException excep)
            {
                Log.Error("Unhandled", excep);
            }

            return ActionTypes.E;
        }

        public override bool Equals(object obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return What.GetHashCode() ^ Ratio.GetHashCode();
        }

        /// <summary>
        /// Gives a string representation of the action
        /// </summary>
        /// <returns>String representation of the action type and ratio</returns>
        public override string ToString()
        {
            switch (What)
            {
                case ActionTypes.P:
                case ActionTypes.B:
                case ActionTypes.C:
                case ActionTypes.W:
                case ActionTypes.R:
                    return string.Format("{0} {1:F1} ", What, Ratio);

                case ActionTypes.A:
                case ActionTypes.E:
                    return string.Format("{0} ", What);

                case ActionTypes.X:
                case ActionTypes.F:
                    return string.Format("{0}       ", What);
                
                default:
                    return string.Format("{0} {1:F1} ", What, Ratio);
            }
        }
    }
}