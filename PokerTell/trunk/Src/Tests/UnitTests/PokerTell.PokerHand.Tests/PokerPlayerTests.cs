namespace PokerTell.PokerHand.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class PokerPlayerTests
    {
        [Test]
        public void IsEqualTo_AreEqual_ReturnsTrue()
        {
            var player1 = new PokerPlayer { Name = "player1" };
            var player2 = new PokerPlayer { Name = "player1" };

            Assert.That(player1, Is.EqualTo(player2));
        }

        [Test]
        public void IsEqualTo_AreNotEqual_ReturnsFalse()
        {
            var player1 = new PokerPlayer { Name = "player1" };
            var player2 = new PokerPlayer { Name = "player2" };

            Assert.That(player1, Is.Not.EqualTo(player2));
        }
    }
}