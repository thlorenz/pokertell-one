namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;

    public class GameTypeParser : Base.GameTypeParser
    {
        const string PokerStarsGameTypePattern = @"(Hold'em|Holdem) (?<GameType>(Limit|Pot Limit|No Limit))";

        protected override string GameTypePattern
        {
            get { return PokerStarsGameTypePattern; }
        }
    }
}