namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class BoardParserTests : Base.BoardParserTests
    {
        protected override string ValidBoard(string board)
        {
            // Board [6h 8d 4s Js]
            return string.Format("Board [{0}]", board);
        }

        protected override BoardParser GetBoardParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.BoardParser();
        }
    }
}