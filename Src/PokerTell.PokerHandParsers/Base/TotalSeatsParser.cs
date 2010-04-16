namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class TotalSeatsParser : ITotalSeatsParser
    {
        public bool IsValid { get; protected set; }

        public int TotalSeats { get; protected set; }

        protected abstract string TotalSeatsPattern { get; }

        public virtual ITotalSeatsParser Parse(string handHistory)
        {
            Match totalSeats = MatchTotalSeats(handHistory);
            IsValid = totalSeats.Success;

            if (IsValid)
            {
                ExtractTotalSeats(totalSeats);
            }

            return this;
        }

        protected virtual void ExtractTotalSeats(Match totalSeats)
        {
            TotalSeats = Convert.ToInt32(totalSeats.Groups["TotalSeats"].Value);
        }

        protected virtual Match MatchTotalSeats(string handHistory)
        {
            return Regex.Match(handHistory, TotalSeatsPattern, RegexOptions.IgnoreCase);
        }
    }
}