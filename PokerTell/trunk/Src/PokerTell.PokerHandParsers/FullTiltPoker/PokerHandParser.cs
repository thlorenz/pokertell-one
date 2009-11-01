using PokerTell.Infrastructure.Interfaces;
using PokerTell.Infrastructure.Interfaces.PokerHand;

namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class PokerHandParser : PokerStars.PokerHandParser
    {
        public PokerHandParser(IConstructor<IAquiredPokerHand> aquiredHandMake, IConstructor<IAquiredPokerPlayer> aquiredPlayerMake, IConstructor<IAquiredPokerRound> aquiredRoundMake, IConstructor<IAquiredPokerAction> aquiredActionMake)
            : base(aquiredHandMake, aquiredPlayerMake, aquiredRoundMake, aquiredActionMake)
        {
            Site = "FullTiltPoker";
        }
    }
}