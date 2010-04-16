namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class BlindsParser : IBlindsParser
    {
        public bool IsValid { get; protected set; }

        public double BigBlind { get; protected set; }

        public double SmallBlind { get; protected set; }

        protected abstract string BlindsPattern { get; }

        public virtual IBlindsParser Parse(string handHistory)
        {
            Match blinds = MatchBlinds(handHistory);
            IsValid = blinds.Success;

            if (IsValid)
            {
                ExtractBlinds(blinds);
            }

            return this;
        }

        protected virtual Match MatchBlinds(string handHistory)
        {
            return Regex.Match(handHistory, BlindsPattern, RegexOptions.IgnoreCase);
        }

        protected virtual void ExtractBlinds(Match blinds)
        {
            SmallBlind = Convert.ToDouble(blinds.Groups["Ratio"].Value.Replace(",", string.Empty));
            BigBlind = Convert.ToDouble(blinds.Groups["Ratio2"].Value.Replace(",", string.Empty));
        }
    }
}