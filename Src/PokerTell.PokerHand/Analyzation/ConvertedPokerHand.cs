namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Extensions;

    [Serializable]
    public class ConvertedPokerHand : PokerHand, IConvertedPokerHand
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NonSerialized]
        readonly IConvertedPokerRound[] _sequences = new IConvertedPokerRound[(int)Streets.River + 1];

        [NonSerialized]
        int _id;

        IList<IConvertedPokerPlayer> _players = new List<IConvertedPokerPlayer>();

        int[] _playersInRound = new int[(int)Streets.River + 1];

        [NonSerialized]
        PokerHandStringConverter _pokerHandStringConverter;

        #endregion

        #region Constructors and Destructors

        public ConvertedPokerHand(string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
            InitializeWith(site, gameId, timeStamp, bb, sb, totalPlayers);
        }

        public ConvertedPokerHand()
        {
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

        /// <summary>
        /// Identity of hand as determined from the database
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Gets Players.
        /// </summary>
        public IList<IConvertedPokerPlayer> Players
        {
            get { return _players; }
            private set { _players = value; }
        }

        public int PlayersInFlop
        {
            get { return PlayersInRound[(int)Streets.Flop]; }
            set { PlayersInRound[(int)Streets.Flop] = value; }
        }

        public int PlayersInRiver
        {
            get { return PlayersInRound[(int)Streets.River]; }
            set { PlayersInRound[(int)Streets.River] = value; }
        }

        /// <summary>
        /// How many players are present at each round
        /// </summary>
        public int[] PlayersInRound
        {
            get { return _playersInRound; }
            private set { _playersInRound = value; }
        }

        public int PlayersInTurn
        {
            get { return PlayersInRound[(int)Streets.Turn]; }
            set { PlayersInRound[(int)Streets.Turn] = value; }
        }

        public string SequenceFlop
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.Flop]); }

            set { _sequences[(int)Streets.Flop] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        public string SequencePreFlop
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.PreFlop]); }

            set { _sequences[(int)Streets.PreFlop] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        public string SequenceRiver
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.River]); }

            set { _sequences[(int)Streets.River] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        public IConvertedPokerRound[] Sequences
        {
            get { return _sequences; }
        }

        public string SequenceTurn
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.Turn]); }

            set { _sequences[(int)Streets.Turn] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        PokerHandStringConverter PokerHandStringConverter
        {
            get { return _pokerHandStringConverter ?? (_pokerHandStringConverter = new PokerHandStringConverter()); }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        public IConvertedPokerPlayer this[int index]
        {
            get { return Players.ElementAt(index); }
        }

        #endregion

        public bool Equals(ConvertedPokerHand other)
        {
            return base.Equals(other) 
                && Players.ToArray().EqualsArray(other.Players.ToArray());
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

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
            convertedPlayer.ParentHand = this;
            Players.Add(convertedPlayer);

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

        public IConvertedPokerHand InitializeWith(
            string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
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

        public IConvertedPokerHand RemoveInactivePlayers()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                // Either no round or no action Preflop
                if (this[i].Rounds.Count < 1 || this[i][Streets.PreFlop].Count < 1)
                {
                    RemovePlayer(i);
                }
            }

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
            _players.Remove(_players.ElementAt(index));
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
                    convertedPlayer.InPosition[(int)street] = false;
                }

                // Parse backwards through all Players, and find first one with Action in that round
                // and set Position true
                for (int iPlayer = Players.Count - 1; iPlayer >= 0; iPlayer--)
                {
                    if (this[iPlayer].Rounds.Count > (int)street)
                    {
                        this[iPlayer].InPosition[(int)street] = true;
                        break;
                    }
                }
            }

            return this;
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
                foreach (IConvertedPokerPlayer iP in Players)
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
        public IEnumerator GetEnumerator()
        {
            return _players.GetEnumerator();
        }

        #endregion

        #region IEnumerable<IConvertedPokerPlayer>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        IEnumerator<IConvertedPokerPlayer> IEnumerable<IConvertedPokerPlayer>.GetEnumerator()
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
            HeroName = aquiredHand.HeroName;
        }

        #endregion
    }
}