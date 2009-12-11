namespace Tools.Tests.GenericRanges
{
    using System;

    using NUnit.Framework;

    using PokerTell.UnitTests;
    using PokerTell.UnitTests.Tools;

    using Tools.GenericRanges;
   
    [TestFixture]
    public class GenericRangeFilterTests : TestWithLog
    {
        GenericRangeFilter<int> _sut;

        [SetUp]
        public void _Init()
        {
            _sut = new GenericRangeFilter<int>();
        }

        [Test]
        public void DoesNotFilterOut_RangeIsNullAndIsActive_ThrowsNullReferenceException()
        {
            _sut.IsActive = true;
            Assert.Throws<NullReferenceException>(() => _sut.DoesNotFilterOut(0));
        }

        [Test]
        public void DoesNotFilterOut_RangeIsNullAndIsNotActive_DoesntThrowException()
        {
            _sut.IsActive = false;
            Assert.DoesNotThrow(() => _sut.DoesNotFilterOut(0));
        }

        [Test]
        public void DoesNotFilterOut_RangeIsNullAndIsNotActive_ReturnsTrue()
        {
            _sut.IsActive = false;
            _sut.DoesNotFilterOut(0).IsTrue();
        }

        [Test]
        public void DoesNotFilterOut_RangeIsNotNullAndIsActive_DoesntThrowException()
        {
            _sut.Range = new GenericRange<int>(0, 1);
            _sut.IsActive = true;

            Assert.DoesNotThrow(() => _sut.DoesNotFilterOut(0));
        }

        [Test]
        public void DoesNotFilterOut_FilterIsNotActiveAndValueNotInRange_ReturnsTrue()
        {
            _sut.Range = new GenericRange<int>(0, 1);
            
            _sut.IsActive = false;

            _sut.DoesNotFilterOut(2).IsTrue();
        }

        [Test]
        public void DoesNotFilterOut_FilterIsActiveAndValueNotInRange_ReturnsFalse()
        {
            _sut.Range = new GenericRange<int>(0, 1);

            _sut.IsActive = true;

            _sut.DoesNotFilterOut(2).IsFalse();
        }

        [Test]
        public void DoesNotFilterOut_FilterIsActiveAndValueIsInRange_ReturnsTrue()
        {
            _sut.Range = new GenericRange<int>(0, 1);

            _sut.IsActive = true;

            _sut.DoesNotFilterOut(1).IsTrue();
        }

        [Test]
        public void ActivateWith_Always_SetsIsActiveToTrue()
        {
            _sut.ActivateWith(0, 1);

            _sut.IsActive.IsTrue();
        }

        [Test]
        public void ActivateWith_Always_SetsRangeToRangeOfMinAndMax()
        {
            const int min = 0;
            const int max = 1;
            _sut.ActivateWith(min, max);

            _sut.Range.MinValue.IsEqualTo(min);
            _sut.Range.MaxValue.IsEqualTo(max);
        }
    }
}