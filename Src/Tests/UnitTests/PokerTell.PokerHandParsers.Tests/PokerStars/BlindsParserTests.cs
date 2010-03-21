namespace PokerTell.PokerHandParsers.Tests.PokerStars
{
    using PokerTell.PokerHandParsers.Base;

    public class BlindsParserTests : Base.BlindsParserTests
    {
        protected override string TournamentGameWithValidBlinds(double smallBlind, double bigBlind)
        {
            // Level XV (1000/2000)
            return string.Format("Level XV ({0}/{1})", smallBlind, bigBlind);
        }

        protected override BlindsParser GetBlindsParser()
        {
            return new PokerTell.PokerHandParsers.PokerStars.BlindsParser();
        }

        protected override string CashGameWithValidBlinds(double smallBlind, double bigBlind)
        {
            // ($0.10/$0.25 USD) 
            return string.Format("(${0}/${1} USD)", smallBlind, bigBlind);
        }
    }
}