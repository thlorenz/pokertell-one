namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System;

    public class GameTypeParser : Base.GameTypeParser
    {
        const string FullTiltGameTypePattern = @"- (?<GameType>(Limit|Pot Limit|No Limit)) (Hold'em|Holdem) -";

        protected override string GameTypePattern
        {
            get { return FullTiltGameTypePattern; }
        }
    }
}