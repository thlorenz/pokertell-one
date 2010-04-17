namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using System.Linq;

    using Interfaces.Parsers;

    public class PlayerSeatsParser : Base.PlayerSeatsParser, IFullTiltPokerPlayerSeatsParser 
    {
        const string FullTiltSeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \("
            + SharedPatterns.RatioPattern
            + @"\) *(?<OutOfHand>, is sitting out){0,1}";

        protected override string SeatPattern
        {
            get { return FullTiltSeatPattern; }
        }

        public int HighestSeatNumber
        {
            get { return PlayerSeats.Keys.Max(); }
        }
    }
}