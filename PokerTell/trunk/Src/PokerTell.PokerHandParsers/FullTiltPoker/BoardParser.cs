using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class BoardParser : PokerHandParsers.BoardParser
    {
        #region Constants and Fields

        const string BoardPattern = @"Board:.*\[(?<Board>(" + SharedPatterns.CardPattern + @" *){0,5}).*\]";

        #endregion

        #region Public Methods

        public override PokerHandParsers.BoardParser Parse(string handHistory)
        {
            Match board = MatchBoard(handHistory);
            IsValid = board.Success;
           
            if (IsValid)
            {
                ExtractBoard(board);
            }

            return this;
        }

        #endregion

        #region Methods

        static Match MatchBoard(string handHistory)
        {
            return Regex.Match(handHistory, BoardPattern, RegexOptions.IgnoreCase);
        }

        void ExtractBoard(Match board)
        {
            Board = board.Groups["Board"].Value;
        }

        #endregion
    }
}