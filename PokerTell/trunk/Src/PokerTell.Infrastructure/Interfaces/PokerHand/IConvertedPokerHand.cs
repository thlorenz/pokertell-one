namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;

    public interface IConvertedPokerHand : IPokerHand, IEnumerable
    {
        IConvertedPokerHand InitializeWith(string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers);

        /// <summary>
        /// Identity of hand as determined from the database
        /// </summary>
        int HandId { get; set; }

        /// <summary>
        /// Note that the user made about this hand using the Reporting tools
        /// Each array element will contain one line of the RichTextBox
        /// </summary>
        string[] Note { get; set; }

        /// <summary>
        /// Gets Players.
        /// </summary>
        ReadOnlyCollection<IConvertedPokerPlayer> Players { get; }

        /// <summary>
        /// How many players are present at each round
        /// </summary>
        int[] PlayersInRound { get; }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        ReadOnlyCollection<IConvertedPokerRound> Sequences { get; }

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        IConvertedPokerPlayer this[int index]
        {
            get;
        }

        /// <summary>
        /// Add Player when creating a hand from 
        /// the database.
        /// </summary>
        /// <param name="convertedPlayer">
        /// The converted Player.
        /// </param>
        IConvertedPokerHand AddPlayer(IConvertedPokerPlayer convertedPlayer);

        /// <summary>
        /// The add sequence.
        /// </summary>
        /// <param name="theRound">
        /// The the round.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        void AddSequence(IConvertedPokerRound theRound);

        /// <summary>
        /// The set strategic positions for all players.
        /// </summary>
        void SetStrategicPositionsForAllPlayers();

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>
        /// Hand header with info about each player
        /// </returns>
        string ToString();

        /// <summary>
        /// Remove a Poker Player
        /// Needed when converting hand
        /// </summary>
        /// <param name="thePlayer">Player to remove</param>
        /// <returns>true if player could be removed</returns>
        bool RemovePlayer(IConvertedPokerPlayer thePlayer);

        void RemovePlayer(int index);

        IConvertedPokerHand InitializeWith(IAquiredPokerHand aquiredHand);

        IConvertedPokerHand AddPlayersFrom(IAquiredPokerHand aquiredHand, double startingPot, IConstructor<IConvertedPokerPlayer> convertedPlayerMake);

        IConvertedPokerHand RemoveInactivePlayers();

        /// <summary>
        /// Determines for each round, how many active players were in it
        /// This is needed to filter the statistics by how many opponents a player faced when he acted
        /// </summary>
        IConvertedPokerHand SetNumOfPlayersInEachRound();

        /// <summary>
        /// Determines who was in position in each round of the hand
        /// This is needed for statistic analysis, since it is important to know if a player was in
        /// position when he acted a certain way
        /// </summary>
        IConvertedPokerHand SetWhoHasPositionInEachRound();
    }
}