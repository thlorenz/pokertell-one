namespace PokerTell.PokerHandParsers.PokerStars
{
    public class TotalPotParser : Base.TotalPotParser
    {
        #region Constants and Fields

        const string PokerStarsTotalPotPattern = @"Total pot " + SharedPatterns.RatioPattern;

        #endregion

        #region Properties

        protected override string TotalPotPattern
        {
            get { return PokerStarsTotalPotPattern; }
        }

        #endregion
    }
}