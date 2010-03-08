namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class PlayerSeatsParser : Base.PlayerSeatsParser
    {
        #region Constants and Fields

        const string FullTiltSeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \("
            + SharedPatterns.RatioPattern
            + @"\) *(?<OutOfHand>, is sitting out){0,1}";

        #endregion

        #region Properties

        protected override string SeatPattern
        {
            get { return FullTiltSeatPattern; }
        }

        #endregion
    }
}