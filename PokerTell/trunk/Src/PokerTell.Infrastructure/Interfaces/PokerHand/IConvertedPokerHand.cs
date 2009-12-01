namespace PokerTell.Infrastructure.Interfaces.PokerHand
{
    using System;
    using System.Collections.Generic;

    public interface IConvertedPokerHand : IPokerHand, IEnumerable<IConvertedPokerPlayer>
    {
        #region Properties

        /// <summary>
        /// Identity of hand as determined from the database
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets Players.
        /// </summary>
        IList<IConvertedPokerPlayer> Players { get; }

        /// <summary>
        /// How many players are present at each round
        /// </summary>
        int[] PlayersInRound { get; }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        IConvertedPokerRound[] Sequences { get; }

        #endregion

        #region Indexers

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        IConvertedPokerPlayer this[int index]
        {
            get;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add Player when creating a hand from 
        /// the database.
        /// </summary>
        /// <param name="convertedPlayer">
        /// The converted Player.
        /// </param>
        IConvertedPokerHand AddPlayer(IConvertedPokerPlayer convertedPlayer);

        IConvertedPokerHand AddPlayersFrom(
            IAquiredPokerHand aquiredHand, double startingPot, IConstructor<IConvertedPokerPlayer> convertedPlayerMake);

        IConvertedPokerHand InitializeWith(
            string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers);

        IConvertedPokerHand InitializeWith(IAquiredPokerHand aquiredHand);

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

        #endregion
    }
}