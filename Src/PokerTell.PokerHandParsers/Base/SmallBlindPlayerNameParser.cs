namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class SmallBlindPlayerNameParser : ISmallBlindPlayerNameParser
    {
        public bool IsValid { get; protected set; }

        public string SmallBlindPlayerName { get; protected set; }

        protected abstract string SmallBlindPattern { get; }

        public virtual ISmallBlindPlayerNameParser Parse(string handHistory)
        {
            Match smallBlindPlayerName = MatchSmallBlindPlayerName(handHistory);

            IsValid = smallBlindPlayerName.Success;

            if (IsValid)
            {
                ExtractSmallBlindPlayerName(smallBlindPlayerName);
            }

            return this;
        }

        void ExtractSmallBlindPlayerName(Match smallBlindPlayerName)
        {
            SmallBlindPlayerName = smallBlindPlayerName.Groups["PlayerName"].Value;
        }

        Match MatchSmallBlindPlayerName(string handHistory)
        {
            return Regex.Match(handHistory, SmallBlindPattern, RegexOptions.IgnoreCase);
        }
    }
}