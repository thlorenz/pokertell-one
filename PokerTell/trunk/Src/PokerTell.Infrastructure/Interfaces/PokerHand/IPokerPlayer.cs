namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;

    public interface IPokerPlayer : IEnumerable, IComparable<IPokerPlayer>
    {
        /// <summary>
        /// Absolute seat number of player as stated in the Hand History
        /// </summary>
        int AbsSeatNum { get; set; }

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        int Count { get; }

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
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        ReadOnlyCollection<IPokerRound> Rounds { get; }

        /// <summary>
        /// Gives string representation of Players info and actions
        /// </summary>
        /// <returns>String representation of Players info and actions</returns>
        string ToString();
    }
}