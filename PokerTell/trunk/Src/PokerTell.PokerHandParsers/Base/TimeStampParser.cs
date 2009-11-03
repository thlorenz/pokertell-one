using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class TimeStampParser
    {
        #region Properties

        public bool IsValid { get; protected set; }

        public DateTime TimeStamp { get; protected set; }

        protected abstract string DatePattern { get; }

        protected abstract string TimePattern { get; }

        #endregion

        #region Public Methods

        public TimeStampParser Parse(string handHistory)
        {
            Match date = MatchDate(handHistory);
            Match time = MatchTime(handHistory);

            IsValid = date.Success && time.Success;

            if (IsValid)
            {
                ExtractTimeStamp(date, time);
            }

            return this;
        }

        #endregion

        #region Methods

        void ExtractTimeStamp(Match date, Match time)
        {
            TimeStamp = new DateTime(
                Convert.ToInt32(date.Groups["Year"].Value),
                Convert.ToInt32(date.Groups["Month"].Value),
                Convert.ToInt32(date.Groups["Day"].Value),
                Convert.ToInt32(time.Groups["Hour"].Value),
                Convert.ToInt32(time.Groups["Minute"].Value),
                Convert.ToInt32(time.Groups["Second"].Value));
        }

        Match MatchDate(string handHistory)
        {
            return Regex.Match(handHistory, DatePattern, RegexOptions.IgnoreCase);
        }

        Match MatchTime(string handHistory)
        {
            return Regex.Match(handHistory, TimePattern, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}