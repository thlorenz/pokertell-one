namespace PokerTell.PokerHand.Tests
{
    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;
    using UnitTests.Tools;

    public class ThatConvertedPokerAction
    {
        const bool WriteXmlToConsole = false;
        
        [Test]
        public void Deserialize_SerializedLegalAction_ReturnsSerializedAction()
        {
            var legalAction = new ConvertedPokerAction(ActionTypes.B, 2.0);
            Assert.That(legalAction.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(legalAction));
        }

        [Test]
        public void Deserialize_SerializedIllegalAction_ReturnsSerializedAction()
        {
            var illegalAction = new ConvertedPokerAction(ActionTypes.E, 1.0);
            Assert.That(illegalAction.DeserializedInMemory(WriteXmlToConsole), Is.EqualTo(illegalAction));
        }
    }
}