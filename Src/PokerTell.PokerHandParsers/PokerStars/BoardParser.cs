namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class BoardParser : Base.BoardParser, IPokerStarsBoardParser
    {
        const string PokerStarsBoardPattern = @"Board.*\[(?<Board>(" + SharedPatterns.CardPattern + @" *){0,5}).*\]";

        protected override string BoardPattern
        {
            get { return PokerStarsBoardPattern; }
        }
    }
}