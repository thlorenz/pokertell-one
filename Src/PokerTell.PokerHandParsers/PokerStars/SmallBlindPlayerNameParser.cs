namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class SmallBlindPlayerNameParser : Base.SmallBlindPlayerNameParser, IPokerStarsSmallBlindPlayerNameParser
    {
        const string PokerStarsSmallBlindPattern = @"(?<PlayerName>.+): posts small blind";

        protected override string SmallBlindPattern
        {
            get { return PokerStarsSmallBlindPattern; }
        }
    }
}