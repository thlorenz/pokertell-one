namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;

    using Enumerations.PokerHand;

    public interface IPokerPlayer<TRound>
    {
        /// <summary>
        /// Absolute seat number of player as stated in the Hand History
        /// </summary>
        int AbsSeatNum { get; set; }

        /// <summary>
        /// Players Hole Cards - set to "??" when unknown
        /// </summary>
        string Holecards { get; set; }

        /// <summary>
        /// Nickname of the player
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Id of player in Database
        /// </summary>
        long PlayerId { get; set; }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs (-1 when yet unknown)
        /// </summary>
        int Position { get; set; }

        /// <summary>
        /// Gives string representation of Players info and actions
        /// </summary>
        /// <returns>String representation of Players info and actions</returns>
        string ToString();

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        int Count { get; }

        #region Indexers

        TRound this[Streets theStreet]
        {
            get;
        }

        TRound this[int index]
        {
            get;
        }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        IList<TRound> Rounds { get; }

        #endregion
    }
}