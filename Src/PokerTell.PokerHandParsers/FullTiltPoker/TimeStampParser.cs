namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class TimeStampParser : Base.TimeStampParser, IFullTiltPokerTimeStampParser
    {
        const string FullTiltDatePattern = @"(?<Year>\d{4})/(?<Month>\d{2})/(?<Day>\d{2})";

        const string FullTiltTimePattern = @"(?<Hour>\d{1,2}):(?<Minute>\d{2}):(?<Second>\d{2})";

        protected override string DatePattern
        {
            get { return FullTiltDatePattern; }
        }

        protected override string TimePattern
        {
            get { return FullTiltTimePattern; }
        }
    }
}