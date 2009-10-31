namespace PokerTell.PokerHandParsers.Tests
{
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NUnit.Framework;

    using PokerHand.Aquisition;

    using UnitTests.Tools;

    public abstract class ThatPlayerActionsParser
    {
        protected PlayerActionsParser _parser;

        const string PlayerName = "Hero";

        [SetUp]
        public void _Init()
        {
            _parser = GetPlayerActionsParser();
        }

        [Test]
        public void Parse_EmptyString_PlayerActionsAreEmpty()
        {
            _parser.Parse(string.Empty, PlayerName);
            Assert.That(_parser.PlayerActions.Count, Is.EqualTo(0));
        }

        [Test]
        public void Parse_NoActions_PlayerActionsAreEmpty()
        {
            _parser.Parse("noActionsInThis", PlayerName);
            Assert.That(_parser.PlayerActions.Count, Is.EqualTo(0));
        }

        [Test]
        [Sequential]
        public void Parse_PlayerHasOneActionWithRatio_AddsActionWithRatioToPlayerActions(
            [Values( ActionTypes.B, ActionTypes.C, ActionTypes.R)] ActionTypes actionType)
        {
            const double someRatio = 1.0;
            var aquiredPokerAction = new AquiredPokerAction(actionType, someRatio);
           
            string streetHistory = OneRatioActionFor(PlayerName, aquiredPokerAction);
            
            _parser.Parse(streetHistory, PlayerName);
           
            Affirm.That(_parser.PlayerActions.First()).IsEqualTo(aquiredPokerAction);
        }

        [Test]
        [Sequential]
        public void Parse_PlayerHasOnePostingAction_AddsActionWithRatioToPlayerActions(
            [Values(PostingTypes.Ante, PostingTypes.BigBlind, PostingTypes.SmallBlind)] PostingTypes postingType)
        {
            const double someRatio = 1.0;
            const ActionTypes actionType = ActionTypes.P;
            var aquiredPokerAction = new AquiredPokerAction(actionType, someRatio);
            
            string streetHistory = PostingActionFor(PlayerName, postingType, someRatio);

            _parser.Parse(streetHistory, PlayerName);
           
            Affirm.That(_parser.PlayerActions.First()).IsEqualTo(aquiredPokerAction);
        }

        [Test]
        [Sequential]
        public void Parse_PlayerHasOneActionWithoutRatio_AddsActionWithoutRatioToPlayerActions(
            [Values(ActionTypes.F, ActionTypes.X)] ActionTypes actionType)
        {
            var aquiredPokerAction = new AquiredPokerAction(actionType, 0);
           
            string streetHistory = OneNonRatioActionFor(PlayerName, aquiredPokerAction);

            _parser.Parse(streetHistory, PlayerName);
           
            Affirm.That(_parser.PlayerActions.First().What).IsEqualTo(aquiredPokerAction.What);
        }

        [Test]
        public void Parse_TwoActions_AddsFirstActionAsFirstToPlayerActions()
        {
            var action1 = new AquiredPokerAction(ActionTypes.B, 1.0);
            var action2 = new AquiredPokerAction(ActionTypes.R, 2.0);

            string streetHistory = OneRatioActionFor(PlayerName, action1) + " \n" +
                                   OneRatioActionFor(PlayerName, action2);

            _parser.Parse(streetHistory, PlayerName);

            Affirm.That(_parser.PlayerActions.First()).IsEqualTo(action1);
        }

        [Test]
        public void Parse_TwoActions_AddsSecondActionAsLastToPlayerActions()
        {
            var action1 = new AquiredPokerAction(ActionTypes.B, 1.0);
            var action2 = new AquiredPokerAction(ActionTypes.R, 2.0);

            string streetHistory = OneRatioActionFor(PlayerName, action1) + " \n" +
                                   OneRatioActionFor(PlayerName, action2);

            _parser.Parse(streetHistory, PlayerName);

            Affirm.That(_parser.PlayerActions.Last()).IsEqualTo(action2);
        }

        [Test]
        public void Parse_ContainsActionForOtherPlayers_DoesNotAddAnyActionToPlayerActions()
        {
            var action = new AquiredPokerAction(ActionTypes.B, 1.0);

            string streetHistory = OneRatioActionFor("OtherPlayer", action);

            _parser.Parse(streetHistory, PlayerName);

            Assert.That(_parser.PlayerActions.Count, Is.EqualTo(0));
        }

        [Test]
        public void Parse_PlayerSomeActionAndUncalledBetAction_AddsUncalledBetActionAsLastToPlayerActions()
        {
            const double  ratio = 1.0;
            var someAction = new AquiredPokerAction(ActionTypes.B, 1.0);
            var uncalledBetAction = new AquiredPokerAction(ActionTypes.U, ratio);

            string streetHistory = OneRatioActionFor(PlayerName, someAction) + " \n" + 
                UncalledBetActionFor(PlayerName, ratio);

            _parser.Parse(streetHistory, PlayerName);

            Affirm.That(_parser.PlayerActions.Last()).IsEqualTo(uncalledBetAction);
        }

        [Test]
        public void Parse_PlayerUncalledBetActionAndAllinBetAction_AddsAllinActionAsLastToPlayerActions()
        {
            const double ratio = 1.0;
            var allinAction = new AquiredPokerAction(ActionTypes.A, ratio);

            string streetHistory = UncalledBetActionFor(PlayerName, ratio) + " \n" +
                                   AllinBetActionFor(PlayerName, ratio);
                
            _parser.Parse(streetHistory, PlayerName);

            Affirm.That(_parser.PlayerActions.Last()).IsEqualTo(allinAction);
        }

        [Test]
        public void Parse_PlayerAllinActionAndWinningAction_AddsWinningActionAsLastToPlayerActions()
        {
            const double ratio = 1.0;
            var winningAction = new AquiredPokerAction(ActionTypes.W, ratio);

            string streetHistory = AllinBetActionFor(PlayerName, ratio) + " \n" +
                                   WinningActionFor(PlayerName, ratio);

            _parser.Parse(streetHistory, PlayerName);

            Affirm.That(_parser.PlayerActions.Last()).IsEqualTo(winningAction);
        }

        protected abstract PlayerActionsParser GetPlayerActionsParser();
        protected abstract string OneRatioActionFor(string playerName, IAquiredPokerAction action);
        protected abstract string PostingActionFor(string playerName, PostingTypes postingType, double ratio);
        protected abstract string OneNonRatioActionFor(string playerName, IAquiredPokerAction action);

        protected abstract string UncalledBetActionFor(string playerName, double ratio);
        protected abstract string AllinBetActionFor(string playerName, double ratio);
        protected abstract string WinningActionFor(string playerName, double ratio);

        public enum PostingTypes
        {
            Ante,
            BigBlind,
            SmallBlind
        }
    }
}