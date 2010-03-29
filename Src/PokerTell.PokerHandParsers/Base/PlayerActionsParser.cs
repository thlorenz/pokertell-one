namespace PokerTell.PokerHandParsers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    public abstract class PlayerActionsParser
    {
        public IDictionary<ActionTypes, string> ActionStrings;

        protected readonly IConstructor<IAquiredPokerAction> _aquiredPokerActionMake;

        protected string _playerName;

        protected string _streetHistory;

        protected PlayerActionsParser(IConstructor<IAquiredPokerAction> aquiredPokerActionMake)
        {
            _aquiredPokerActionMake = aquiredPokerActionMake;
            CreateActionStrings();
        }

        public IList<IAquiredPokerAction> PlayerActions { get; protected set; }

        protected abstract string ActionPattern { get; }

        protected abstract string AllinBetPattern { get; }

        protected abstract string UncalledBetPattern { get; }

        protected abstract string WinningPattern { get; }

        public virtual PlayerActionsParser Parse(string streetHistory, string playerName)
        {
            _streetHistory = streetHistory;
            _playerName = Regex.Escape(playerName);

            PlayerActions = new List<IAquiredPokerAction>();

            MatchCollection actions = MatchAllPlayerActions();

            ExtractAllActions(actions);

            ExtractUncalledBetActionIfItExists();
            ExtractAllinBetActionIfItExists();
            ExtractWinningActionIfItExists();

            return this;
        }

        /// <summary>
        /// Converts a human readable action to an Action What type and is called by the Hand Parser
        /// </summary>
        /// <param name="actionString">Human readable action as found in Hand History</param>
        /// <returns>An ActionTypes type defining the action</returns>
        protected static ActionTypes ConvertActionString(string actionString)
        {
            actionString = actionString.ToLower();
            if (actionString.Contains("post") || actionString.Contains("antes"))
            {
                return ActionTypes.P;
            }

            if (actionString.Contains("fold"))
            {
                return ActionTypes.F;
            }

            if (actionString.Contains("check"))
            {
                return ActionTypes.X;
            }

            if (actionString.Contains("call"))
            {
                return ActionTypes.C;
            }

            if (actionString.Contains("bet"))
            {
                return ActionTypes.B;
            }

            if (actionString.Contains("raise"))
            {
                return ActionTypes.R;
            }

            if (actionString.Contains("collect") || actionString.Contains("won"))
            {
                return ActionTypes.W;
            }

            throw new FormatException("Invalid Action string " + actionString);
        }

        void CreateActionStrings()
        {
            ActionStrings = new Dictionary<ActionTypes, string>
                {
                    { ActionTypes.B, "bets" }, 
                    { ActionTypes.C, "calls" }, 
                    { ActionTypes.F, "folds" }, 
                    { ActionTypes.P, "posts" }, 
                    { ActionTypes.R, "raises" }, 
                    { ActionTypes.X, "checks" }, 
                };
        }

        void ExtractAction(Match action)
        {
            string actionString = action.Groups["What"].Value;
            double ratio = action.Groups["Ratio"].Success
                               ? Convert.ToDouble(action.Groups["Ratio"].Value.Replace(",", string.Empty))
                               : 1.0;

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
            double ratio = Convert.ToDouble(allinBet.Groups["Ratio"].Value.Replace(",", string.Empty));
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
            double ratio = Convert.ToDouble(uncalledBet.Groups["Ratio"].Value.Replace(",", string.Empty));
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
            double ratio = Convert.ToDouble(winning.Groups["Ratio"].Value.Replace(",", string.Empty));
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
            return Regex.Match(_streetHistory, AllinBetPattern, RegexOptions.IgnoreCase);
        }

        MatchCollection MatchAllPlayerActions()
        {
            return Regex.Matches(_streetHistory, ActionPattern, RegexOptions.IgnoreCase);
        }

        Match MatchUncalledBet()
        {
            return Regex.Match(_streetHistory, UncalledBetPattern, RegexOptions.IgnoreCase);
        }

        Match MatchWinning()
        {
            return Regex.Match(_streetHistory, WinningPattern, RegexOptions.IgnoreCase);
        }
    }
}