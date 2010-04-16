namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.PokerHandParsers.Interfaces.Parsers;

    public class PlayerSeatsParser : Base.PlayerSeatsParser, IPokerStarsPlayerSeatsParser
    {
        const string PokerStarsSeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \("
            + SharedPatterns.RatioPattern
            + @" in chips\) *(?<OutOfHand>out of hand)*";

        protected override string SeatPattern
        {
            get { return PokerStarsSeatPattern; }
        }
    }
}