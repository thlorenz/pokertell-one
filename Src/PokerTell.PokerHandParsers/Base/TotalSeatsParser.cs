using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class TotalSeatsParser
    {
        public bool IsValid { get; protected set; }

        public int TotalSeats { get; protected set; }

        protected abstract string TotalSeatsPattern { get; }
       
        public virtual TotalSeatsParser Parse(string handHistory)
        {
            Match totalSeats = MatchTotalSeats(handHistory);
            IsValid = totalSeats.Success;

            if (IsValid)
            {
                ExtractTotalSeats(totalSeats);
            }

            return this;
        }

        protected virtual Match MatchTotalSeats(string handHistory)
        {
            return Regex.Match(handHistory, TotalSeatsPattern, RegexOptions.IgnoreCase);
        }

        protected virtual void ExtractTotalSeats(Match totalSeats)
        {
            TotalSeats = Convert.ToInt32(totalSeats.Groups["TotalSeats"].Value);
        }
    }
}