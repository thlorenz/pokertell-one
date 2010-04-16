namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class TimeStampParser : Base.TimeStampParser, IPokerStarsTimeStampParser
    {
        const string PokerStarsDatePattern = @"(?<Year>\d{4})/(?<Month>\d{2})/(?<Day>\d{2})";

        const string PokerStarsTimePattern = @"(?<Hour>\d{1,2}):(?<Minute>\d{2}):(?<Second>\d{2})";

        protected override string DatePattern
        {
            get { return PokerStarsDatePattern; }
        }

        protected override string TimePattern
        {
            get { return PokerStarsTimePattern; }
        }
    }
}