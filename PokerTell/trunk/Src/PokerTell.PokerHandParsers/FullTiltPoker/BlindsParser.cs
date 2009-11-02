using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class BlindsParser : PokerHandParsers.BlindsParser
    {
        #region Constants and Fields

        const string BlindsPattern = @" - " + SharedPatterns.RatioPattern + "/" + SharedPatterns.Ratio2Pattern + " ";

        #endregion

        #region Public Methods

        public override PokerHandParsers.BlindsParser Parse(string handHistory)
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

        static Match MatchBlinds(string handHistory)
        {
            return Regex.Match(handHistory, BlindsPattern, RegexOptions.IgnoreCase);
        }

        void ExtractBlinds(Match blinds)
        {
            SmallBlind = Convert.ToDouble(blinds.Groups["Ratio"].Value.Replace(",",string.Empty));
            BigBlind = Convert.ToDouble(blinds.Groups["Ratio2"].Value.Replace(",",string.Empty));
        }

        #endregion
    }
}