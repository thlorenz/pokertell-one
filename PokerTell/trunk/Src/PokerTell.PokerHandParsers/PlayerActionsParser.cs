namespace PokerTell.PokerHandParsers
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    public abstract class PlayerActionsParser
    {
        protected PlayerActionsParser(IConstructor<IAquiredPokerAction> aquiredPokerActionMake)
        {
            _aquiredPokerActionMake = aquiredPokerActionMake;
        }

        #region Constants and Fields

        public IDictionary<ActionTypes, string> ActionStrings;

        protected readonly IConstructor<IAquiredPokerAction> _aquiredPokerActionMake;

        #endregion

        #region Properties

        public IList<IAquiredPokerAction> PlayerActions { get; protected set; }

        #endregion

        #region Public Methods

        public abstract PlayerActionsParser Parse(string streetHistory, string playerName);

        #endregion

        #region Methods

        protected virtual void CreateActionStrings()
        {
            ActionStrings = new Dictionary<ActionTypes, string>
                {
                    { ActionTypes.B, "bets" },
                    { ActionTypes.C, "calls" },
                    { ActionTypes.F, "folds" },
                    { ActionTypes.P, "posts" },
                    { ActionTypes.R, "raises" },
                    { ActionTypes.X, "checks" },
                };
        }

        /// <summary>
        /// Converts a human readable action to an Action What type and is called by the Hand Parser
        /// </summary>
        /// <param name="actionString">Human readable action as found in Hand History</param>
        /// <returns>An ActionTypes type defining the action</returns>
        protected static ActionTypes ConvertActionString(string actionString)
        {
                actionString = actionString.ToLower();
                if (actionString.Contains("post"))
                {
                    return ActionTypes.P;
                }

                if (actionString.Contains("fold"))
                {
                    return ActionTypes.F;
                }

                if (actionString.Contains("check"))
                {
                    return ActionTypes.X;
                }

                if (actionString.Contains("call"))
                {
                    return ActionTypes.C;
                }

                if (actionString.Contains("bet"))
                {
                    return ActionTypes.B;
                }

                if (actionString.Contains("raise"))
                {
                    return ActionTypes.R;
                }

                if (actionString.Contains("collect"))
                {
                    return ActionTypes.W;
                }

                throw new FormatException("Invalid Action string " + actionString);
         }

        #endregion
    }
}