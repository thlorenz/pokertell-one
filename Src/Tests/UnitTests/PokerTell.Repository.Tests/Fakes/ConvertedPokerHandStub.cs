namespace PokerTell.Repository.Tests.Fakes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class ConvertedPokerHandStub : IConvertedPokerHand
    {
        /// <summary>
        /// Array containing the names of all players present at the table
        /// </summary>
        public IList<string> AllNames { get; set; }

        public double Ante { get; set; }

        /// <summary>
        /// The Big Blind
        /// </summary>
        public double BB { get; set; }

        /// <summary>
        /// Contains all cards on the board (As Kh Qd
        /// </summary>
        public string Board { get; set; }

        /// <summary>
        /// String representation of date obtained from TimeStamp
        /// </summary>
        public string DateAsString { get; set; }

        /// <summary>
        /// The ID of the hand set by the Site and given in the Hand History
        /// </summary>
        public ulong GameId { get; set; }

        /// <summary>
        /// Identity of hand as determined from the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets Players.
        /// </summary>
        public IList<IConvertedPokerPlayer> Players { get; set; }

        /// <summary>
        /// How many players are present at each round
        /// </summary>
        public int[] PlayersInRound { get; set; }

        /// <summary>
        /// The small Blind
        /// </summary>
        public double SB { get; set; }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        public IConvertedPokerRound[] Sequences { get; set; }

        public string SequenceFlop { get; set; }

        public string SequencePreFlop { get; set; }

        public string SequenceRiver { get; set; }

        public string SequenceTurn { get; set; }

        public int PlayersInFlop { get; set; }

        public int PlayersInTurn { get; set; }

        public int PlayersInRiver { get; set; }

        /// <summary>
        /// Name of the PokerSite the hand occurred on
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// The name of the table the hand occurred at - given in Hand History
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// String representation of time obtained from TimeStamp
        /// </summary>
        public string TimeAsString { get; set; }

        /// <summary>
        /// Date and Time when the hand occured
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Total Players that acted in the hand
        /// </summary>
        public int TotalPlayers { get; set; }

        /// <summary>
        /// Total number seats at the table
        /// </summary>
        public int TotalSeats { get; set; }

        /// <summary>
        /// The ID of the torunament set by the Site and given in the Hand History
        /// </summary>
        public ulong TournamentId { get; set; }

        public string HeroName { get; set; }

        public GameTypes GameType { get; set; }

        public string HandHistory { get; set; }

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        public IConvertedPokerPlayer this[int index]
        {
            get { return Players.ElementAt(index); }
        }

        public int CompareTo(object obj)
        {
            // throw new NotImplementedException();
            return 0;
        }

        /// <summary>
        /// Add Player when creating a hand from 
        /// the database.
        /// </summary>
        /// <param name="convertedPlayer">
        /// The converted Player.
        /// </param>
        public IConvertedPokerHand AddPlayer(IConvertedPokerPlayer convertedPlayer)
        {
            return this;
        }

        public IConvertedPokerHand AddPlayersFrom(
            IAquiredPokerHand aquiredHand, double startingPot, IConstructor<IConvertedPokerPlayer> convertedPlayerMake)
        {
            // throw new NotImplementedException();
            return this;
        }

        public IConvertedPokerHand InitializeWith(
            string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
            // throw new NotImplementedException();
            return this;
        }

        public IConvertedPokerHand InitializeWith(IAquiredPokerHand aquiredHand)
        {
            // throw new NotImplementedException();
            return this;
        }

        public IConvertedPokerHand RemoveInactivePlayers()
        {
            // throw new NotImplementedException();
            return this;
        }

        /// <summary>
        /// Remove a Poker Player
        /// Needed when converting hand
        /// </summary>
        /// <param name="thePlayer">Player to remove</param>
        /// <returns>true if player could be removed</returns>
        public bool RemovePlayer(IConvertedPokerPlayer thePlayer)
        {
            // throw new NotImplementedException();
            return true;
        }

        public void RemovePlayer(int index)
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Determines for each round, how many active players were in it
        /// This is needed to filter the statistics by how many opponents a player faced when he acted
        /// </summary>
        public IConvertedPokerHand SetNumOfPlayersInEachRound()
        {
            // throw new NotImplementedException();
            return this;
        }

        /// <summary>
        /// The set strategic positions for all players.
        /// </summary>
        public void SetStrategicPositionsForAllPlayers()
        {
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Determines who was in position in each round of the hand
        /// This is needed for statistic analysis, since it is important to know if a player was in
        /// position when he acted a certain way
        /// </summary>
        public IConvertedPokerHand SetWhoHasPositionInEachRound()
        {
            // throw new NotImplementedException();
            return this;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<IConvertedPokerPlayer> IEnumerable<IConvertedPokerPlayer>.GetEnumerator()
        {
            return Players.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return Players.GetEnumerator();
        }
    }
}