namespace PokerTell.PokerHand
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Contains all information about a Poker Hand and methods to create and analyse it
    /// </summary>
    [Serializable]
    public abstract class PokerHand : IPokerHand
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [NonSerialized]
        IList<string> _allNames;

        double _ante;

        double _bb;

        string _board;

        ulong _gameId;

        double _sb;

        string _site;

        string _tableName;

        DateTime _timeStamp;

        int _totalPlayers;

        [NonSerialized]
        int _totalSeats;

        ulong _tournamentId;

        #endregion

        #region Constructors and Destructors

        protected PokerHand()
        {
            AllNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokerHand"/> class. 
        /// Creates Hand with given parameters
        /// </summary>
        /// <param name="site">
        /// <see cref="Site"></see>
        /// </param>
        /// <param name="gameid">
        /// <see cref="GameId"></see>
        /// </param>
        /// <param name="timeStamp">
        /// <see cref="TimeStamp"></see>
        /// </param>
        /// <param name="bb">
        /// <see cref="BB"></see>
        /// </param>
        /// <param name="sb">
        /// <see cref="SB"></see> 
        /// </param>
        /// <param name="totalplrs">
        /// <see cref="TotalPlayers"></see>
        /// </param>
        protected PokerHand(string site, ulong gameid, DateTime timeStamp, double bb, double sb, int totalplrs)
            : this()
        {
            InitializeBase(totalplrs, site, gameid, timeStamp, bb, sb);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Array containing the names of all players present at the table
        /// </summary>
        public IList<string> AllNames
        {
            get { return _allNames; }
            protected set { _allNames = value; }
        }

        public double Ante
        {
            get { return _ante; }
            set { _ante = value; }
        }

        /// <summary>
        /// The Big Blind
        /// </summary>
        public double BB
        {
            get { return _bb; }
            protected set { _bb = value; }
        }

        /// <summary>
        /// Contains all cards on the board (As Kh Qd
        /// </summary>
        public string Board
        {
            get { return _board; }

            set
            {
                // Trunc board if it is too long max is "SS SS SS SS SS" = 4*3+2 
                // (problem caused by error in handhistory)
                if (value.Length > 14)
                {
                    value = value.Substring(0, 14);
                }

                _board = value;
            }
        }

        /// <summary>
        /// String representation of date obtained from TimeStamp
        /// </summary>
        public string DateAsString
        {
            get { return TimeStamp.ToString("yyyy/MM/dd"); }
        }

        /// <summary>
        /// The ID of the hand set by the Site and given in the Hand History
        /// </summary>
        public ulong GameId
        {
            get { return _gameId; }
            protected set { _gameId = value; }
        }

        /// <summary>
        /// The small Blind
        /// </summary>
        public double SB
        {
            get { return _sb; }
            set { _sb = value; }
        }

        /// <summary>
        /// Name of the PokerSite the hand occurred on
        /// </summary>
        public string Site
        {
            get { return _site; }
            protected set { _site = value; }
        }

        /// <summary>
        /// The name of the table the hand occurred at - given in Hand History
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// String representation of time obtained from TimeStamp
        /// </summary>
        public string TimeAsString
        {
            get { return TimeStamp.ToString("HH:mm:ss"); }
        }

        /// <summary>
        /// Date and Time when the hand occured
        /// </summary>
        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            protected set { _timeStamp = value; }
        }

        /// <summary>
        /// Total Players that acted in the hand
        /// </summary>
        public int TotalPlayers
        {
            get { return _totalPlayers; }
            set { _totalPlayers = value; }
        }

        /// <summary>
        /// Total number seats at the table
        /// </summary>
        public int TotalSeats
        {
            get { return _totalSeats; }
            set { _totalSeats = value; }
        }

        /// <summary>
        /// The ID of the torunament set by the Site and given in the Hand History
        /// </summary>
        public ulong TournamentId
        {
            get { return _tournamentId; }
            set { _tournamentId = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares given PokerHand with this one using the Game#
        /// </summary>
        /// <param name="obj">Second Poker Hand</param>
        /// <returns>true if equal</returns>
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
            return Equals((PokerHand)obj);
        }

        public bool Equals(PokerHand other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other._ante == _ante && other._bb == _bb && Equals(other._board, _board) && other._gameId == _gameId &&
                   other._sb == _sb && Equals(other._site, _site) && Equals(other._tableName, _tableName) &&
                   other._timeStamp.Equals(_timeStamp) && other._totalPlayers == _totalPlayers &&
                   other._totalSeats == _totalSeats && other._tournamentId == _tournamentId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _ante.GetHashCode();
                result = (result * 397) ^ _bb.GetHashCode();
                result = (result * 397) ^ (_board != null ? _board.GetHashCode() : 0);
                result = (result * 397) ^ _gameId.GetHashCode();
                result = (result * 397) ^ _sb.GetHashCode();
                result = (result * 397) ^ (_site != null ? _site.GetHashCode() : 0);
                result = (result * 397) ^ (_tableName != null ? _tableName.GetHashCode() : 0);
                result = (result * 397) ^ _timeStamp.GetHashCode();
                result = (result * 397) ^ _totalPlayers;
                result = (result * 397) ^ _totalSeats;
                result = (result * 397) ^ _tournamentId.GetHashCode();
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        int IComparable.CompareTo(object obj)
        {
            if (TimeStamp < ((IPokerHand)obj).TimeStamp)
            {
                return -1;
            }
            else if (TimeStamp > ((IPokerHand)obj).TimeStamp)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region IPokerHand

        public void InitializeBase(int totalplrs, string site, ulong gameid, DateTime timeStamp, double bb, double sb)
        {
            try
            {
                if (totalplrs < -1 || totalplrs > 10)
                {
                    throw new ArgumentOutOfRangeException(
                        "totalplrs", totalplrs, "Value must be between -1 (unknown) and " + 10);
                }

                if (site == null)
                {
                    throw new ArgumentNullException("site");
                }

                Site = site;
                GameId = gameid;
                TimeStamp = timeStamp;

                BB = bb;
                SB = sb;
                TotalPlayers = totalplrs;
                Board = string.Empty;
                TournamentId = 0;
                TableName = string.Empty;
            }
            catch (ArgumentNullException excep)
            {
                Log.Error("Rethrown", excep);
                throw;
            }
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Ante: {0}, BB: {1}, Board: {2}, GameId: {3}, SB: {4}, Site: {5}, TableName: {6}, TimeStamp: {7}, TotalPlayers: {8}, TotalSeats: {9}, TournamentId: {10}", _ante, _bb, _board, _gameId, _sb, _site, _tableName, _timeStamp, _totalPlayers, _totalSeats, _tournamentId);
        }

        #endregion
    }
}