using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class HoleCardsParser
    {
        #region Constants and Fields

        protected const string HoleCardsPattern =
            @".+\[" +
            @"(?<HoleCard1>" + SharedPatterns.CardPattern + @") " +
            @"(?<HoleCard2>" + SharedPatterns.CardPattern + @")" +
            @"\]";

        protected string _handHistory;

        protected string _playerName;

        #endregion

        #region Properties

        public string Holecards { get; protected set; }

        protected abstract string HeroHoleCardsPattern { get; }

        protected abstract string ShownOrMuckedHoleCardsPattern { get; }

        #endregion

        #region Public Methods

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

        #endregion

        #region Methods

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

        #endregion
    }
}