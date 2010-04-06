namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Base;

    using log4net;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools;
    using Tools.Extensions;

    [Serializable]
    public class ConvertedPokerPlayer : PokerPlayer, IConvertedPokerPlayer
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NonSerialized]
        ActionSequences[] _actionSequences = new ActionSequences[(int)Streets.River + 1];

        [NonSerialized]
        int[] _betSizeIndexes = new int[(int)Streets.River + 1];

        [NonSerialized]
        long _id;

        [NonSerialized]
        bool?[] _inPosition = new bool?[(int)Streets.River + 1];

        int _mAfter;

        int _mBefore;

        [NonSerialized]
        IConvertedPokerHand _parentHand;

        [NonSerialized]
        IPlayerIdentity _playerIdentity;

        [NonSerialized]
        PokerHandStringConverter _pokerHandStringConverter;

        [NonSerialized]
        int _preflopRaiseInFrontPos;

        IList<IConvertedPokerRound> _rounds = new List<IConvertedPokerRound>();

        [NonSerialized]
        SequenceStringConverter _sequenceStringConverter;

        [NonSerialized]
        string[] _sequenceStrings = new string[(int)Streets.River + 1];

        StrategicPositions _strategicPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerPlayer"/> class.
        /// </summary>
        public ConvertedPokerPlayer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertedPokerPlayer"/> class.
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

        public ActionSequences[] ActionSequences
        {
            get { return _actionSequences; }
            private set { _actionSequences = value; }
        }

        public string ActionsFlop
        {
            get { return GetActionsIfRoundExistsFor(Streets.Flop); }

            private set { ValidateActionsAddRoundsIfNecessaryAndAssignRoundConvertedFrom(value, Streets.Flop); }
        }

        public string ActionsPreFlop
        {
            get { return GetActionsIfRoundExistsFor(Streets.PreFlop); }

            private set { ValidateActionsAddRoundsIfNecessaryAndAssignRoundConvertedFrom(value, Streets.PreFlop); }
        }

        public string ActionsRiver
        {
            get { return GetActionsIfRoundExistsFor(Streets.River); }

            private set { ValidateActionsAddRoundsIfNecessaryAndAssignRoundConvertedFrom(value, Streets.River); }
        }

        public string ActionsTurn
        {
            get { return GetActionsIfRoundExistsFor(Streets.Turn); }

            private set { ValidateActionsAddRoundsIfNecessaryAndAssignRoundConvertedFrom(value, Streets.Turn); }
        }

        public int[] BetSizeIndexes
        {
            get { return _betSizeIndexes; }
            private set { _betSizeIndexes = value; }
        }

        public long Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        /// <summary>
        /// Is player in position on Flop, Turn or River?
        /// </summary>
        public bool?[] InPosition
        {
            get { return _inPosition; }
            protected set { _inPosition = value; }
        }

        public bool? InPositionFlop
        {
            get { return InPosition[(int)Streets.Flop]; }
            private set { InPosition[(int)Streets.Flop] = value; }
        }

        public bool? InPositionPreFlop
        {
            get { return InPosition[(int)Streets.PreFlop]; }
            private set { InPosition[(int)Streets.PreFlop] = value; }
        }

        public bool? InPositionRiver
        {
            get { return InPosition[(int)Streets.River]; }
            private set { InPosition[(int)Streets.River] = value; }
        }

        public bool? InPositionTurn
        {
            get { return InPosition[(int)Streets.Turn]; }
            private set { InPosition[(int)Streets.Turn] = value; }
        }

        /// <summary>
        /// M of player after the hand is over
        /// </summary>
        public int MAfter
        {
            get { return _mAfter; }
            set { _mAfter = value; }
        }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        public int MBefore
        {
            get { return _mBefore; }
            set { _mBefore = value; }
        }

        public IConvertedPokerHand ParentHand
        {
            get { return _parentHand; }
            set { _parentHand = value; }
        }

        public IPlayerIdentity PlayerIdentity
        {
            get { return _playerIdentity; }
            set
            {
                _playerIdentity = value;
                Name = value.Name;
            }
        }

        /// <summary>
        /// Position, that a preflop raise came from, before player could act
        /// </summary>
        public int PreflopRaiseInFrontPos
        {
            get { return _preflopRaiseInFrontPos; }
            protected set { _preflopRaiseInFrontPos = value; }
        }

        /// <summary>
        /// List of all Poker Rounds for current hand Preflop Flop
        /// </summary>
        public IList<IConvertedPokerRound> Rounds
        {
            get { return _rounds; }
            set { _rounds = value; }
        }

        public ActionSequences SequenceFlop
        {
            get { return ActionSequences[(int)Streets.Flop]; }
            private set { ActionSequences[(int)Streets.Flop] = value; }
        }

        public ActionSequences SequencePreFlop
        {
            get { return ActionSequences[(int)Streets.PreFlop]; }
            private set { ActionSequences[(int)Streets.PreFlop] = value; }
        }

        public ActionSequences SequenceRiver
        {
            get { return ActionSequences[(int)Streets.River]; }
            private set { ActionSequences[(int)Streets.River] = value; }
        }

        public int BetSizeIndexFlop
        {
            get { return BetSizeIndexes[(int)Streets.Flop]; }
            set { BetSizeIndexes[(int)Streets.Flop] = value; }
        }

        public int BetSizeIndexTurn
        {
            get { return BetSizeIndexes[(int)Streets.Turn]; }
            set { BetSizeIndexes[(int)Streets.Turn] = value; }
        }

        public int BetSizeIndexRiver
        {
            get { return BetSizeIndexes[(int)Streets.River]; }
            set { BetSizeIndexes[(int)Streets.River] = value; }
        }

        /// <summary>
        /// Contains Sequence strings for each Round of the Player
        /// representing the way he acted or reacted to opponents actions
        /// Sometimes referred to as the 'line' and is used to determine bettin patterns
        /// during statistical analysis
        /// </summary>
        public string[] SequenceStrings
        {
            get { return _sequenceStrings; }
            protected set { _sequenceStrings = value; }
        }

        public ActionSequences SequenceTurn
        {
            get { return ActionSequences[(int)Streets.Turn]; }
            private set { ActionSequences[(int)Streets.Turn] = value; }
        }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        public StrategicPositions StrategicPosition
        {
            get { return _strategicPosition; }
            protected set { _strategicPosition = value; }
        }

        PokerHandStringConverter PokerHandStringConverter
        {
            get { return _pokerHandStringConverter ?? (_pokerHandStringConverter = new PokerHandStringConverter()); }
        }

        SequenceStringConverter SequenceStringConverter
        {
            get { return _sequenceStringConverter ?? (_sequenceStringConverter = new SequenceStringConverter()); }
        }

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public IConvertedPokerRound this[int index]
        {
            get { return Rounds[index]; }
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

        public bool Equals(ConvertedPokerPlayer other)
        {
            return base.Equals(other)
                   && Rounds.ToArray().EqualsArray(other.Rounds.ToArray());
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

            return Equals(obj as ConvertedPokerPlayer);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(IConvertedPokerPlayer other)
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

        /// <summary>
        /// Add a new Poker Round to the player
        /// </summary>
        public IConvertedPokerPlayer Add()
        {
            Add(new ConvertedPokerRound());

            return this;
        }

        /// <summary>
        /// Add a given Poker round to the Player
        /// </summary>
        /// <param name="convertedRound">
        /// The converted Round.
        /// </param>
        public IConvertedPokerPlayer Add(IConvertedPokerRound convertedRound)
        {
            try
            {
                if (convertedRound == null)
                {
                    throw new ArgumentNullException("convertedRound");
                }

                if (Rounds.Count < 4)
                {
                    Rounds.Add(convertedRound);
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

        public IConvertedPokerPlayer InitializeWith(string name, double mBefore, double mAfter, int positionNum, int totalPlayers, string holecards)
        {
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
                InPosition = new bool?[4];
                PreflopRaiseInFrontPos = -1;
            }
            catch (Exception excep)
            {
                Log.Error("Unhandled", excep);
            }

            return this;
        }

        public IConvertedPokerPlayer SetActionSequencesAndBetSizeKeysFromSequenceStrings()
        {
            for (int i = 0; i < SequenceStrings.Length; i++)
            {
                SequenceStringConverter.Convert(SequenceStrings[i]);
                ActionSequences[i] = SequenceStringConverter.ActionSequence;
                BetSizeIndexes[i] = SequenceStringConverter.BetSizeIndex;
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
        public void SetActionSequenceString(
            ref string currentSequence, IConvertedPokerAction myNextAction, Streets street)
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
                if (string.IsNullOrEmpty(SequenceStrings[(int)street]))
                {
                    // CurrentSequence will be empty for unraised pot and contain R for each raise/reraise in a raised pot
                    SequenceStrings[(int)street] = currentSequence + myNextAction.What;

                    // It is now a raised pot for all players to come
                    if (myNextAction.What == ActionTypes.R)
                    {
                        currentSequence = currentSequence + myNextAction.What;
                    }
                }

                return;
            }

            if (SequenceStrings[(int)street] == null)
            {
                SequenceStrings[(int)street] = string.Empty;
            }

            // PostFlop, once we have a bet or reaction to bet (represented by number) we don't need to know more
            const string patContainsNumber = "[0-9]";
            if (Regex.IsMatch(SequenceStrings[(int)street], patContainsNumber))
            {
                return;
            }

            switch (myNextAction.What)
            {
                case ActionTypes.F:
                case ActionTypes.C:
                case ActionTypes.X:
                    SequenceStrings[(int)street] = SequenceStrings[(int)street] + currentSequence + myNextAction.What;
                    break;
                case ActionTypes.B:
                    var normalizedRatio =
                        (int)
                        (10.0 * Normalizer.NormalizeToKeyValues(ApplicationProperties.BetSizeKeys, myNextAction.Ratio));

                    SequenceStrings[(int)street] = SequenceStrings[(int)street] + currentSequence + normalizedRatio;

                    currentSequence = currentSequence + normalizedRatio;
                    break;
                case ActionTypes.R:
                    SequenceStrings[(int)street] = SequenceStrings[(int)street] + currentSequence + myNextAction.What;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rounds.GetEnumerator();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerator<IConvertedPokerRound> GetEnumerator()
        {
            return Rounds.GetEnumerator();
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

        string BettingRoundsToString()
        {
            string betting = string.Empty;
            try
            {
                // Iterate through rounds pre-flop to river
                foreach (IConvertedPokerRound round in this)
                {
                    betting += "| " + round;
                }
            }
            catch (ArgumentNullException excep)
            {
                excep.Data.Add("betRoundCount = ", Rounds.Count);
                Log.Error("Returning betting Rounds I go so far", excep);
            }

            return betting;
        }

        string GetActionsIfRoundExistsFor(Streets street)
        {
            return Rounds.Count > (int)street
                       ? PokerHandStringConverter.BuildSqlStringFrom(Rounds[(int)street])
                       : null;
        }

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
                    break;
                case 0:
                    StrategicPosition = StrategicPositions.BU;
                    break;
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

        void ValidateActionsAddRoundsIfNecessaryAndAssignRoundConvertedFrom(string actions, Streets street)
        {
            if (string.IsNullOrEmpty(actions))
            {
                return;
            }

            while (Rounds.Count < (int)street + 1)
            {
                Add();
            }

            Rounds[(int)street] = PokerHandStringConverter.ConvertedRoundFrom(actions);
        }
    }
}