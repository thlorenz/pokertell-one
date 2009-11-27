using System;

namespace PokerTell.PokerHandParsers.Tests.FullTiltPoker
{
    using Base;

    public class BoardParserTests : Tests.BoardParserTests
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