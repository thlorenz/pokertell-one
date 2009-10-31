namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class AnteParser : PokerHandParsers.AnteParser
    {
        const string AntePattern = @"posts the ante " + SharedPatterns.RatioPattern;

        public override void Parse(string handHistory)
        {
            Match ante = MatchTotalPot(handHistory);
            IsValid = ante.Success;

            if (IsValid)
            {
                ExtractTotalPot(ante);
            }
        }

        static Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, AntePattern, RegexOptions.IgnoreCase);
        }

        void ExtractTotalPot(Match ante)
        {
            Ante = Convert.ToDouble(ante.Groups["Ratio"].Value);
        }
    }
}