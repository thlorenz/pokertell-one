namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class TimeStampParser : PokerHandParsers.TimeStampParser
    {
        #region Constants and Fields

        const string DatePattern = @"(?<Year>\d{4})/(?<Month>\d{2})/(?<Day>\d{2})";

        const string TimePattern = @"(?<Hour>\d{1,2}):(?<Minute>\d{2}):(?<Second>\d{2})";

        #endregion

        #region Public Methods

        public override PokerHandParsers.TimeStampParser Parse(string handHistory)
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

        static Match MatchDate(string handHistory)
        {
            return Regex.Match(handHistory, DatePattern, RegexOptions.IgnoreCase);
        }

        static Match MatchTime(string handHistory)
        {
            return Regex.Match(handHistory, TimePattern, RegexOptions.IgnoreCase);
        }

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

        #endregion
    }
}