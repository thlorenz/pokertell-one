namespace PokerTell.PokerHandParsers.PokerStars
{
    using System.Text.RegularExpressions;

    public class HoleCardsParser : PokerHandParsers.HoleCardsParser
    {
        const string DealtToPattern = @"Dealt to ";

        const string ShowsOrMuckedPattern = @".+(showed|mucked)";
        const string HoleCardsPattern = 
                @".+\[" + 
                @"(?<HoleCard1>" + SharedPatterns.CardPattern + @") " +
                @"(?<HoleCard2>" + SharedPatterns.CardPattern + @")" + 
                @"\]";

        public override PokerHandParsers.HoleCardsParser Parse(string handHistory, string playerName)
        {
            _handHistory = handHistory;
            _playerName = Regex.Escape(playerName);

            Holecards = string.Empty;

            Match holeCards = MatchDealtToHoleCards();
            if (holeCards.Success)
            {
                ExtractHoleCards(holeCards);
                return this;
            }

            holeCards = MatchShownOrMuckedHoleCards();
            if (holeCards.Success)
            {
                ExtractHoleCards(holeCards);
            }

            return this;
        }

        Match MatchShownOrMuckedHoleCards()
        {
            string showsOrMuckedPatternForPlayer = string.Format(
                "{0}{1}{2}", _playerName, ShowsOrMuckedPattern, HoleCardsPattern);
            return Regex.Match(_handHistory, showsOrMuckedPatternForPlayer, RegexOptions.IgnoreCase);
        }

        void ExtractHoleCards(Match holeCards)
        {
            Holecards = string.Format("{0} {1}", holeCards.Groups["HoleCard1"], holeCards.Groups["HoleCard2"]);
        }

        Match MatchDealtToHoleCards()
        {
            string dealtToPatternForPlayer = string.Format("{0}{1}{2}", DealtToPattern, _playerName, HoleCardsPattern);
            return Regex.Match(_handHistory, dealtToPatternForPlayer, RegexOptions.IgnoreCase);
        }
    }
}