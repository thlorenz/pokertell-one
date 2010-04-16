namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class BoardParser : Base.BoardParser, IFullTiltPokerBoardParser 
    {
        const string FullTiltBoardPattern = @"Board:.*\[(?<Board>(" + SharedPatterns.CardPattern + @" *){0,5}).*\]";

        protected override string BoardPattern
        {
            get { return FullTiltBoardPattern; }
        }
    }
}