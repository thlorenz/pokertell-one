namespace PokerTell.PokerHandParsers.Base
{
    using System.Text.RegularExpressions;

    using Interfaces.Parsers;

    public abstract class BoardParser : IBoardParser
    {
        public string Board { get; protected set; }

        public bool IsValid { get; protected set; }

        protected abstract string BoardPattern { get; }

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

        protected virtual void ExtractBoard(Match board)
        {
            Board = board.Groups["Board"].Value;
        }

        protected virtual Match MatchBoard(string handHistory)
        {
            return Regex.Match(handHistory, BoardPattern, RegexOptions.IgnoreCase);
        }
    }
}