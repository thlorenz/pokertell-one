namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class BoardParser : Base.BoardParser
    {
        #region Constants and Fields

        const string FullTiltBoardPattern = @"Board:.*\[(?<Board>(" + SharedPatterns.CardPattern + @" *){0,5}).*\]";

        #endregion

        #region Properties

        protected override string BoardPattern
        {
            get { return FullTiltBoardPattern; }
        }

        #endregion
    }
}