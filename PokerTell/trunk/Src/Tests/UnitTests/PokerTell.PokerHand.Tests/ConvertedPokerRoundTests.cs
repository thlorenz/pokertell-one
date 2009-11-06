namespace PokerTell.PokerHand.Tests
{
    using System;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;

    using UnitTests.Tools;

    public class ConvertedPokerRoundTests
    {
        [Test]
        public void BinaryDeserialize_SerializedEmptyRound_ReturnsSameRound()
        {
            var round = new ConvertedPokerRound();
            Assert.That(round.BinaryDeserializedInMemory(), Is.EqualTo(round));
        }

        [Test]
        public void BinaryDeserialize_SerializedRoundWithOneAction_ReturnsSameRound()
        {
            var round = (ConvertedPokerRound)
                        new ConvertedPokerRound()
                            .Add(new ConvertedPokerAction(ActionTypes.C, 1.0));

            Assert.That(round.BinaryDeserializedInMemory(), Is.EqualTo(round));
        }

        [Test]
        public void BinaryDeserialize_SerializedRoundWithTwoActions_ReturnsSameRound()
        {
            var round = (ConvertedPokerRound)
                        new ConvertedPokerRound()
                            .Add(new ConvertedPokerAction(ActionTypes.C, 1.0))
                            .Add(new ConvertedPokerAction(ActionTypes.R, 2.0));
            
            Assert.That(round.BinaryDeserializedInMemory(), Is.EqualTo(round));
        }

        [Test]
        public void IsEqualTo_AreEqual_ReturnsTrue()
        {
            var round1 = new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.F, 1.0));
            var round2 = new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.F, 1.0));

            Assert.That(round1, Is.EqualTo(round2));
        }

        [Test]
        public void IsEqualTo_AreNotEqual_ReturnsFalse()
        {
            var round1 = new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.F, 1.0));
            var round2 = new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.C, 1.0));

            Assert.That(round1, Is.Not.EqualTo(round2));
        }
    }
}