namespace PokerTell.Statistics.Tests.Analyzation
{
    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    public class ValuedHoleCardsTests
    {
        ValuedHoleCards _sut;

        [SetUp]
        public void A_Init()
        {
            _sut = new ValuedHoleCards("As Ah");    
        }

        [Test]
        public void ConstructWith_As_Ah_ChenValueIsTwenty()
        {
            _sut.ChenValue.ShouldBeEqualTo(20);
        }

        [Test]
        public void ConstructWith_As_Ah_SklanskyMalmuthGroupingIsOne()
        {
            _sut.SklanskyMalmuthGrouping.ShouldBeEqualTo(1);
        }
    }
}