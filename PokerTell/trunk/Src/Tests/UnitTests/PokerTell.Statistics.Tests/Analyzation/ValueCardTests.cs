namespace PokerTell.Statistics.Tests.Analyzation
{
    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    public class ValueCardTests
    {
        ValuedCard _sut;

        [SetUp]
        public void A_Init()
        {
            _sut = new ValuedCard("As");
        }

        [Test]
        public void Constructor_CardIs_As_RankIs_A()
        {
            _sut.Rank.ShouldBeEqualTo(CardRank.A);
        }

        [Test]
        public void Constructor_CardIs_As_SuitIs_s()
        {
            _sut.Suit.ShouldBeEqualTo('s');
        }

        [Test]
        public void Constructor_CardIs_As_ValueIs_Ten()
        {
            _sut.Value.ShouldBeEqualTo(10.0);
        }
    }
}