namespace PokerTell.PokerHand.Aquisition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    /// <summary>
    /// Description of PokerPlayer.
    /// </summary>
    public class AquiredPokerPlayer : PokerPlayer, IAquiredPokerPlayer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        public AquiredPokerPlayer()
        {
            Rounds = new List<IAquiredPokerRound>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AquiredPokerPlayer"/> class. 
        /// Create a player with given characteristics
        /// Used when parsing a hand
        /// </summary>
        /// <param name="name">
        /// <see cref="name"></see>
        /// </param>
        /// <param name="stack">
        /// <see cref="stack"></see>
        /// </param>
        public AquiredPokerPlayer(string name, double stack)
            : this()
        {
            InitializeWith(name, stack);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AquiredPokerPlayer"/> class. 
        /// Create a player with given characteristics
        /// Used when adding a hand from Poker Office
        /// when using this constructor positions, position names, playernames need to be set later
        /// </summary>
        /// <param name="playerId">
        /// <see cref="PokerPlayer.Id"></see>
        /// </param>
        /// <param name="seatNum">
        /// <see cref="PokerPlayer.SeatNumber"></see>
        /// </param>
        /// <param name="holecards">
        /// <see cref="AquiredPokerPlayer.Holecards"></see>
        /// </param>
        public AquiredPokerPlayer(long playerId, int seatNum, string holecards)
            : this()
        {
            InitializeWith(seatNum, holecards, playerId);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Seat relative to seat of small blind
        /// </summary>
        public int RelativeSeatNumber { get; set; }

        /// <summary>
        /// Stack of Player after the hand is played
        /// Calculated by substracting  betting,raising and calling amounts and adding winning amounts
        /// for all rounds
        /// </summary>
        public double StackAfter
        {
            get { return StackBefore + ChipsGained(); }
        }

        /// <summary>
        /// Stack of Player at the start of the hand
        /// Determined by the Parser from the Hand History
        /// </summary>
        public double StackBefore { get; set; }

        /// <summary>
        /// Number of Rounds that player saw
        /// </summary>
        public int Count
        {
            get { return Rounds.Count; }
        }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        public IList<IAquiredPokerRound> Rounds { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IAquiredPokerPlayer

        /// <summary>
        /// Add a new Poker Round to the player
        /// </summary>
        public IAquiredPokerPlayer AddRound()
        {
            AddRound(new AquiredPokerRound());
            return this;
        }

        /// <summary>
        /// Add a given Poker round to the Player
        /// </summary>
        /// <param name="aquiredRound">Poker Round to add</param>
        public IAquiredPokerPlayer AddRound(IAquiredPokerRound aquiredRound)
        {
            try
            {
                if (aquiredRound == null)
                {
                    throw new ArgumentNullException(
                        "aquiredRound", "Could be caused because wrong correct Round type was passed in.");
                }

                if (Count < 4)
                {
                    Rounds.Add(aquiredRound);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("aquiredRound", 
                        "Tried to add BettingRound when Count is >= 4 already");
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            return this;
        }

        public IAquiredPokerPlayer InitializeWith(string name, double stack)
        {
            try
            {
                if (name == null)
                {
                    throw new ArgumentNullException("Name");
                }

                Name = name;
                StackBefore = stack;

                Holecards = "?? ??";
                Position = -1;
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Unhandled", excep);
            }

            return this;
        }

        public IAquiredPokerPlayer InitializeWith(int seatNum, string holecards, long playerId)
        {
            try
            {
                if (seatNum < 0 || seatNum > 10)
                {
                    throw new ArgumentOutOfRangeException("seatNum", seatNum, "Value must be between 0 and 10");
                }

                if (holecards == null)
                {
                    throw new ArgumentNullException("holecards");
                }

                Id = playerId;
                SeatNumber = seatNum;
                Holecards = holecards;
                Position = -1;

                // Unknown when adding from Pokeroffice
                StackBefore = 0;
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
            }

            return this;
        }

        /// <summary>
        /// This is called from the Parser, after all players have been added.
        /// </summary>
        /// <description>
        /// Important: At this point PlayerCount = TotalPlayers at table allowed to play
        ///	Determines and sets the Position of the player in a Seat# x
        ///	 Result:
        ///	 SB will be 0
        ///	 BB will be 1
        ///	 etc.
        ///	 Button will be TotalPlayers -1
        /// Then it determines the strategic position (SB,BB,EA,MI) etc. by calling SetStrategicPosition
        /// </description>
        /// <param name="sbPosition">Position (Seat) of the small blind</param>
        /// <param name="playerCount">Total amount of players in the hand - empty seats not counted</param>
        /// <returns>true if all went well</returns>
        public bool SetPosition(int sbPosition, int playerCount)
        {
            if (RelativeSeatNumber > playerCount)
            {
                LogInvalidInputError(playerCount, sbPosition);
                return false;
            }
            
            if (RelativeSeatNumber >= sbPosition)
            {
                Position = RelativeSeatNumber - sbPosition;
            }
            else
            {
                Position = playerCount - (sbPosition - RelativeSeatNumber);
            }

            if (Position < 0)
            {
                LogInvalidInputError(playerCount, sbPosition);
                return false;
            }

            return true;
        }

        void LogInvalidInputError(int playerCount, int sbPosition)
        {
            Log.DebugFormat(
                "Pos_num {0} PlayerCount: {1} Seat: {2} SBPosition: {3}", 
                Position, 
                playerCount, 
                sbPosition, 
                RelativeSeatNumber);
        }

        /// <summary>
        /// Gives string representation of Players info and actions
        /// </summary>
        /// <returns>String representation of Players info and actions</returns>
        public override string ToString()
        {
            try
            {
                return string.Format(
                    "[{3} {0} {1}] {4} - {5} \t{2}\n", 
                    Position, 
                    Name, 
                    BettingRoundsToString(), 
                    Holecards, 
                    StackBefore, 
                    StackAfter);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning empty string", excep);
                return string.Empty;
            }
        }

        #endregion

        #endregion

        #region Methods

        protected double ChipsGained()
        {
            double gainedChips = 0;

            foreach (IAquiredPokerRound iRound in this)
            {
                gainedChips += iRound.ChipsGained;
            }

            return gainedChips;
        }

        #endregion

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IAquiredPokerRound this[int index]
        {
            get { return Rounds[index]; }
        }

        [NonSerialized]
        long _id;

        /// <summary>
        /// Id of player in Database
        /// </summary>
        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="theStreet">
        /// The the street.
        /// </param>
        public IAquiredPokerRound this[Streets theStreet]
        {
            get { return this[(int)theStreet]; }
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((AquiredPokerPlayer)obj);
        }

        public bool Equals(AquiredPokerPlayer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other._holecards, _holecards) && other.SeatNumber == SeatNumber && Equals(other.Name, Name) &&
                   other.Id == Id && other.Position == Position && Equals(other.Rounds, Rounds);
        }

        public override int GetHashCode()
        {
            int result = Holecards != null ? Holecards.GetHashCode() : 0;
            result = (result * 397) ^ SeatNumber;
            result = (result * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            result = (result * 397) ^ Id.GetHashCode();
            result = (result * 397) ^ Position;
            result = (result * 397) ^ (Rounds != null ? Rounds.GetHashCode() : 0);
            return result;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((IAquiredPokerPlayer)obj);
        }

        public int CompareTo(IAquiredPokerPlayer other)
        {
            if (Position < other.Position)
            {
                return -1;
            }

            if (Position > other.Position)
            {
                return 1;
            }

            return 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rounds.GetEnumerator();
        }

        string BettingRoundsToString()
        {
            string betting = string.Empty;
            try
            {
                // Iterate through rounds pre-flop to river
                foreach (object iB in this)
                {
                    betting += "| " + iB;
                }
            }
            catch (ArgumentNullException excep)
            {
                excep.Data.Add("betRoundCount = ", Rounds.Count);
                Log.Error("Returning betting Rounds I go so far", excep);
            }

            return betting;
        }
    }
}