// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionTypes.cs" company="">
// </copyright>
// <summary>
//   Enumeration of all possible actions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PokerTell.Infrastructure.Enumerations.PokerHand
{
    using System.Collections.Generic;

    /// <summary>
    /// Enumeration of all possible actions
    /// </summary>
    public enum ActionTypes
    {
        /// <summary>
        /// Post ante or blinds
        /// </summary>
        P, 

        /// <summary>
        /// Fold
        /// </summary>
        F, 

        /// <summary>
        /// Check
        /// </summary>
        X, 

        /// <summary>
        /// Call
        /// </summary>
        C, 

        /// <summary>
        /// Bet
        /// </summary>
        B, 

        /// <summary>
        /// Raise
        /// </summary>
        R, 

        /// <summary>
        /// All In
        /// </summary>
        A, 

        /// <summary>
        /// Uncalled bet - returned
        /// </summary>
        U, 

        /// <summary>
        /// Collect (Win Pot)
        /// </summary>
        W, 

        /// <summary>
        /// Error
        /// </summary>
        E, 

        /// <summary>
        /// Action didn't exist
        /// </summary>
        N
    }

    /// <summary>
    /// The action what utility.
    /// </summary>
    public static class ActionTypesUtility
    {
        #region Constants and Fields

        /// <summary>
        /// The names.
        /// </summary>
        private static readonly string[] Names = new[]
            {
                "Post", "Fold", "Check", "Call", "Bet", "Raise", "All-In", "Uncalled Bet", "Win", "Error", "Not Found" 
            };

        #endregion

        #region Properties

        /// <summary>
        /// Gets Reactions.
        /// </summary>
        public static IEnumerable<ActionTypes> Reactions
        {
            get { return new[] { ActionTypes.F, ActionTypes.C, ActionTypes.R }; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The name.
        /// </summary>
        /// <param name="actionType">
        /// The action what.
        /// </param>
        /// <returns>
        /// The name.
        /// </returns>
        public static string Name(ActionTypes actionType)
        {
            return Names[(int)actionType];
        }

        #endregion
    }
}