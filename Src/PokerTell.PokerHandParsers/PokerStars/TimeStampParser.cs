namespace PokerTell.PokerHandParsers.PokerStars
{
    public class TimeStampParser : Base.TimeStampParser
    {
        #region Constants and Fields

        const string PokerStarsDatePattern = @"(?<Year>\d{4})/(?<Month>\d{2})/(?<Day>\d{2})";

        const string PokerStarsTimePattern = @"(?<Hour>\d{1,2}):(?<Minute>\d{2}):(?<Second>\d{2})";

        #endregion

        #region Properties

        protected override string DatePattern
        {
            get { return PokerStarsDatePattern; }
        }

        protected override string TimePattern
        {
            get { return PokerStarsTimePattern; }
        }

        #endregion
    }
}