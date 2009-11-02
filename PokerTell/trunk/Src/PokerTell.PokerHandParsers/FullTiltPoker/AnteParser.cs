using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class AnteParser : PokerHandParsers.AnteParser
    {
        const string AntePattern = @" Ante " + SharedPatterns.RatioPattern + " - ";

        public override PokerHandParsers.AnteParser Parse(string handHistory)
        {
            Match ante = MatchTotalPot(handHistory);
            IsValid = ante.Success;

            if (IsValid)
            {
                ExtractTotalPot(ante);
            }
            return this;            
        }

        static Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, AntePattern, RegexOptions.IgnoreCase);
        }

        void ExtractTotalPot(Match ante)
        {
            Ante = Convert.ToDouble(ante.Groups["Ratio"].Value.Replace(",",string.Empty));
        }
    }
}