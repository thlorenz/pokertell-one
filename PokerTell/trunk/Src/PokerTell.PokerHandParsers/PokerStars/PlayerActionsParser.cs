namespace PokerTell.PokerHandParsers.PokerStars
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    public class PlayerActionsParser : PokerHandParsers.PlayerActionsParser
    {
        #region Constants and Fields

        const string AllinBetPattern = @": bets " + SharedPatterns.RatioPattern + @" and is all-in";

        const string UncalledBetPattern = @"Uncalled bet \(" + SharedPatterns.RatioPattern + @"\) returned to *";

        const string WinningPattern = @".+collected " + SharedPatterns.RatioPattern + " from .*pot";

        string _playerName;

        string _streetHistory;

        #endregion

        #region Constructors and Destructors

        public PlayerActionsParser(Constructor<IAquiredPokerAction> aquiredPokerActionMake)
            : base(aquiredPokerActionMake)
        {
            CreateActionStrings();
        }

        #endregion

        #region Properties

        string ActionPattern
        {
            get
            {
                return
                    "(" +
                    @"((?<What>" + ActionStrings[ActionTypes.P] + ") (((small|big) blind)|(the ante)) " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.C] + "|" + ActionStrings[ActionTypes.B] + ") " +
                    SharedPatterns.RatioPattern + ")" +
                    @"|((?<What>" + ActionStrings[ActionTypes.R] + ") " + SharedPatterns.RatioPattern + ")" +
                    @"|(?<What>" + ActionStrings[ActionTypes.F] + "|" + ActionStrings[ActionTypes.X] + ")" +
                    ")";
            }
        }

        #endregion

        #region Public Methods

        public override void Parse(string streetHistory, string playerName)
        {
            _streetHistory = streetHistory;
            _playerName = Regex.Escape(playerName);
            PlayerActions = new List<IAquiredPokerAction>();

            MatchCollection actions = MatchAllPlayerActions();

            ExtractAllActions(actions);

            ExtractUncalledBetActionIfItExists();
            ExtractAllinBetActionIfItExists();
            ExtractWinningActionIfItExists();
        }

        #endregion

        #region Methods

        protected override sealed void CreateActionStrings()
        {
            base.CreateActionStrings();
        }

        void ExtractAction(Match action)
        {
            string actionString = action.Groups["What"].Value;
            double ratio = action.Groups["Ratio"].Success ? Convert.ToDouble(action.Groups["Ratio"].Value) : 1.0;

            ActionTypes actionType = ConvertActionString(actionString);

            IAquiredPokerAction aquiredAction = _aquiredPokerActionMake.New.InitializeWith(actionType, ratio);

            PlayerActions.Add(aquiredAction);
        }

        void ExtractAllActions(MatchCollection actions)
        {
            foreach (Match action in actions)
            {
                ExtractAction(action);
            }
        }

        void ExtractAllinBetAction(Match allinBet)
        {
            double ratio = Convert.ToDouble(allinBet.Groups["Ratio"].Value);
            IAquiredPokerAction allinBetAction = _aquiredPokerActionMake.New.InitializeWith(ActionTypes.A, ratio);
            PlayerActions.Add(allinBetAction);
        }

        void ExtractAllinBetActionIfItExists()
        {
            Match allinBet = MatchAllinBet();
            if (allinBet.Success)
            {
                ExtractAllinBetAction(allinBet);
            }
        }

        void ExtractUncalledBetAction(Match uncalledBet)
        {
            double ratio = Convert.ToDouble(uncalledBet.Groups["Ratio"].Value);
            IAquiredPokerAction uncalledBetAction = _aquiredPokerActionMake.New.InitializeWith(ActionTypes.U, ratio);
            PlayerActions.Add(uncalledBetAction);
        }

        void ExtractUncalledBetActionIfItExists()
        {
            Match uncalledBet = MatchUncalledBet();
            if (uncalledBet.Success)
            {
                ExtractUncalledBetAction(uncalledBet);
            }
        }

        void ExtractWinningAction(Match winning)
        {
            double ratio = Convert.ToDouble(winning.Groups["Ratio"].Value);
            IAquiredPokerAction winningAction = _aquiredPokerActionMake.New.InitializeWith(ActionTypes.W, ratio);
            PlayerActions.Add(winningAction);
        }

        void ExtractWinningActionIfItExists()
        {
            Match winning = MatchWinning();
            if (winning.Success)
            {
                ExtractWinningAction(winning);
            }
        }

        Match MatchAllinBet()
        {
            string allinBetPatternForPlayer = _playerName + AllinBetPattern;
            return Regex.Match(_streetHistory, allinBetPatternForPlayer, RegexOptions.IgnoreCase);
        }

        MatchCollection MatchAllPlayerActions()
        {
            string playerActionPattern = string.Format("{0}: {1}", _playerName, ActionPattern);
            return Regex.Matches(_streetHistory, playerActionPattern, RegexOptions.IgnoreCase);
        }

        Match MatchUncalledBet()
        {
            string uncalledBetPatternForPlayer = UncalledBetPattern + _playerName;
            return Regex.Match(_streetHistory, uncalledBetPatternForPlayer, RegexOptions.IgnoreCase);
        }

        Match MatchWinning()
        {
            string winningPatternForPlayer = _playerName + WinningPattern;
            return Regex.Match(_streetHistory, winningPatternForPlayer, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}