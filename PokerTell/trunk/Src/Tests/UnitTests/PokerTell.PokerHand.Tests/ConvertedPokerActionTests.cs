namespace PokerTell.PokerHand.Tests
{
    using System;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    using NUnit.Framework;
    using UnitTests.Tools;

    public class ConvertedPokerActionTests
    {
        [Test]
        public void BinaryDeserialize_SerializedLegalAction_ReturnsSerializedAction()
        {
            var legalAction = new ConvertedPokerAction(ActionTypes.B, 2.0);
            Assert.That(legalAction.BinaryDeserializedInMemory(), Is.EqualTo(legalAction));
        }

        [Test]
        public void BinaryDeserialize_SerializedIllegalAction_ReturnsSerializedAction()
        {
            var illegalAction = new ConvertedPokerAction(ActionTypes.E, 1.0);
            Assert.That(illegalAction.BinaryDeserializedInMemory(), Is.EqualTo(illegalAction));
        }

        [Test]
        public void XmlDeserialize_SerializedLegalAction_ReturnsSerializedAction()
        {
            var legalAction = new ConvertedPokerAction(ActionTypes.B, 2.0);
            Assert.That(legalAction.XmlDeserializedInMemory(), Is.EqualTo(legalAction));
        }

        [Test]
        public void XmlDeserialize_SerializedIllegalAction_ReturnsSerializedAction()
        {
            var illegalAction = new ConvertedPokerAction(ActionTypes.E, 1.0);
            Assert.That(illegalAction.XmlDeserializedInMemory(), Is.EqualTo(illegalAction));
        }
    
    }
}