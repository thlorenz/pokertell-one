namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class BlindsParser : Base.BlindsParser
    {
        #region Constants and Fields

        const string FullTiltBlindsPattern =
            @" - " + SharedPatterns.RatioPattern + "/" + SharedPatterns.Ratio2Pattern + " ";

        #endregion

        #region Properties

        protected override string BlindsPattern
        {
            get { return FullTiltBlindsPattern; }
        }

        #endregion
    }
}