namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    public abstract class HoleCardsParser
    {
        protected const string HoleCardsPattern =
            @".+\[" +
            @"(?<HoleCard1>" + SharedPatterns.CardPattern + @") " +
            @"(?<HoleCard2>" + SharedPatterns.CardPattern + @")" +
            @"\]";

        protected string _handHistory;

        protected string _playerName;

        public string Holecards { get; protected set; }

        protected abstract string HeroHoleCardsPattern { get; }

        protected abstract string ShownOrMuckedHoleCardsPattern { get; }

        public virtual HoleCardsParser Parse(string handHistory, string playerName)
        {
            _handHistory = handHistory;
            _playerName = Regex.Escape(playerName);

            Holecards = string.Empty;

            Match holeCards = MatchHeroHoleCards();
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

        void ExtractHoleCards(Match holeCards)
        {
            Holecards = string.Format("{0} {1}", holeCards.Groups["HoleCard1"], holeCards.Groups["HoleCard2"]);
        }

        Match MatchHeroHoleCards()
        {
            return Regex.Match(_handHistory, HeroHoleCardsPattern, RegexOptions.IgnoreCase);
        }

        Match MatchShownOrMuckedHoleCards()
        {
            return Regex.Match(_handHistory, ShownOrMuckedHoleCardsPattern, RegexOptions.IgnoreCase);
        }
    }
}