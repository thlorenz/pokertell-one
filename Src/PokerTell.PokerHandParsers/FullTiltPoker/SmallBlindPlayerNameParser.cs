namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Interfaces.Parsers;

    public class SmallBlindPlayerNameParser : Base.SmallBlindPlayerNameParser, IFullTiltPokerSmallBlindPlayerNameParser 
    {
        const string FullTiltSmallBlindPattern = @"(?<PlayerName>.+) posts the small blind";

        protected override string SmallBlindPattern
        {
            get { return FullTiltSmallBlindPattern; }
        }
    }
}