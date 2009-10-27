namespace PokerTell.PokerHand.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Tests.Fakes;
    using PokerTell.PokerHand.ViewModels;
    using PokerTell.UnitTests.Tools;

    using Tools.Extensions;

    using UnitTests;

    [TestFixture]
    internal class ThatHandHistoryViewModel
    {
        [SetUp]
        public void _Init()
        {
            InitConvertedPokerHand();
            _stub = new StubBuilder();
        }

        IConvertedPokerHand _pokerHand;

        IHandHistoryViewModel _handHistoryViewModel;

        StubBuilder _stub;

        void InitConvertedPokerHand()
        {
            var player1 = new ConvertedPokerPlayer("player1", 10, 5, 0, 6, "As Kd") { new ConvertedPokerRound() };
            player1[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.2));

            var player2 = new ConvertedPokerPlayer("player2", 12, 4, 1, 6, "?? ??") { new ConvertedPokerRound() };
            player2[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.X, 1.0));

            var player3 = new ConvertedPokerPlayer("player3", 13, 2, 2, 6, "?? ??") { new ConvertedPokerRound() };
            player3[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player4 = new ConvertedPokerPlayer("player4", 14, 4, 3, 6, "?? ??") { new ConvertedPokerRound() };
            player4[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.F, 1.0));

            var player5 = new ConvertedPokerPlayer("player5", 15, 3, 4, 6, "?? ??") { new ConvertedPokerRound() };
            player5[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player6 = new ConvertedPokerPlayer("player6", 16, 14, 5, 6, "?? ??") { new ConvertedPokerRound() };
            player6[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.F, 1.0));

            _pokerHand = new ConvertedPokerHand("PokerStars", 1234, DateTime.MinValue, 200, 100, 6);
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
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresIsSelected(
            [Values(true, false)] bool parameter)
        {
            var historyViewModel = new HandHistoryViewModel { IsSelected = parameter };
            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().IsSelected, Is.EqualTo(historyViewModel.IsSelected));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresNote()
        {
            var historyViewModel = new HandHistoryViewModel { Note = "someNote" };
            Assert.That(historyViewModel.BinaryDeserializedInMemory().Note, Is.EqualTo(historyViewModel.Note));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresSelectedRow()
        {
            var historyViewModel = new HandHistoryViewModel { SelectedRow = _stub.Some(1) };
            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().SelectedRow, Is.EqualTo(historyViewModel.SelectedRow));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresVisible(
            [Values(true, false)] bool parameter)
        {
            var historyViewModel = new HandHistoryViewModel { Visible = parameter };
            Assert.That(historyViewModel.BinaryDeserializedInMemory().Visible, Is.EqualTo(historyViewModel.Visible));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowSelectOption(
            [Values(true, false)] bool parameter)
        {
            var historyViewModel = new HandHistoryViewModel { ShowSelectOption = parameter };
            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().ShowSelectOption,
                Is.EqualTo(historyViewModel.ShowSelectOption));
        }

        [Test]
        [Sequential]
        public void BinaryDeserialize_Serialized_RestoresShowPreflopFolds(
            [Values(true, false)] bool parameter)
        {
            var historyViewModel = new HandHistoryViewModel { ShowPreflopFolds = parameter };
            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().ShowPreflopFolds,
                Is.EqualTo(historyViewModel.ShowPreflopFolds));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresHand()
        {
            var historyViewModel = new HandHistoryViewModel().UpdateWith(_pokerHand);
            Assert.That(historyViewModel.BinaryDeserializedInMemory().Hand, Is.EqualTo(historyViewModel.Hand));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresPlayerRows()
        {
            var historyViewModel = new HandHistoryViewModel().UpdateWith(_pokerHand);

            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().PlayerRows, Is.EqualTo(historyViewModel.PlayerRows));
        }

        [Test]
        public void BinaryDeserialize_Serialized_RestoresBoard()
        {
            _pokerHand.Board = _stub.Valid(For.Board, "As Ks 9h");
            var historyViewModel = new HandHistoryViewModel().UpdateWith(_pokerHand);

            Assert.That(
                historyViewModel.BinaryDeserializedInMemory().Board, Is.EqualTo(historyViewModel.Board));
        }
    }
}