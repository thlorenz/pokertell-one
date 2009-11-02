using System;
using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class SmallBlindPlayerNameParser : PokerHandParsers.SmallBlindPlayerNameParser
    {
        const string SmallBlindPattern = @"(?<PlayerName>.+) posts the small blind";

        public override PokerHandParsers.SmallBlindPlayerNameParser Parse(string handHistory)
        {
            Match smallBlindPlayerName = MatchSmallBlindPlayerName(handHistory);

            IsValid = smallBlindPlayerName.Success;

            if (IsValid)
            {
                ExtractSmallBlindPlayerName(smallBlindPlayerName);
            }

            return this;
        }

        static Match MatchSmallBlindPlayerName(string handHistory)
        {
            return Regex.Match(handHistory, SmallBlindPattern, RegexOptions.IgnoreCase);
        }

        void ExtractSmallBlindPlayerName(Match smallBlindPlayerName)
        {
            SmallBlindPlayerName = smallBlindPlayerName.Groups["PlayerName"].Value;
        }
    }
}