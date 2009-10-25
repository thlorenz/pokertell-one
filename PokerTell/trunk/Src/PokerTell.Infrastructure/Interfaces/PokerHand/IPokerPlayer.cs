namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;

    public interface IPokerPlayer
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
    }
}