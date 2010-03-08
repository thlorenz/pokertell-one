using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class TotalPotParser
    {
        public bool IsValid { get; protected set; }

        public double TotalPot { get; protected set; }

        protected abstract string TotalPotPattern { get; }

        public TotalPotParser Parse(string handHistory)
        {
            Match totalPot = MatchTotalPot(handHistory);
            IsValid = totalPot.Success;

            if (IsValid)
            {
                ExtractTotalPot(totalPot);
            }

            return this;
        }

        Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, TotalPotPattern, RegexOptions.IgnoreCase);
        }

        void ExtractTotalPot(Match totalPot)
        {
            TotalPot = Convert.ToDouble(totalPot.Groups["Ratio"].Value.Replace(",", string.Empty));
        }
    }
}