namespace PokerTell.PokerHandParsers.PokerStars
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public class PlayerActionsParser : Base.PlayerActionsParser
    {
        #region Constants and Fields

        const string PokerStarsAllinBetPattern = @": .+" + SharedPatterns.RatioPattern + @" and is all-in";

        const string PokerStarsUncalledBetPattern =
            @"Uncalled bet \(" + SharedPatterns.RatioPattern + @"\) returned to *";

        const string PokerStarsWinningPattern = @".+collected " + SharedPatterns.RatioPattern + " from .*pot";

        #endregion

        #region Constructors and Destructors

        public PlayerActionsParser(IConstructor<IAquiredPokerAction> aquiredPokerActionMake)
            : base(aquiredPokerActionMake)
        {
        }

        #endregion

        #region Properties

        protected override string ActionPattern
        {
            get { return string.Format("{0}: {1}", _playerName, PokerStarsActionPattern); }
        }

        protected override string AllinBetPattern
        {
            get { return _playerName + PokerStarsAllinBetPattern; }
        }

        protected override string UncalledBetPattern
        {
            get { return PokerStarsUncalledBetPattern + _playerName; }
        }

        protected override string WinningPattern
        {
            get { return _playerName + PokerStarsWinningPattern; }
        }

        string PokerStarsActionPattern
        {
            get
            {
                return
                    "(" +
                    @"((?<What>" + ActionStrings[ActionTypes.P] + ") (((small|big) blind)|(the ante)) " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.C] + "|" + ActionStrings[ActionTypes.B] + ") " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.R] + ") " + SharedPatterns.Ratio2Pattern +
                    @" to " + SharedPatterns.RatioPattern + ")" +
                    @"|(?<What>" + ActionStrings[ActionTypes.F] + "|" + ActionStrings[ActionTypes.X] + ")" +
                    ")";
            }
        }

        #endregion
    }
}