namespace PokerTell.PokerHand.Tests
{
    using System;

    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.PokerHand.Analyzation;

    [TestFixture]
    internal class ConvertedPokerPlayerTests_SetActionSequence
    {
        #region Constants and Fields

        ConvertedPokerPlayer _convertedPlayer;

        #endregion

        #region Public Methods

        [SetUp]
        public void __Init()
        {
            const int someTotalPlayers = 6;
            const int someMBefore = 0;
            const int someMAfter = 0;
            const int somePosition = 0;

            string someHoleCards = string.Empty;
            _convertedPlayer = new ConvertedPokerPlayer();
            _convertedPlayer.InitializeWith(
                "someName", someMBefore, someMAfter, somePosition, someTotalPlayers, someHoleCards);
        }

        [Test]
        [Combinatorial]
        public void _PostFlopPlayerSequenceContainsNumberVaryActionType_CurrentSequenceIsNotChanged(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes actionType)
        {
            const double someRatio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, someRatio);

            const string playerSequence = "5";
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(currentSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void _PostFlopPlayerSequenceContainsNumberVaryActionType_PlayerSequenceIsNotChanged(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes actionType)
        {
            const double someRatio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, someRatio);

            const string playerSequence = "5";
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(playerSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(_convertedPlayer.Sequence[(int)street], Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void
            _PostFlopPlayerSequenceDoesNotContainNumberBet_CurrentSequenceAndNormalizedRatioIsAppendedToPlayerSequence(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const ActionTypes actionType = ActionTypes.B;
            const double ratio = 0.54;
            const string normalizedRatio = "5";
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = "someSequence";
            string expectedSequence = String.Concat(playerSequence, currentSequence, normalizedRatio);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(_convertedPlayer.Sequence[(int)street], Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void
            _PostFlopPlayerSequenceDoesNotContainNumberCheckFoldCallRaise_CurrentSequenceAndActionIsAppendedToPlayerSequence(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.R)] ActionTypes actionType, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const double ratio = 0.54;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);
            string currentSequence = "someSequence";

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string expectedSequence = String.Concat(playerSequence, currentSequence, actionType);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(_convertedPlayer.Sequence[(int)street], Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void _PostFlopVaryPlayerSequenceBet_NormalizedRatioIsAppendedToCurrentSequence(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const ActionTypes actionType = ActionTypes.B;
            const double ratio = 0.54;
            const string normalizedRatio = "5";
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Concat(currentSequence, normalizedRatio);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void _PostFlopVaryPlayerSequenceFoldCheckCall_CurrentSequenceIsNotChanged(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C)] ActionTypes actionType, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const double ratio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(currentSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void _PostFlopVaryPlayerSequenceRaise_IsAppendedToCurrentSequence(
            [Values(Streets.Flop, Streets.Turn, Streets.River)] Streets street, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const ActionTypes actionType = ActionTypes.R;
            const double ratio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Concat(currentSequence, actionType);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void _PreflopEmptyPlayerSequenceRaise_IsAppendedToCurrentSequence()
        {
            const Streets street = Streets.PreFlop;
            const ActionTypes actionType = ActionTypes.R;
            const double ratio = 2.1;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            string playerSequence = string.Empty;
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Concat(currentSequence, actionType);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        public void _PreflopNonEmptyPlayerSequenceRaise_CurrentSequenceIsNotChanged()
        {
            const Streets street = Streets.PreFlop;
            const ActionTypes actionType = ActionTypes.R;
            const double ratio = 2.1;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            const string playerSequence = "nonEmpty";
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(currentSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        [Test]
        [Sequential]
        public void _PreflopNonEmptyPlayerSequenceVaryActionType_IsSetToCurrentSequenceAppendedWithActionType(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes actionType)
        {
            const Streets street = Streets.PreFlop;
            const double someRatio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, someRatio);

            string playerSequence = string.Empty;
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Concat(playerSequence, actionType);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(_convertedPlayer.Sequence[(int)street], Is.EqualTo(expectedSequence));
        }

        [Test]
        [Sequential]
        public void _PreflopNonEmptyPlayerSequenceVaryActionType_PlayerSequenceIsNotChanged(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C, ActionTypes.B, ActionTypes.R)] ActionTypes actionType)
        {
            const Streets street = Streets.PreFlop;
            const double someRatio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, someRatio);

            const string playerSequence = "nonEmpty";
            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(playerSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(_convertedPlayer.Sequence[(int)street], Is.EqualTo(expectedSequence));
        }

        [Test]
        [Combinatorial]
        public void _PreflopVaryPlayerSequenceFoldCheckCall_CurrentSequenceIsNotChanged(
            [Values(ActionTypes.F, ActionTypes.X, ActionTypes.C)] ActionTypes actionType, 
            [Values("", "nonEmpty")] string playerSequence)
        {
            const Streets street = Streets.PreFlop;
            const double ratio = 1.0;
            var convertedAction = new ConvertedPokerAction(actionType, ratio);

            _convertedPlayer.Sequence[(int)street] = playerSequence;
            string currentSequence = string.Empty;
            string expectedSequence = String.Copy(currentSequence);

            _convertedPlayer.SetActionSequence(ref currentSequence, convertedAction, street);

            Assert.That(currentSequence, Is.EqualTo(expectedSequence));
        }

        #endregion
    }
}