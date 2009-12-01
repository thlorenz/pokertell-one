namespace PokerTell.PokerHand.Tests.Analyzation
{
    using NUnit.Framework;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.UnitTests.Tools;

    [TestFixture]
    public class ConvertedPokerActionTests
    {
        #region Public Methods

        [Test]
        public void BinaryDeserialize_SerializedIllegalAction_ReturnsSerializedAction()
        {
            var illegalAction = new ConvertedPokerAction(ActionTypes.E, 1.0);
            Assert.That(illegalAction.BinaryDeserializedInMemory(), Is.EqualTo(illegalAction));
        }

        [Test]
        public void BinaryDeserialize_SerializedLegalAction_ReturnsSerializedAction()
        {
            var legalAction = new ConvertedPokerAction(ActionTypes.B, 2.0);
            Assert.That(legalAction.BinaryDeserializedInMemory(), Is.EqualTo(legalAction));
        }

        [Test]
        public void XmlDeserialize_SerializedIllegalAction_ReturnsSerializedAction()
        {
            var illegalAction = new ConvertedPokerAction(ActionTypes.E, 1.0);
            Assert.That(illegalAction.XmlDeserializedInMemory(), Is.EqualTo(illegalAction));
        }

        [Test]
        public void XmlDeserialize_SerializedLegalAction_ReturnsSerializedAction()
        {
            var legalAction = new ConvertedPokerAction(ActionTypes.B, 2.0);
            Assert.That(legalAction.XmlDeserializedInMemory(), Is.EqualTo(legalAction));
        }

        #endregion
    }
}