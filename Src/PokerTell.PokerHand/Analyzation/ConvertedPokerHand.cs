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

    /// <summary>
    /// The converted poker hand.
    /// </summary>
    [Serializable]
    public class ConvertedPokerHand : PokerHand, IConvertedPokerHand
    {
        /// <summary>
        /// The log.
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The _sequences.
        /// </summary>
        [NonSerialized]
        readonly IConvertedPokerRound[] _sequences = new IConvertedPokerRound[(int)Streets.River + 1];

        /// <summary>
        /// The _id.
        /// </summary>
        [NonSerialized]
        int _id;

        /// <summary>
        /// The _players.
        /// </summary>
        IList<IConvertedPokerPlayer> _players = new List<IConvertedPokerPlayer>();

        /// <summary>
        /// The _players in round.
        /// </summary>
        int[] _playersInRound = new int[(int)Streets.River + 1];

        /// <summary>
        /// The _poker hand string converter.
        /// </summary>
        [NonSerialized]
        PokerHandStringConverter _pokerHandStringConverter;

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

        /// <summary>
        /// Gets or sets PlayersInFlop.
        /// </summary>
        public int PlayersInFlop
        {
            get { return PlayersInRound[(int)Streets.Flop]; }
            set { PlayersInRound[(int)Streets.Flop] = value; }
        }

        /// <summary>
        /// Gets or sets PlayersInRiver.
        /// </summary>
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

        /// <summary>
        /// Gets or sets PlayersInTurn.
        /// </summary>
        public int PlayersInTurn
        {
            get { return PlayersInRound[(int)Streets.Turn]; }
            set { PlayersInRound[(int)Streets.Turn] = value; }
        }

        /// <summary>
        /// Gets or sets SequenceFlop.
        /// </summary>
        public string SequenceFlop
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.Flop]); }

            set { _sequences[(int)Streets.Flop] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        /// <summary>
        /// Gets or sets SequencePreFlop.
        /// </summary>
        public string SequencePreFlop
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.PreFlop]); }

            set { _sequences[(int)Streets.PreFlop] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        /// <summary>
        /// Gets or sets SequenceRiver.
        /// </summary>
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

        /// <summary>
        /// Gets or sets SequenceTurn.
        /// </summary>
        public string SequenceTurn
        {
            get { return PokerHandStringConverter.BuildSqlStringFrom(_sequences[(int)Streets.Turn]); }

            set { _sequences[(int)Streets.Turn] = PokerHandStringConverter.ConvertedRoundFrom(value); }
        }

        /// <summary>
        /// Gets PokerHandStringConverter.
        /// </summary>
        PokerHandStringConverter PokerHandStringConverter
        {
            get { return _pokerHandStringConverter ?? (_pokerHandStringConverter = new PokerHandStringConverter()); }
        }

        /// <summary>
        /// List of all Poker Players in the hand
        /// </summary>
        public IConvertedPokerPlayer this[int index]
        {
            get { return Players.ElementAt(index); }
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public bool Equals(ConvertedPokerHand other)
        {
            return base.Equals(other)
                   && Players.ToArray().EqualsArray(other.Players.ToArray());
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
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

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The get hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
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
            convertedPlayer.ParentHand = this;
            Players.Add(convertedPlayer);

            return this;
        }

        /// <summary>
        /// The add players from.
        /// </summary>
        /// <param name="aquiredHand">
        /// The aquired hand.
        /// </param>
        /// <param name="startingPot">
        /// The starting pot.
        /// </param>
        /// <param name="convertedPlayerMake">
        /// The converted player make.
        /// </param>
        /// <returns>
        /// </returns>
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
        /// The initialize with.
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
        /// <returns>
        /// </returns>
        public IConvertedPokerHand InitializeWith(
            string site, ulong gameId, DateTime timeStamp, double bb, double sb, int totalPlayers)
        {
            InitializeBase(totalPlayers, site, gameId, timeStamp, bb, sb);
            return this;
        }

        /// <summary>
        /// The initialize with.
        /// </summary>
        /// <param name="aquiredHand">
        /// The aquired hand.
        /// </param>
        /// <returns>
        /// </returns>
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
        /// The remove inactive players.
        /// </summary>
        /// <returns>
        /// </returns>
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

        public IConvertedPokerHand AdjustOrderOfPlayersIfItIsHeadsUp()
        {
            if (TotalPlayers == 2)
            {
                Players = Players.Reverse().ToList();
                for (int position = 0; position <= 1; position++)
                    if (Players[position] != null)
                        Players[position].Position = position;
            }
            
            return this;
        }
        /*
         
            if (TotalSeats == 2 && Players.Count == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Players[i].StrategicPosition == StrategicPositions.BU)
                        Players[i].Position = 1;
                    else if (Players[i].StrategicPosition == StrategicPositions.BB)
                        Players[i].Position = 0;
                }

                Players = Players.OrderBy(p => p.Position).ToList();
            }
         */
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
                    "\nTour#: {0} Hand: #{1} {2} - {3} {4} ", TournamentId, GameId, DateAsString, TimeAsString, GameType);

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

        /// <summary>
        /// The copy additional properties from.
        /// </summary>
        /// <param name="aquiredHand">
        /// The aquired hand.
        /// </param>
        void CopyAdditionalPropertiesFrom(IAquiredPokerHand aquiredHand)
        {
            AllNames = aquiredHand.AllNames;
            Board = aquiredHand.Board;
            TableName = aquiredHand.TableName;
            TotalSeats = aquiredHand.TotalSeats;
            TournamentId = aquiredHand.TournamentId;
            Ante = aquiredHand.Ante;
            HeroName = aquiredHand.HeroName;
            GameType = aquiredHand.GameType;
            HandHistory = aquiredHand.HandHistory;
        }
    }
}