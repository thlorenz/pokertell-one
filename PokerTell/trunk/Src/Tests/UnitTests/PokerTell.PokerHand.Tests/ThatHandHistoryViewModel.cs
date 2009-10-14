namespace PokerTell.PokerHand.Tests
{
    using System;

    using Analyzation;

    using Fakes;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Moq;

    using NUnit.Framework;

    using ViewModels;

    [TestFixture]
    internal class ThatHandHistoryViewModel
    {
        [SetUp]
        public void _Init()
        {
            InitConvertedPokerHand();
            _stub = new StubBuilder();
        }

        private IConvertedPokerHand _pokerHand;
        private IHandHistoryViewModel _handHistoryViewModel;

        StubBuilder _stub;

        private void InitConvertedPokerHand()
        {
            var player1 = new ConvertedPokerPlayer("player1", 10, 5, 0, 6, "As Kd");
            player1.AddRound(new ConvertedPokerRound());
            player1[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.2));

            var player2 = new ConvertedPokerPlayer("player2", 12, 4, 1, 6, "?? ??");
            player2.AddRound(new ConvertedPokerRound());
            player2[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.X, 1.0));

            var player3 = new ConvertedPokerPlayer("player3", 13, 2, 2, 6, "?? ??");
            player3.AddRound(new ConvertedPokerRound());
            player3[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player4 = new ConvertedPokerPlayer("player4", 14, 4, 3, 6, "?? ??");
            player4.AddRound(new ConvertedPokerRound());
            player4[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.F, 1.0));

            var player5 = new ConvertedPokerPlayer("player5", 15, 3, 4, 6, "?? ??");
            player5.AddRound(new ConvertedPokerRound());
            player5[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player6 = new ConvertedPokerPlayer("player6", 16, 14, 5, 6, "?? ??");
            player6.AddRound(new ConvertedPokerRound());
            player6[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.F, 1.0));

            _pokerHand = new ConvertedPokerHand("PokerStars", 1234, DateTime.Now, 200, 100, 6);
            _pokerHand.AddPlayer(player1);
            _pokerHand.AddPlayer(player2);
            _pokerHand.AddPlayer(player3);
            _pokerHand.AddPlayer(player4);
            _pokerHand.AddPlayer(player5);
            _pokerHand.AddPlayer(player6);

            _pokerHand.Ante = 50;
            _pokerHand.TournamentId = 2354;
        }

        [Test]
        public void AddsAllPlayersToRows_IfShowPreflopFoldsIsRequested()
        {
            _handHistoryViewModel = new HandHistoryViewModel(true);
            _handHistoryViewModel.UpdateWith(_pokerHand);

            Assert.That(_handHistoryViewModel.PlayerRows.Count, Is.EqualTo(6));
        }

        [Test]
        public void AddsOnlyActivePlayersToRows_ByDefault()
        {
            _handHistoryViewModel = new HandHistoryViewModel();
            _handHistoryViewModel.UpdateWith(_pokerHand);

            Assert.That(_handHistoryViewModel.PlayerRows.Count, Is.EqualTo(4));
        }

        [Test]
        public void CreatesHeaderCorrectly()
        {
            _handHistoryViewModel = new HandHistoryViewModel();
            _handHistoryViewModel.UpdateWith(_pokerHand);

            Assert.That(_handHistoryViewModel.TournamentId, Is.EqualTo(_pokerHand.TournamentId.ToString()));
            Assert.That(_handHistoryViewModel.GameId, Is.EqualTo(_pokerHand.GameId.ToString()));
            Assert.That(_handHistoryViewModel.BigBlind, Is.EqualTo(_pokerHand.BB.ToString()));
            Assert.That(_handHistoryViewModel.SmallBlind, Is.EqualTo(_pokerHand.SB.ToString()));
            Assert.That(_handHistoryViewModel.Ante, Is.EqualTo(_pokerHand.Ante.ToString()));
            Assert.That(_handHistoryViewModel.TotalPlayers, Is.EqualTo(_pokerHand.TotalPlayers.ToString()));
            Assert.That(_handHistoryViewModel.TimeStamp, Is.EqualTo(_pokerHand.TimeStamp.ToString()));
        }

        [Test]
        public void CreatesBoardViewModelAndUpdatesItOnUpdate()
        {
            _pokerHand.Board = "As Kh Qd";

            var board = new BoardViewModel();
            board.UpdateWith(_pokerHand.Board);

            _handHistoryViewModel = new HandHistoryViewModel();
            _handHistoryViewModel.UpdateWith(_pokerHand);

            Assert.That(_handHistoryViewModel.Board, Is.EqualTo(board));
        }

        [Test]
        public void AdjustToConditionAction_ConditionIsFullFilledReturnsFalse_SetsVisibleToFalse()
        {
            _handHistoryViewModel = new HandHistoryViewModel { Visible = true };

            var conditionStub = new StubCondition { FullFill = false };

            _handHistoryViewModel.AdjustToConditionAction.Invoke(conditionStub);

            Assert.That(_handHistoryViewModel.Visible, Is.False);
        }

        [Test]
        public void AdjustToConditionAction_ConditionIsFullFilledReturnsTrue_SetsVisibleToTrue()
        {
            _handHistoryViewModel = new HandHistoryViewModel { Visible = false };

            var conditionStub = new StubCondition { FullFill = true };

            _handHistoryViewModel.AdjustToConditionAction.Invoke(conditionStub);

            Assert.That(_handHistoryViewModel.Visible, Is.True);
        }

        [Test]
        public void Note_IsSet_SetsNoteOnContainedHand()
        {
            var hand = new ConvertedPokerHand();
            _handHistoryViewModel = new HandHistoryViewModel()
                .UpdateWith(hand);
            
            const string line1 = "Test Line1";
            const string line2 = "Test Line2";
            var newNote = new[] { line1, line2 };

            _handHistoryViewModel.Note = newNote;

            Assert.That(hand.Note, Is.EqualTo(newNote));
        }
    }
}