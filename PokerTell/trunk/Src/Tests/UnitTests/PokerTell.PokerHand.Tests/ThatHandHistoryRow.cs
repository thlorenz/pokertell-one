namespace PokerTell.PokerHand.Tests
{
    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using NUnit.Framework;

    using ViewModels;

    /// <summary>
    /// Summary description for ThatHandHistoryRow.
    /// </summary>
    [TestFixture]
    internal class ThatHandHistoryRow
    {
        /// <summary>
        /// The _poker player.
        /// </summary>
        private IConvertedPokerPlayer _pokerPlayer;

        /// <summary>
        /// The _hand history row.
        /// </summary>
        private HandHistoryRow _handHistoryRow;

        /// <summary>
        /// The init poker player.
        /// </summary>
        private void InitPokerPlayer()
        {
            _pokerPlayer = new ConvertedPokerPlayer("hero", 10, 5, 2, 9, "As Kd");
            _pokerPlayer.AddRound(new ConvertedPokerRound());
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.R, 3.4));
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.R, 2.4));
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));

            _pokerPlayer.AddRound(new ConvertedPokerRound());
            _pokerPlayer[Streets.Flop].Add(new ConvertedPokerAction(ActionTypes.B, 0.4));

            _handHistoryRow = new HandHistoryRow(_pokerPlayer);
        }

        /// <summary>
        /// The init poker player in small blind.
        /// </summary>
        private void InitPokerPlayerInSmallBlind()
        {
            _pokerPlayer = new ConvertedPokerPlayer("hero", 10, 5, 0, 9, "As Kd");
            _pokerPlayer.AddRound(new ConvertedPokerRound());
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.R, 3.4));
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.R, 2.4));
            _pokerPlayer[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));

            _handHistoryRow = new HandHistoryRow(_pokerPlayer);
        }

        /// <summary>
        /// The if player is in blinds_ preflop will be indented.
        /// </summary>
        [Test]
        public void IfPlayerIsInBlinds_PreflopWillBeIndented()
        {
            InitPokerPlayerInSmallBlind();

            const string Indent = "___     ";
            Assert.That(_handHistoryRow.Preflop.StartsWith(Indent));
        }

        /// <summary>
        /// The properties are assigned correctly from player model.
        /// </summary>
        [Test]
        public void PropertiesAreAssignedCorrectlyFromPlayerModel()
        {
            InitPokerPlayer();

            Assert.That(_handHistoryRow.Position, Is.EqualTo(_pokerPlayer.StrategicPosition.ToString()));
            Assert.That(_handHistoryRow.PlayerName, Is.EqualTo(_pokerPlayer.Name));
            Assert.That(_handHistoryRow.HoleCards.Rank1, Is.EqualTo("A"));
            Assert.That(_handHistoryRow.M, Is.EqualTo(_pokerPlayer.MBefore.ToString()));
            Assert.That(_handHistoryRow.Preflop, Is.EqualTo(_pokerPlayer[Streets.PreFlop].ToString()));
            Assert.That(_handHistoryRow.Flop, Is.EqualTo(_pokerPlayer[Streets.Flop].ToString()));
            Assert.That(_handHistoryRow.Turn, Is.EqualTo(string.Empty));
        }
    }
}