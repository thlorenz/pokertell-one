namespace PokerTell.PokerHandParsers.PokerStars
{
    public class SmallBlindPlayerNameParser : Base.SmallBlindPlayerNameParser
    {
        #region Constants and Fields

        const string PokerStarsSmallBlindPattern = @"(?<PlayerName>.+): posts small blind";

        #endregion

        #region Properties

        protected override string SmallBlindPattern
        {
            get { return PokerStarsSmallBlindPattern; }
        }

        #endregion
    }
}