namespace PokerTell.PokerHand.Tests
{
    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;

    using UnitTests.Tools;

    public class ThatConvertedPokerRound
    {
        [Test]
        public void Deserialize_SerializedEmptyRound_ReturnsSameRound()
        {
            var round = new ConvertedPokerRound();
            Assert.That(round.DeserializedInMemory(), Is.EqualTo(round));
        }

        [Test]
        public void Deserialize_SerializedRoundWithOneEmptyAction_ReturnsSameRound()
        {
            var round = (ConvertedPokerRound)
                        new ConvertedPokerRound()
                            .Add(new ConvertedPokerAction(ActionTypes.C, 1.0));
                    
            Assert.That(round.DeserializedInMemory(), Is.EqualTo(round));
        }
    }
}