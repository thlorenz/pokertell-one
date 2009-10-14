namespace PokerTell.PokerHand
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Infrastructure.Interfaces.PokerHand;

    using log4net;

    /// <summary>
    /// Contains all information about a Poker Hand and methods to create and analyse it
    /// </summary>
    public abstract class PokerHand : IPokerHand
    {
        #region Constants and Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private string _board;

        #endregion

        #region Constructors and Destructors

        public PokerHand()
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

        #region Properties

        /// <summary>
        /// Array containing the names of all players present at the table
        /// </summary>
        public IList<string> AllNames { get; protected set; }

        public double Ante { get; set; }

        /// <summary>
        /// The Big Blind
        /// </summary>
        public double BB { get; protected set; }

        /// <summary>
        /// Contains all cards on the board (As Kh Qd
        /// </summary>
        public string Board
        {
            get { return _board; }

            set
            {
                // Trunc board if it is too long max is "SS SS SS SS SS" = 4*3+2 (problem caused by error in handhistory)
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
        public ulong GameId { get; protected set; }

        /// <summary>
        /// The small Blind
        /// </summary>
        public double SB { get; set; }

        /// <summary>
        /// Name of the PokerSite the hand occurred on
        /// </summary>
        public string Site { get; protected set; }

        /// <summary>
        /// The name of the table the hand occurred at - given in Hand History
        /// </summary>
        public string TableName { get; set; }

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
        public DateTime TimeStamp { get; protected set; }

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Compares given PokerHand with this one using the Game#
        /// </summary>
        /// <param name="obj">Second Poker Hand</param>
        /// <returns>true if equal</returns>
        public override bool Equals(object obj)
        {
            return obj != null && GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return GameId.GetHashCode() ^ Site.GetHashCode();
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

        #endregion
    }
}