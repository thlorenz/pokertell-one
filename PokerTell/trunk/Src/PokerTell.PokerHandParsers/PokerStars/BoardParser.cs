namespace PokerTell.PokerHandParsers.PokerStars
{
    public class BoardParser : Base.BoardParser
    {
        #region Constants and Fields

        const string PokerStarsBoardPattern = @"Board.*\[(?<Board>(" + SharedPatterns.CardPattern + @" *){0,5}).*\]";

        #endregion

        #region Properties

        protected override string BoardPattern
        {
            get { return PokerStarsBoardPattern; }
        }

        #endregion
    }
}