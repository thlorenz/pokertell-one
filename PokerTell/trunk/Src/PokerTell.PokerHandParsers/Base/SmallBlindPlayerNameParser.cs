using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class SmallBlindPlayerNameParser
    {
        #region Properties

        public bool IsValid { get; protected set; }

        public string SmallBlindPlayerName { get; protected set; }

        protected abstract string SmallBlindPattern { get; }

        #endregion

        #region Public Methods

        public virtual SmallBlindPlayerNameParser Parse(string handHistory)
        {
            Match smallBlindPlayerName = MatchSmallBlindPlayerName(handHistory);

            IsValid = smallBlindPlayerName.Success;

            if (IsValid)
            {
                ExtractSmallBlindPlayerName(smallBlindPlayerName);
            }

            return this;
        }

        #endregion

        #region Methods

        void ExtractSmallBlindPlayerName(Match smallBlindPlayerName)
        {
            SmallBlindPlayerName = smallBlindPlayerName.Groups["PlayerName"].Value;
        }

        Match MatchSmallBlindPlayerName(string handHistory)
        {
            return Regex.Match(handHistory, SmallBlindPattern, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}