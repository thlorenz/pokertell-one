namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class TimeStampParser : Base.TimeStampParser
    {
        #region Constants and Fields

        const string FullTiltDatePattern = @"(?<Year>\d{4})/(?<Month>\d{2})/(?<Day>\d{2})";

        const string FullTiltTimePattern = @"(?<Hour>\d{1,2}):(?<Minute>\d{2}):(?<Second>\d{2})";

        #endregion

        #region Properties

        protected override string DatePattern
        {
            get { return FullTiltDatePattern; }
        }

        protected override string TimePattern
        {
            get { return FullTiltTimePattern; }
        }

        #endregion
    }
}