namespace PokerTell.PokerHand.Analyzation
{
    using System;
    using System.Linq;
    using System.Text;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Extensions;

    public class AnalyzablePokerPlayer : IAnalyzablePokerPlayer
    {
        #region Constants and Fields

        protected IConvertedPokerRound[] _sequences;

        readonly string[] _sequenceStrings;

        PokerHandStringConverter _pokerHandStringConverter;

        #endregion

        #region Constructors and Destructors

        public AnalyzablePokerPlayer(
            long id, 
            int handId, 
            string holecards, 
            int mBefore, 
            int position, 
            StrategicPositions strategicPosition, 
            bool? inPositionPreFlop, 
            bool? inPositionFlop, 
            bool? inPositionTurn, 
            bool? inPositionRiver, 
            ActionSequences actionSequencePreFlop, 
            ActionSequences actionSequenceFlop, 
            ActionSequences actionSequenceTurn, 
            ActionSequences actionSequenceRiver, 
            int betSizeIndexFlop, 
            int betSizeIndexTurn, 
            int betSizeIndexRiver, 
            double bb, 
            double ante, 
            DateTime timeStamp, 
            int totalPlayers, 
            string sequencePreFlop, 
            string sequenceFlop, 
            string sequenceTurn, 
            string sequenceRiver)
            : this()
        {
            Id = id;
            HandId = handId;
            Holecards = holecards;
            MBefore = mBefore;
            Position = position;
            StrategicPosition = strategicPosition;

            InPosition[(int)Streets.PreFlop] = inPositionPreFlop;
            InPosition[(int)Streets.Flop] = inPositionFlop;
            InPosition[(int)Streets.Turn] = inPositionTurn;
            InPosition[(int)Streets.River] = inPositionRiver;

            ActionSequences[(int)Streets.PreFlop] = actionSequencePreFlop;
            ActionSequences[(int)Streets.Flop] = actionSequenceFlop;
            ActionSequences[(int)Streets.Turn] = actionSequenceTurn;
            ActionSequences[(int)Streets.River] = actionSequenceRiver;

            BetSizeIndexes[(int)Streets.Flop] = betSizeIndexFlop;
            BetSizeIndexes[(int)Streets.Turn] = betSizeIndexTurn;
            BetSizeIndexes[(int)Streets.River] = betSizeIndexRiver;

            BB = bb;
            Ante = ante;
            TimeStamp = timeStamp;
            TotalPlayers = totalPlayers;

            _sequenceStrings[(int)Streets.PreFlop] = sequencePreFlop;
            _sequenceStrings[(int)Streets.Flop] = sequenceFlop;
            _sequenceStrings[(int)Streets.Turn] = sequenceTurn;
            _sequenceStrings[(int)Streets.River] = sequenceRiver;
        }

        public AnalyzablePokerPlayer()
        {
            InPosition = new bool?[(int)(Streets.River + 1)];
            ActionSequences = new ActionSequences[(int)(Streets.River + 1)];
            BetSizeIndexes = new int[(int)(Streets.River + 1)];
            _sequenceStrings = new string[(int)(Streets.River + 1)];
        }

        #endregion

        #region Properties

        public ActionSequences[] ActionSequences { get; set; }

        public double Ante { get; set; }

        /// <summary>
        /// The Big Blind
        /// </summary>
        public double BB { get; set; }

        public int[] BetSizeIndexes { get; set; }

        public int HandId { get; set; }

        public string Holecards { get; set; }

        public long Id { get; set; }

        /// <summary>
        /// Is player in position on Flop, Turn or River?
        /// </summary>
        public bool?[] InPosition { get; set; }

        /// <summary>
        /// M of player at the start of the hand
        /// </summary>
        public int MBefore { get; set; }

        /// <summary>
        /// Position: SB=0, BB=1, Button=totalplrs
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// List of all PokerRound Sequences for current hand Preflop Flop
        /// </summary>
        public IConvertedPokerRound[] Sequences
        {
            get
            {
                if (_sequences == null)
                {
                    InitializeSequences();
                }

                return _sequences;
            }
        }

        /// <summary>
        /// Position of Player in relation to Button (SB - BU)
        /// </summary>
        public StrategicPositions StrategicPosition { get; set; }

        /// <summary>
        /// Date and Time when the hand occured
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Total Players that acted in the hand
        /// </summary>
        public int TotalPlayers { get; set; }

        PokerHandStringConverter PokerHandStringConverter
        {
            get { return _pokerHandStringConverter ?? (_pokerHandStringConverter = new PokerHandStringConverter()); }
        }

        #endregion

        #region Public Methods

        public bool Equals(AnalyzablePokerPlayer other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.Id == Id;
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

            return Equals((AnalyzablePokerPlayer)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            var sb =
                new StringBuilder(
                    string.Format(
                        "Id: {0}, BB: {1}, Ante: {2}, TimeStamp: {3}, TotalPlayers: {4}, HandId: {5}, MBefore: {6}, Position: {7}, StrategicPosition: {8}, Holecards: {9}", 
                        Id, 
                        BB, 
                        Ante, 
                        TimeStamp, 
                        TotalPlayers, 
                        HandId, 
                        MBefore, 
                        Position, 
                        StrategicPosition, 
                        Holecards));

            sb.AppendLine("\nInPosition");
            InPosition.ToList().ForEach(sp => sb.Append(sp.ToString() + " "));
            sb.AppendLine("\nActionSequences");
            ActionSequences.ToList().ForEach(seq => sb.Append(seq.ToString() + " "));
            sb.AppendLine("\nBetSizeIndexes");
            BetSizeIndexes.ToList().ForEach(bs => sb.Append(bs.ToString() + " "));
            sb.AppendLine("\nSequences");
            Sequences.ToList().ForEach(seq => sb.Append(seq.ToStringNullSafe() + "\n"));

            return sb.ToString();
        }

        #endregion

        #region Methods

        void InitializeSequences()
        {
            _sequences = new IConvertedPokerRound[(int)Streets.River + 1];
            _sequences[(int)Streets.PreFlop] =
                PokerHandStringConverter.ConvertedRoundFrom(_sequenceStrings[(int)Streets.PreFlop]);
            _sequences[(int)Streets.Flop] =
                PokerHandStringConverter.ConvertedRoundFrom(_sequenceStrings[(int)Streets.Flop]);
            _sequences[(int)Streets.Turn] =
                PokerHandStringConverter.ConvertedRoundFrom(_sequenceStrings[(int)Streets.Turn]);
            _sequences[(int)Streets.River] =
                PokerHandStringConverter.ConvertedRoundFrom(_sequenceStrings[(int)Streets.River]);
        }

        #endregion
    }
}