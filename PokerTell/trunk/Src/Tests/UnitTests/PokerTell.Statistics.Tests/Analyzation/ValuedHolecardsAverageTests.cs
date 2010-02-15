namespace PokerTell.Statistics.Tests.Analyzation
{
    using System.Collections.Generic;

    using Interfaces;

    using Moq;

    using NUnit.Framework;

    using Statistics.Analyzation;

    using UnitTests.Tools;

    public class ValuedHolecardsAverageTests
    {
        ValuedHoleCardsAverage _sut;

        [SetUp]
        public void A_Init()
        {
            var stub = new StubBuilder();
            var valuedHoleCards1 = stub.Setup<IValuedHoleCards>()
                .Get(vhc => vhc.ChenValue).Returns(1)
                .Get(vhc => vhc.SklanskyMalmuthGrouping).Returns(1).Out;
            var valuedHoleCards2 = stub.Setup<IValuedHoleCards>()
                .Get(vhc => vhc.ChenValue).Returns(2)
                .Get(vhc => vhc.SklanskyMalmuthGrouping).Returns(3).Out;

            _sut = new ValuedHoleCardsAverage();
            _sut.InitializeWith(new []{valuedHoleCards1, valuedHoleCards2 });
        }

        [Test]
        public void ConstructWith_TwoValuedHoleCardsThatHaveChenValuesOneAndTwo_ChenValueIsOnePointFiveRoundedUpToTwo()
        {
            _sut.ChenValue.ShouldBeEqualTo(2);
        }

        [Test]
        public void ConstructWith_TwoValuedHoleCardsThatHaveSklanskGroupingssOneAndThree_SklanskyGroupingIsTwo()
        {
            _sut.SklanskyMalmuthGrouping.ShouldBeEqualTo(2);
        }
    }
}