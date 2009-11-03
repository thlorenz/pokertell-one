namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class SmallBlindPlayerNameParser : Base.SmallBlindPlayerNameParser
    {
        #region Constants and Fields

        const string FullTiltSmallBlindPattern = @"(?<PlayerName>.+) posts the small blind";

        #endregion

        #region Properties

        protected override string SmallBlindPattern
        {
            get { return FullTiltSmallBlindPattern; }
        }

        #endregion
    }
}