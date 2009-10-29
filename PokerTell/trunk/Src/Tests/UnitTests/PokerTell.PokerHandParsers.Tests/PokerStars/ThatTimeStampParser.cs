namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatTimeStampParser : Tests.ThatTimeStampParser
    {
        protected override TimeStampParser GetTimeStampParser()
        {
            return new PokerHandParsers.PokerStars.TimeStampParser();
        }

        protected override string ValidTimeStamp(DateTime timeStamp)
        {
            // 2009/09/08 15:43:42 ET
            var dateString = timeStamp.ToString("yyyy/MM/dd");
            var timeString = timeStamp.ToString("HH:mm:ss");
            return string.Format("{0} {1} ET", dateString, timeString);
        }
    }
}