namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Services;

    using Tools.Extensions;

    /// <summary>
    /// Description of PokerHand.
    /// </summary>
    [Serializable]
    public class ConvertedPokerHand : PokerHand, IConvertedPokerHand
    {
        #region Constants and Fields

        /// <summary>
        /// The log.
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The lst sequences.
        /// </summary>
        List<IConvertedPokerRound> _lstSequences;

        /// <summary>
        /// The lst p.
        /// </summary>
        List<IConvertedPokerPlayer> _players;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerHand"/> class.
        /// </summary>
        public ConvertedPokerHand()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerHand"/> class.
        /// </summary>
        /// <param name="site">
        /// The site.
        /// </param>
        /// <param name="gameId">
        /// The game id.
        /// </param>
        /// <param name="timeStamp">
        /// The time stamp.
        /// </param>
        /// <param name="bb">
        /// The bb.
        /// </param>
        /// <param name="sb">
        /// The sb.
        /// </param>
        /// <param name="totalPlayers">
        /// The total players.
        /// </param>
        public ConvertedPokerHand(string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
            InitializeWith(site, gameId, timeStamp, bb, sb, totalPlayers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerHand"/> class.
        /// </summary>
        /// <param name="aquiredHand">
        /// The aquired hand.
        /// </param>
        public ConvertedPokerHand(IAquiredPokerHand aquiredHand)
        {
            InitializeWith(aquiredHand);
        }

        #endregion

        #region Properties

        int _handId;

        /// <summary>
        /// Identity of hand as determined from the database
        /// </summary>
        public int HandId
        {
            get { return _handId; }
            set { _handId = value; }
        }

        /// <summary>
        /// Gets Players.
        /// </summary>
        public ReadOnlyCollection<IConvertedPokerPlayer> Players
        {
            get { return _players.AsReadOnly(); }
        }

        int[] _playersInRound;

        /// <summary>
        /// How many players are present at each round
        /// </summary>
        public int[] PlayersInRound
        {
            get { return _playersInRound; }
            private set { _playersInRound = value; }
        }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        public ReadOnlyCollection<IConvertedPokerRound> Sequences
        {
            get { return _lstSequences.AsReadOnly(); }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        public IConvertedPokerPlayer this[int index]
        {
            get { return Players[index]; }
        }

        #endregion

        #region Public Methods

        public IConvertedPokerHand RemoveInactivePlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                // Either no round or no action Preflop
                if (this[i].Count < 1 || this[i][Streets.PreFlop].Count < 1)
                {
                    RemovePlayer(i);
                }
            }

            return this;
        }

        /// <summary>
        /// Determines for each round, how many active players were in it
        /// This is needed to filter the statistics by how many opponents a player faced when he acted
        /// </summary>
        public IConvertedPokerHand SetNumOfPlayersInEachRound()
        {
            for (Streets street = Streets.PreFlop; street <= Streets.River; street++)
            {
                int playerCount = 0;

                foreach (IConvertedPokerPlayer convertedPlayer in this)
                {
                    if (convertedPlayer.Rounds.Count > (int)street)
                    {
                        playerCount++;
                    }
                }

                PlayersInRound[(int)street] = playerCount;
            }
           
            return this;
        }

        /// <summary>
        /// Determines who was in position in each round of the hand
        /// This is needed for statistic analysis, since it is important to know if a player was in
        /// position when he acted a certain way
        /// </summary>
        public IConvertedPokerHand SetWhoHasPositionInEachRound()
        {
            for (Streets street = Streets.PreFlop; street <= Streets.River; street++)
            {
                // Initialize all to false
                foreach (IConvertedPokerPlayer convertedPlayer in this)
                {
                    convertedPlayer.InPosition[(int)street] = 0;
                }

                // Parse backwards through all Players, and find first one with Action in that round
                // and set Position true
                for (int iPlayer = Players.Count - 1; iPlayer >= 0; iPlayer--)
                {
                    if (this[iPlayer].Rounds.Count > (int)street)
                    {
                        this[iPlayer].InPosition[(int)street] = 1;
                        break;
                    }
                }
            }
            return this;
        }

        #endregion

        #region Implemented Interfaces

        #region IConvertedPokerHand

        /// <summary>
        /// Add Player when creating a hand from 
        /// the database.
        /// </summary>
        /// <param name="convertedPlayer">
        /// The converted Player.
        /// </param>
        public IConvertedPokerHand AddPlayer(IConvertedPokerPlayer convertedPlayer)
        {
            _players.Add(convertedPlayer);
            return this;
        }

        public IConvertedPokerHand AddPlayersFrom(
            IAquiredPokerHand aquiredHand, double startingPot, IConstructor<IConvertedPokerPlayer> convertedPlayerMake)
        {
            foreach (IAquiredPokerPlayer aquiredPlayer in aquiredHand)
            {
                double mBefore = startingPot > 0 ? aquiredPlayer.StackBefore / startingPot : 0;
                double mAfter = startingPot > 0 ? aquiredPlayer.StackAfter / startingPot : 0;

                IConvertedPokerPlayer convertedPlayer =
                    convertedPlayerMake.New.InitializeWith(
                        aquiredPlayer.Name, 
                        mBefore, 
                        mAfter, 
                        aquiredPlayer.Position, 
                        aquiredHand.TotalPlayers, 
                        aquiredPlayer.Holecards);

                convertedPlayer.SeatNumber = aquiredPlayer.SeatNumber;

                AddPlayer(convertedPlayer);
            }

            return this;
        }

        /// <summary>
        /// The add sequence.
        /// </summary>
        /// <param name="theRound">
        /// The the round.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public void AddSequence(IConvertedPokerRound theRound)
        {
            if (_lstSequences.Count >= 4)
            {
                throw new ArgumentOutOfRangeException("theRound", "A maximum of 4 sequences is allowed");
            }

            // Only add if it contains actions
            if (theRound.Count > 0)
            {
                _lstSequences.Add(theRound);
            }
        }

        public IConvertedPokerHand InitializeWith(
            string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
            Initialize();

            InitializeBase(totalPlayers, site, gameId, timeStamp, bb, sb);

            return this;
        }

        public IConvertedPokerHand InitializeWith(IAquiredPokerHand aquiredHand)
        {
            InitializeWith(
                aquiredHand.Site, 
                aquiredHand.GameId, 
                aquiredHand.TimeStamp, 
                aquiredHand.BB, 
                aquiredHand.SB, 
                aquiredHand.TotalPlayers);

            CopyAdditionalPropertiesFrom(aquiredHand);

            return this;
        }

        /// <summary>
        /// The remove player.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void RemovePlayer(int index)
        {
            _players.RemoveAt(index);
        }

        /// <summary>
        /// Remove a Poker Player
        /// Needed when converting hand
        /// </summary>
        /// <param name="thePlayer">Player to remove</param>
        /// <returns>true if player could be removed</returns>
        public bool RemovePlayer(IConvertedPokerPlayer thePlayer)
        {
            return _players.Remove(thePlayer);
        }

        /// <summary>
        /// The set strategic positions for all players.
        /// </summary>
        public void SetStrategicPositionsForAllPlayers()
        {
            foreach (IConvertedPokerPlayer convertedPlayer in this)
            {
                convertedPlayer.SetStrategicPosition(TotalPlayers);
            }
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>
        /// Hand header with info about each player
        /// </returns>
        public override string ToString()
        {
            string handinfo = string.Empty;
            try
            {
                handinfo = string.Format(
                    "\nTour#: {0} Hand: #{1} {2} - {3} ", TournamentId, GameId, DateAsString, TimeAsString);

                handinfo += string.Format("[{3}] BB:{0} SB:{1} TP:{2}\n", BB, SB, TotalPlayers, Board);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning what I have so far", excep);
            }

            try
            {
                foreach (IConvertedPokerPlayer iP in this)
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

        #endregion

        #region IEnumerable

        /// <summary>
        /// Inumerator
        /// </summary>
        /// <returns>
        /// Enumerator of List of Players
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _players.GetEnumerator();
        }

        #endregion

        #endregion

        #region Methods

        void CopyAdditionalPropertiesFrom(IAquiredPokerHand aquiredHand)
        {
            AllNames = aquiredHand.AllNames;
            Board = aquiredHand.Board;
            TableName = aquiredHand.TableName;
            TotalSeats = aquiredHand.TotalSeats;
            TournamentId = aquiredHand.TournamentId;
            Ante = aquiredHand.Ante;
        }

        void Initialize()
        {
            _lstSequences = new List<IConvertedPokerRound>();
            PlayersInRound = new int[(int)Streets.River + 1];
            _players = new List<IConvertedPokerPlayer>();
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as ConvertedPokerHand);
        }

        public bool Equals(ConvertedPokerHand other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.GetHashCode().Equals(GetHashCode())
                   && other.Players.ToArray().EqualsArray(Players.ToArray())
                   && other.PlayersInRound.EqualsArray(PlayersInRound);
                   
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result * 397) ^ HandId;
                return result;
            }
        }
    }
}