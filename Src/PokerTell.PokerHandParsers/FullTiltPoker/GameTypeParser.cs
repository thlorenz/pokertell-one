namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class GameTypeParser : Base.GameTypeParser, IFullTiltPokerGameTypeParser
    {
        const string FullTiltGameTypePattern = @"- (?<GameType>(Limit|Pot Limit|No Limit)) (Hold'em|Holdem) -";

        protected override string GameTypePattern
        {
            get { return FullTiltGameTypePattern; }
        }
    }
}