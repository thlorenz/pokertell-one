namespace PokerTell.PokerHandParsers.PokerStars
{
    public class PlayerSeatsParser : Base.PlayerSeatsParser
    {
        #region Constants and Fields

        const string PokerStarsSeatPattern =
            @"Seat (?<SeatNumber>\d{1,2}): (?<PlayerName>.+) \("
            + SharedPatterns.RatioPattern
            + @" in chips\) *(?<OutOfHand>out of hand)*";

        #endregion

        #region Properties

        protected override string SeatPattern
        {
            get { return PokerStarsSeatPattern; }
        }

        #endregion
    }
}