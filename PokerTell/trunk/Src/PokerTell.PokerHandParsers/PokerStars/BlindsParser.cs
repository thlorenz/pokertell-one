namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Text.RegularExpressions;

    public class BlindsParser : PokerHandParsers.BlindsParser
    {
        #region Constants and Fields

        const string BlindsPattern = @"\(\$*(?<SB>(\d+\.){0,1}\d+)/\$*(?<BB>(\d+\.){0,1}\d+)";

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
            BigBlind = Convert.ToDouble(blinds.Groups["BB"].Value);
            SmallBlind = Convert.ToDouble(blinds.Groups["SB"].Value);
        }

        #endregion
    }
}