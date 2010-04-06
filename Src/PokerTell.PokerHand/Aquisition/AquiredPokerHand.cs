//Date: 5/2/2009

namespace PokerTell.PokerHand.Aquisition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Description of PokerHand.
    /// </summary>
    public class AquiredPokerHand : PokerHand, IAquiredPokerHand
    {
        protected List<IAquiredPokerPlayer> _players;

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AquiredPokerHand()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AquiredPokerHand"/> class. 
        /// Enables Hand creation, before number of players is known
        /// This is necessary during Parsing and when importing from other databases
        /// </summary>
        /// <param name="site"></param>
        /// <param name="gameId"></param>
        /// <param name="timeStamp"></param>
        /// <param name="BB"><see cref="BB"></see></param>
        /// <param name="SB"><see cref="SB"></see></param>
        public AquiredPokerHand(string site, ulong gameId, DateTime timeStamp, double BB, double SB)
            : this(site, gameId, timeStamp, BB, SB, -1)
        {
        }

        public AquiredPokerHand(string site, ulong gameId, DateTime timeStamp, double BB, double SB, int totalPlayers)
            : base(site, gameId, timeStamp, BB, SB, totalPlayers)
        {
            Initialize();
            InitializeWith(site, gameId, timeStamp, BB, SB, totalPlayers);
        }

        public IAquiredPokerHand InitializeWith(string site, ulong gameId, DateTime timeStamp, double BB, double SB, int totalPlayers)
        {
            InitializeBase(totalPlayers, site, gameId, timeStamp, BB, SB);
            return this;
        }

        void Initialize()
        {
            _players = new List<IAquiredPokerPlayer>();
            Seats = new List<IAquiredPlayerSeat>();
        }

        public ReadOnlyCollection<IAquiredPokerPlayer> Players
        {
            get { return _players.AsReadOnly(); }
        }

        /// <summary>
        /// Pot at the end of the hand
        /// </summary>
        public double TotalPot { get; set; }

        /// <summary>
        /// List of the seat numbers and appropriate Player Names
        /// </summary>
        public IList<IAquiredPlayerSeat> Seats { get; protected set; }

        public IAquiredPokerPlayer this[int index]
        {
            get { return Players[index]; }
        }

        /// <summary>
        /// Method to add a player to the hand
        /// This version is used by the Parser and when importing a hand from Poker Office
        /// </summary>
        /// <param name="P">Poker Player</param>
        public IAquiredPokerHand AddPlayer(IAquiredPokerPlayer aquiredPlayer)
        {
            _players.Add(aquiredPlayer);
            return this;
        }

        /// <summary>
        /// Determines if a player with a certain ID already was added to the hand
        /// Needed when importing from Poker Office to avoid adding a player twice
        /// </summary>
        /// <param name="ID">ID of the player</param>
        /// <returns></returns>
        public bool PlayerExists(long ID)
        {
            foreach (IAquiredPokerPlayer aquiredPlayer in this)
            {
                if (aquiredPlayer.Id == ID)
                {
                    return true;
                }
            }

            return false; // Not Found
        }

        /// <summary>
        /// Sorts Players by their position
        /// They need to be sorted to recreate and thus analyse the hand
        /// </summary>
        public void SortPlayersByPosition()
        {
            _players.Sort();

            // For Headsup, the small blind is the button and thus needs to be shown after the big blind
            if (_players.Count == 2) _players.Reverse();
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>Hand header with info about each player</returns>
        public override string ToString()
        {
            string handinfo = string.Empty;
            try
            {
                handinfo = string.Format(
                    "\nTour#: {0} Hand: #{1} {2} - {3} {4} ", TournamentId, GameId, DateAsString, TimeAsString, GameType);

                handinfo += string.Format("[{3}] BB:{0} SB:{1} TP:{2} Hero: {4}\n", BB, SB, TotalPlayers, Board, HeroName);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning what I have so far", excep);
            }

            try
            {
                foreach (IAquiredPokerPlayer iP in this)
                {
                    handinfo += iP.ToString();
                }
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning what I have so far", excep);
            }

            return handinfo;
        }

        /// <summary>
        /// Inumerator
        /// </summary>
        /// <returns>Enumerator of List of Players</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _players.GetEnumerator();
        }

        /// <summary>
        /// Remove a Poker Player
        /// Needed when converting hand
        /// </summary>
        /// <param name="thePlayer">Player to remove</param>
        /// <returns>true if player could be removed</returns>
        public bool RemovePlayer(IAquiredPokerPlayer thePlayer)
        {
            return _players.Remove(thePlayer);
        }

        public void RemovePlayer(int index)
        {
            _players.RemoveAt(index);
        }
    }
}