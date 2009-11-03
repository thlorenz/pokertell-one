using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class BlindsParser
    {
        public bool IsValid { get; protected set; }

        public double BigBlind { get; protected set; }

        public double SmallBlind { get; protected set; }

        protected abstract string BlindsPattern { get; }

        #region Public Methods

        public virtual BlindsParser Parse(string handHistory)
        {
            Match blinds = MatchBlinds(handHistory);
            IsValid = blinds.Success;

            if (IsValid)
            {
                ExtractBlinds(blinds);
            }
            return this;
        }

        #endregion

        #region Methods

        protected virtual Match MatchBlinds(string handHistory)
        {
            return Regex.Match(handHistory, BlindsPattern, RegexOptions.IgnoreCase);
        }

        protected virtual void ExtractBlinds(Match blinds)
        {
            SmallBlind = Convert.ToDouble(blinds.Groups["Ratio"].Value.Replace(",", string.Empty));
            BigBlind = Convert.ToDouble(blinds.Groups["Ratio2"].Value.Replace(",", string.Empty));
        }

        #endregion
    }
}