namespace PokerTell.Statistics.Tests.Analyzation
{
    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    public class ValuedHoleCardsTests
    {
        [Test]
        public void ConstructWith_As_Ah_ChenValueIsTwenty()
        {
            new ValuedHoleCards("As Ah")
                .ChenValue.ShouldBeEqualTo(20);
        }

        [Test]
        public void ConstructWith_As_Ah_SklanskyMalmuthGroupingIsOne()
        {
            new ValuedHoleCards("As Ah")
                .SklanskyMalmuthGrouping.ShouldBeEqualTo(1);
        }

        [Test]
        public void ConstructWith_As_Ks_AreSuitedIsTrue()
        {
            new ValuedHoleCards("As Ks")
                .AreSuited.ShouldBeTrue();
        }

        [Test]
        public void ConstructWith_As_Kh_AreSuitedIsFalse()
        {
            new ValuedHoleCards("As Kh")
                .AreSuited.ShouldBeFalse();
        }

        [Test]
        public void ConstructWith_Th_Qs_OrdersCardsDescending_SoFirstIs_Qs_SecondIs_Th()
        {
            var sut = new ValuedHoleCards("Th Qs");

            sut.ValuedCards.First.Rank.ShouldBeEqualTo(CardRank.Q);
            sut.ValuedCards.First.Suit.ShouldBeEqualTo('s');

            sut.ValuedCards.Second.Rank.ShouldBeEqualTo(CardRank.T);
            sut.ValuedCards.Second.Suit.ShouldBeEqualTo('h');
        }
    }
}