namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IAquiredPokerHand : IPokerHand, IEnumerable
    {
        ReadOnlyCollection<IAquiredPokerPlayer> Players { get; }

        /// <summary>
        /// Pot at the end of the hand
        /// </summary>
        double TotalPot { get; set; }

        IAquiredPokerPlayer this[int index]
        {
            get;
        }

        /// <summary>
        /// List of the seat numbers and appropriate Player Names
        /// </summary>
        IList<IAquiredPlayerSeat> Seats { get; }

        /// <summary>
        /// Method to add a player to the hand
        /// This version is used by the Parser and when importing a hand from Poker Office
        /// </summary>
        /// <param name="aquiredPlayer">Poker Player</param>
        IAquiredPokerHand AddPlayer(IAquiredPokerPlayer aquiredPlayer);

        /// <summary>
        /// Determines if a player with a certain ID already was added to the hand
        /// Needed when importing from Poker Office to avoid adding a player twice
        /// </summary>
        /// <param name="ID">ID of the player</param>
        /// <returns></returns>
        bool PlayerExists(long id);

        /// <summary>
        /// Sorts Players by their position
        /// They need to be sorted to recreate and thus analyse the hand
        /// </summary>
        void SortPlayersByPosition();

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>Hand header with info about each player</returns>
        string ToString();

        /// <summary>
        /// Remove a Poker Player
        /// Needed when converting hand
        /// </summary>
        /// <param name="thePlayer">Player to remove</param>
        /// <returns>true if player could be removed</returns>
        bool RemovePlayer(IAquiredPokerPlayer thePlayer);

        void RemovePlayer(int index);

        IAquiredPokerHand InitializeWith(string site, ulong gameId, DateTime timeStamp, double BB, double SB, int totalPlayers);
    }
}