namespace PokerTell.PokerHandParsers.PokerStars
{
    public class BlindsParser : Base.BlindsParser
    {
        #region Constants and Fields

        const string PokerStarsBlindsPattern =
            @"\(" + SharedPatterns.RatioPattern + @"/" + SharedPatterns.Ratio2Pattern;

        #endregion

        #region Properties

        protected override string BlindsPattern
        {
            get { return PokerStarsBlindsPattern; }
        }

        #endregion
    }
}