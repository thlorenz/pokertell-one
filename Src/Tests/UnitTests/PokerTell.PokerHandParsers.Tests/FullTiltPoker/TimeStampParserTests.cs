using System;

namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class TimeStampParserTests : Tests.TimeStampParserTests
    {
        protected override TimeStampParser GetTimeStampParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.TimeStampParser();
        }

        protected override string ValidTimeStamp(DateTime timeStamp)
        {
            // 17:28:11 ET - 2009/10/29
            var dateString = timeStamp.ToString("yyyy/MM/dd");
            var timeString = timeStamp.ToString("HH:mm:ss");
            return string.Format("{0} ET - {1} ", timeString, dateString);
        }
    }
}