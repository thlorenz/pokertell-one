namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using System;

    public class ThatBoardParser : Tests.ThatBoardParser
    {
        protected override string ValidBoard(string board)
        {
            // Board [6h 8d 4s Js]
            return string.Format("Board [{0}]", board);
        }

        protected override BoardParser GetBoardParser()
        {
            return new PokerHandParsers.PokerStars.BoardParser();
        }
    }
}