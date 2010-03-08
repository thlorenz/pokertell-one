namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    public class TotalPotParser : Base.TotalPotParser
    {
        #region Constants and Fields

        const string FullTiltTotalPotPattern = @"Total pot " + SharedPatterns.RatioPattern;

        #endregion

        #region Properties

        protected override string TotalPotPattern
        {
            get { return FullTiltTotalPotPattern; }
        }

        #endregion
    }
}