using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class AnteParser
    {
        #region Properties

        public double Ante { get; protected set; }

        public bool IsValid { get; protected set; }

        protected abstract string AntePattern { get; }

        #endregion

        #region Public Methods

        public virtual AnteParser Parse(string handHistory)
        {
            Match ante = MatchTotalPot(handHistory);
            IsValid = ante.Success;

            if (IsValid)
            {
                ExtractTotalPot(ante);
            }
            return this;
        }

        #endregion

        #region Methods

        protected virtual void ExtractTotalPot(Match ante)
        {
            Ante = Convert.ToDouble(ante.Groups["Ratio"].Value.Replace(",", string.Empty));
        }

        protected virtual Match MatchTotalPot(string handHistory)
        {
            return Regex.Match(handHistory, AntePattern, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}