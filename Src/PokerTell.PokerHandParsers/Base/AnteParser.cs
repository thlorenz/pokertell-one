namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class AnteParser : IAnteParser
    {
        public double Ante { get; protected set; }

        public bool IsValid { get; protected set; }

        protected abstract string AntePattern { get; }

        public virtual IAnteParser Parse(string handHistory)
        {
            Match ante = MatchTotalPot(handHistory);
            IsValid = ante.Success;

            if (IsValid)
            {
                ExtractTotalPot(ante);
            }

            return this;
        }

        protected virtual void ExtractTotalPot(Match ante)
        {
            Ante = Convert.ToDouble(ante.Groups["Ratio"].Value.Replace(",", string.Empty));
        }

        protected virtual Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, AntePattern, RegexOptions.IgnoreCase);
        }
    }
}