namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System.Text.RegularExpressions;

    public class TotalSeatsParser : Base.TotalSeatsParser
    {
        #region Constants and Fields

        const string FullTiltTotalSeatsPattern =
            TableNameParser.FullTiltTableNamePattern + @"\((?<TotalSeats>[0-9]{1,2}) max\) ";

        const string HeadsUpPattern = TableNameParser.FullTiltTableNamePattern + @"\(heads up\) ";

        #endregion

        #region Properties

        protected override string TotalSeatsPattern
        {
            get { return FullTiltTotalSeatsPattern; }
        }

        #endregion

        #region Public Methods

        public override Base.TotalSeatsParser Parse(string handHistory)
        {
            IsValid = !string.IsNullOrEmpty(handHistory);

            Match totalSeats = MatchTotalSeats(handHistory);
            if (totalSeats.Success)
            {
                ExtractTotalSeats(totalSeats);
                return this;
            }

            Match headsUp = MatchHeadsUp(handHistory);
            if (headsUp.Success)
            {
                TotalSeats = 2;
                return this;
            }

            DefaultToStandard();
            return this;
        }

        #endregion

        #region Methods

        static Match MatchHeadsUp(string handHistory)
        {
            return Regex.Match(handHistory, HeadsUpPattern, RegexOptions.IgnoreCase);
        }

        void DefaultToStandard()
        {
            TotalSeats = 9;
        }

        #endregion
    }
}