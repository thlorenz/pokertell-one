using System.Text.RegularExpressions;

namespace PokerTell.PokerHandParsers.Base
{
    public abstract class BoardParser
    {
        public bool IsValid { get; protected set; }

        public string Board { get; protected set; }

        protected abstract string BoardPattern { get; }

        #region Public Methods

        public virtual BoardParser Parse(string handHistory)
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

        protected virtual Match MatchBoard(string handHistory)
        {
            return Regex.Match(handHistory, BoardPattern, RegexOptions.IgnoreCase);
        }

        protected virtual void ExtractBoard(Match board)
        {
            Board = board.Groups["Board"].Value;
        }
    }
}