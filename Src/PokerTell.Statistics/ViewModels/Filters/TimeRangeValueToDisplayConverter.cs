namespace PokerTell.Statistics.ViewModels.Filters
{
    using System;

    public class TimeRangeValueToDisplayConverter
    {
        public readonly Func<int, string> Convert = value =>
        {
            var minutes = Math.Abs(value);
            if (minutes == 0)
            {
                return "Now";
            }

            return minutes < 120
                       ? minutes + " minutes ago"
                       : (minutes / 60) + " hours ago";
        };
    }
}