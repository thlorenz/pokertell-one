namespace PokerTell.PokerHandParsers.FullTiltPoker
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public class PlayerActionsParser : Base.PlayerActionsParser
    {
        #region Constants and Fields

        const string FullTiltAllinBetPattern = @".+" + SharedPatterns.RatioPattern + @", and is all in";

        const string FullTiltUncalledBetPattern = @"Uncalled bet of " + SharedPatterns.RatioPattern + @" returned to *";

        const string FullTiltWinningPattern = @".+and won \(" + SharedPatterns.RatioPattern + @"\)";

        #endregion

        #region Properties

        public PlayerActionsParser(IConstructor<IAquiredPokerAction> aquiredPokerActionMake)
            : base(aquiredPokerActionMake)
        {
        }

        protected override string ActionPattern
        {
            get { return string.Format("{0} {1}", _playerName, FullTiltActionPattern); }
        }

        protected override string AllinBetPattern
        {
            get { return _playerName + FullTiltAllinBetPattern; }
        }

        protected override string UncalledBetPattern
        {
            get { return FullTiltUncalledBetPattern + _playerName; }
        }

        protected override string WinningPattern
        {
            get { return _playerName + FullTiltWinningPattern; }
        }

        string FullTiltActionPattern
        {
            get
            {
                return
                    "(" +
                    @"((?<What>" + ActionStrings[ActionTypes.P] + ") the (small|big) blind of " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>antes) " + SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.C] + "|" + ActionStrings[ActionTypes.B] + ") " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.R] + ")" + @" to " + SharedPatterns.RatioPattern + ")" +
                    @"|(?<What>" + ActionStrings[ActionTypes.F] + "|" + ActionStrings[ActionTypes.X] + ")" +
                    ")";
            }
        }

        #endregion
    }
}