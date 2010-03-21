namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Base;

    public class BoardParserTests : Base.BoardParserTests
    {
        protected override string ValidBoard(string board)
        {
            // Board: [9d 9s Qc 5h 2d]
            return string.Format("Board: [{0}]", board);
        }

        protected override BoardParser GetBoardParser()
        {
            return new PokerTell.PokerHandParsers.FullTiltPoker.BoardParser();
        }
    }
}