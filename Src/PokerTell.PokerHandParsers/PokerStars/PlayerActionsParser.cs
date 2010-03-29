namespace PokerTell.PokerHandParsers.PokerStars
{
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public class PlayerActionsParser : Base.PlayerActionsParser
    {
        const string PokerStarsAllinBetPattern = @": .+" + SharedPatterns.RatioPattern + @" and is all-in";

        const string PokerStarsUncalledBetPattern =
            @"Uncalled bet \(" + SharedPatterns.RatioPattern + @"\) returned to *";

        const string PokerStarsWinningPattern = @".+collected " + SharedPatterns.RatioPattern + " from .*pot";

        public PlayerActionsParser(IConstructor<IAquiredPokerAction> aquiredPokerActionMake)
            : base(aquiredPokerActionMake)
        {
        }

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
    }
}