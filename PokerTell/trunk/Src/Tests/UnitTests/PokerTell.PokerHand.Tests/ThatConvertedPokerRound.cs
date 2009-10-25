namespace PokerTell.PokerHand.Tests
{
    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;

    using UnitTests.Tools;

    public class ThatConvertedPokerRound
    {
        const bool WriteXmlToConsole = false;
        
        [Test]
        public void Deserialize_SerializedEmptyRound_ReturnsSameRound()
        {
            var round = new ConvertedPokerRound();
            Assert.That(round.DeserializedInMemory(), Is.EqualTo(round));
        }

        [Test]
        public void Deserialize_SerializedRoundWithOneAction_ReturnsSameRound()
        {
            var round = (ConvertedPokerRound)
                        new ConvertedPokerRound()
                            .Add(new ConvertedPokerAction(ActionTypes.C, 1.0));

            Assert.That(round.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(round));
        }

        [Test]
        public void Deserialize_SerializedRoundWithTwoActions_ReturnsSameRound()
        {
            var round = (ConvertedPokerRound)
                        new ConvertedPokerRound()
                            .Add(new ConvertedPokerAction(ActionTypes.C, 1.0))
                            .Add(new ConvertedPokerAction(ActionTypes.R, 2.0));

            Assert.That(round.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(round));
        }
    }
}