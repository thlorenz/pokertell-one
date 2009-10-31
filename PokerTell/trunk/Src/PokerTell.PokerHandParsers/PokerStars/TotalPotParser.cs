namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class TotalPotParser : PokerHandParsers.TotalPotParser
    {
        const string TotalPotPattern = @"Total pot " + SharedPatterns.RatioPattern;

        public override void Parse(string handHistory)
        {
            Match totalPot = MatchTotalPot(handHistory);
            IsValid = totalPot.Success;

            if (IsValid)
            {
                ExtractTotalPot(totalPot);
            }
        }

        static Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, TotalPotPattern, RegexOptions.IgnoreCase);
        }

        void ExtractTotalPot(Match totalPot)
        {
            TotalPot = Convert.ToDouble(totalPot.Groups["Ratio"].Value);
        }
    }
}