namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class TotalPotParser : ITotalPotParser
    {
        public bool IsValid { get; protected set; }

        public double TotalPot { get; protected set; }

        protected abstract string TotalPotPattern { get; }

        public ITotalPotParser Parse(string handHistory)
        {
            Match totalPot = MatchTotalPot(handHistory);
            IsValid = totalPot.Success;

            if (IsValid)
            {
                ExtractTotalPot(totalPot);
            }

            return this;
        }

        void ExtractTotalPot(Match totalPot)
        {
            TotalPot = Convert.ToDouble(totalPot.Groups["Ratio"].Value.Replace(",", string.Empty));
        }

        Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, TotalPotPattern, RegexOptions.IgnoreCase);
        }
    }
}