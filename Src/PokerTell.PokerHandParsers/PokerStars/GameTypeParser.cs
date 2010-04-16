namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class GameTypeParser : Base.GameTypeParser, IPokerStarsGameTypeParser
    {
        const string PokerStarsGameTypePattern = @"(Hold'em|Holdem) (?<GameType>(Limit|Pot Limit|No Limit))";

        protected override string GameTypePattern
        {
            get { return PokerStarsGameTypePattern; }
        }
    }
}