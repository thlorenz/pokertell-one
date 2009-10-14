namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    using PokerTell.Infrastructure;

    using Tools;

    /// <summary>
    /// Description of PokerPlayer.
    /// </summary>
    public class ConvertedPokerPlayer : PokerPlayer, IConvertedPokerPlayer
    {
        #region Constants and Fields

        /// <summary>
        /// The Log.
        /// </summary>
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerPlayer"/> class.
        /// </summary>
        public ConvertedPokerPlayer()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerPlayer"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="mBefore">
        /// The m before.
        /// </param>
        /// <param name="mAfter">
        /// The m after.
        /// </param>
        /// <param name="positionNum">
        /// The position num.
        /// </param>
        /// <param name="totalPlayers">
        /// The total players.
        /// </param>
        /// <param name="holecards">
        /// The holecards.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ConvertedPokerPlayer(
            string name, double mBefore, double mAfter, int positionNum, int totalPlayers, string holecards)
            : this()
        {
            InitializeWith(name, mBefore, mAfter, positionNum, totalPlayers, holecards);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Is player in position on Flop, Turn or River? 0 = yes, 1 = no
        /// </summary>
        public int[] InPosition { get; private set; }

        /// <summary>
        /// M of player after the hand is over
        /// </summary>
        public int MAfter { get; set; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        public int MBefore { get; set; }

        /// <summary>
        /// Position, that a preflop raise came from, before player could act
        /// </summary>
        public int PreflopRaiseInFrontPos { get; private set; }

        /// <summary>
        /// Contains Sequence strings for each Round of the Player
        /// representing the way he acted or reacted to opponents actions
        /// Sometimes referred to as the 'line' and is used to determine bettin patterns
        /// during statistical analysis
        /// </summary>
        public string[] Sequence { get; private set; }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        public StrategicPositions StrategicPosition { get; private set; }

        #endregion

        #region Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IConvertedPokerRound this[int index]
        {
            get { return (IConvertedPokerRound)Rounds[index]; }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="theStreet">
        /// The the street.
        /// </param>
        public IConvertedPokerRound this[Streets theStreet]
        {
            get { return this[(int)theStreet]; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a new Poker Round to the player
        /// </summary>
        public IConvertedPokerPlayer AddRound()
        {
            AddRound(new ConvertedPokerRound());

            return this;
        }

        /// <summary>
        /// Add a given Poker round to the Player
        /// </summary>
        /// <param name="convertedRound">
        /// The converted Round.
        /// </param>
        public IConvertedPokerPlayer AddRound(IConvertedPokerRound convertedRound)
        {
            try
            {
                if (convertedRound == null)
                {
                    throw new ArgumentNullException("convertedRound");
                }

                if (Count < 4)
                {
                    _rounds.Add(convertedRound);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("convertedRound");
                }
            }
            catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
            }

            return this;
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return Rounds.GetEnumerator();
        }

        public IConvertedPokerPlayer InitializeWith(string name, double mBefore, double mAfter, int positionNum, int totalPlayers, string holecards)
        {
            Initialize();

            try
            {
                if (positionNum < 0 || positionNum > 9)
                {
                    throw new ArgumentOutOfRangeException("positionNum", positionNum, "Value must be between 0 and 9");
                }

                if (totalPlayers < 2 || totalPlayers > 10)
                {
                    throw new ArgumentOutOfRangeException(
                        "TotalPlayers", totalPlayers, "Value must be between 2 and 10");
                }

                if (holecards == null)
                {
                    throw new ArgumentNullException("holecards");
                }

                if (name == null)
                {
                    throw new ArgumentNullException("Name");
                }

                Name = name;

                MBefore = Convert.ToInt32(mBefore);
                MAfter = Convert.ToInt32(mAfter);

                Position = positionNum;

                SetStrategicPosition(totalPlayers);

                Holecards = holecards;
                InPosition = new int[4];
                PreflopRaiseInFrontPos = -1;
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
            }

            return this;
        }

        /// <summary>
        /// Determines a Sequence string, representing, what the player did in a round
        /// </summary>
        /// <param name="currentSequence">
        /// Actions affecting us that happened so far
        /// </param>
        /// <param name="myNextAction">
        /// What am I about to do
        /// </param>
        /// <param name="street">
        /// Preflop,Flop, Turn, etc.
        /// </param>
        /// <para>
        /// Sequences are of the following format: 
        /// Relevant actions are listed in order. ActionTypes abreviations are used, except for bet, in that case
        /// the normalized Bet size is listed.
        /// if we assume a bet of 0.5 which is normalized to 5 (multiplied by 10), we end up with the following 
        /// representations for the following ActionSequences
        /// HeroB - "5"
        /// HeroXOppBHeroF - "X5F"
        /// HeroXOppBHeroC - "X5C"
        /// HeroXOppBHeroR - "X5R"
        /// OppBHeroF - "5F"
        /// OppBHeroC - "5C"
        /// OppBHeroR - "5R"
        /// Preflop - "F" or "C", or "R"
        /// It will also produce Sequences such as "X5RC" (he checked, opp bet 0.5, another opp raised and he called)
        /// These don't fit in any of the above standard situations and cannot be included in the standard
        /// statistics analysis. 
        /// They will automatically be ignored when querying for statistics because they won't match any of the
        /// standard Sequence strings.
        /// </para>
        public void SetActionSequence(ref string currentSequence, IConvertedPokerAction myNextAction, Streets street)
        {
            // Add what we did and  what happened so far to determine our ActionSequence
            // Once we added a sequence part w/ a number (which represents a bet), we will stop adding to the sequence
            // because we don't want to get so deep into the sequence. For instance we want to know, if he raised a bet but not
            // how he reacted to a reraise to that raise.
            // These further reactions will be analysed, once the user requests it

            // Preflop we only are interested in the first action
            if (street == Streets.PreFlop)
            {
                // Only consider first action/reaction
                if (string.IsNullOrEmpty(Sequence[(int)street]))
                {
                    // CurrentSequence will be empty for unraised pot and contain R for each raise/reraise in a raised pot
                    Sequence[(int)street] = currentSequence + myNextAction.What;

                    // It is now a raised pot for all players to come
                    if (myNextAction.What == ActionTypes.R)
                    {
                        currentSequence = currentSequence + myNextAction.What;
                    }
                }

                return;
            }

            // PostFlop, once we have a bet or reaction to bet (represented by number) we don't need to know more
            const string patContainsNumber = "[0-9]";
            if (Regex.IsMatch(Sequence[(int)street], patContainsNumber))
            {
                return;
            }

            switch (myNextAction.What)
            {
                case ActionTypes.F:
                case ActionTypes.C:
                case ActionTypes.X:
                    Sequence[(int)street] = Sequence[(int)street] + currentSequence + myNextAction.What;
                    break;
                case ActionTypes.B:
                    var normalizedRatio =
                        (int)
                        (10.0 * Normalizer.NormalizeToKeyValues(ApplicationProperties.BetSizeKeys, myNextAction.Ratio));

                    Sequence[(int)street] = Sequence[(int)street] + currentSequence + normalizedRatio;

                    currentSequence = currentSequence + normalizedRatio;
                    break;
                case ActionTypes.R:
                    Sequence[(int)street] = Sequence[(int)street] + currentSequence + myNextAction.What;
                    currentSequence = currentSequence + myNextAction.What;
                    break;
            }
        }

        /// <summary>
        /// Determines and sets the strategic Position of the player
        /// </summary>
        /// <description>
        /// The number from 0 - 9 (10 player table)
        /// will be transformed into a number from (SB - BU) - SB,BB,EA,MI,LT,CO,BU
        /// to do this, all we need to do is to see, how far the player is away from the button
        /// and if he is SB or BB.
        /// 
        /// This works for a 2 - 10 Player Table
        /// Note that only with at least 8 players will there be an early Position
        /// any table smaller will start with MI Position, some will only have EA etc.
        /// This way the playing style is analyzed only according to the relative position to
        /// the BU and the actual table size becomes irrelevant
        /// </description>
        /// <param name="playerCount">
        /// A number from 0 (SB) to TotalPlayers -1 (BU)
        /// </param>
        public void SetStrategicPosition(int playerCount)
        {
            if (Position < 0)
            {
                Log.DebugFormat("Player NamePosition - Player has Pos_num {0}", Position);
            }

            if (playerCount > 2)
            {
                NoHeadsUp(playerCount);
            }
            else
            {
                HeadsUp();
            }
        }

        /// <summary>
        /// Gives string representation of Players info and actions
        /// </summary>
        /// <returns>
        /// String representation of Players info and actions
        /// </returns>
        public override string ToString()
        {
            try
            {
                return string.Format(
                    "[{5} {0} {1}] {2}-{3}\t{4}\n", 
                    StrategicPosition, 
                    Name, 
                    MBefore, 
                    MAfter, 
                    BettingRoundsToString(), 
                    Holecards);
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Returning empty string", excep);
                return string.Empty;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The heads up.
        /// </summary>
        void HeadsUp()
        {
            // correct SB to BU b/c that is strategically more correct
            switch (Position)
            {
                case 1:
                    StrategicPosition = StrategicPositions.BB;
                    Position = 0;
                    break;
                case 0:
                    StrategicPosition = StrategicPositions.BU;
                    Position = 1;
                    break;
            }
        }

        void Initialize()
        {
            InitializeSequence();
            InitializeInPosition();
        }

        /// <summary>
        /// The initialize in position.
        /// </summary>
        void InitializeInPosition()
        {
            InPosition = new int[4];
            for (int i = 0; i < Sequence.Length; i++)
            {
                // 2 means no action in round and is the default value in the database
                InPosition[i] = 2;
            }
        }

        /// <summary>
        /// The initialize sequence.
        /// </summary>
        void InitializeSequence()
        {
            Sequence = new string[4];
            for (int i = 0; i < Sequence.Length; i++)
            {
                Sequence[i] = string.Empty;
            }
        }

        /// <summary>
        /// The no heads up.
        /// </summary>
        /// <param name="playerCount">
        /// The player count.
        /// </param>
        void NoHeadsUp(int playerCount)
        {
            StrategicPosition = StrategicPositions.SB;
            switch (playerCount - Position)
            {
                case 1:
                    StrategicPosition = StrategicPositions.BU;
                    break;
                case 2:
                    StrategicPosition = StrategicPositions.CO;
                    break;
                case 3:
                    StrategicPosition = StrategicPositions.LT;
                    break;
                case 4:
                case 5:
                    StrategicPosition = StrategicPositions.MI;
                    break;
                case 6:
                case 7:
                case 8:
                    StrategicPosition = StrategicPositions.EA;
                    break;
            }

            // Name BB and SB
            switch (Position)
            {
                case 0:
                    StrategicPosition = StrategicPositions.SB;
                    break;
                case 1:
                    StrategicPosition = StrategicPositions.BB;
                    break;
            }
        }

        #endregion
    }
}