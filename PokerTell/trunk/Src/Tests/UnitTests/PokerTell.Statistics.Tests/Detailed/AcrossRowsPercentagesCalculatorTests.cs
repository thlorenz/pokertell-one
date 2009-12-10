namespace PokerTell.Statistics.Tests.Detailed
{
    using System;

    using NUnit.Framework;

    using PokerTell.Statistics.Detailed;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.UnitTests;
    using PokerTell.UnitTests.Tools;

    [TestFixture]
    public class AcrossRowsPercentagesCalculatorTests : TestWithLog
    {
        #region Constants and Fields

        readonly Action<int, int, int> _noOp = delegate { };

        IPercentagesCalculator _sut;

        #endregion

        #region Public Methods

        [SetUp]
        public void _Init()
        {
            _sut = new AcrossRowsPercentagesCalculator();
        }

        [Test]
        public void CalculatePercenages_OneRow_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 1, null, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercenages_TwoRowsWithDifferentColumnCount_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 2, row => row == 0 ? 1 : 2, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercenages_TwoRowsWithSameColumnCount_DoesntThrowException()
        {
            TestDelegate calculate = () =>
                                     _sut.CalculatePercentages(() => 2, row => 1, (row, col) => 0, _noOp);
            Assert.DoesNotThrow(calculate);
        }

        [Test]
        public void CalculatePercenages_TwoRowsWithZeroColumns_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 2, row => 0, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercenages_ZeroRows_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 0, null, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercentages_ThreeRows_Row0Col0Count0_Row1Col0Count0_Row2Col0Count0_Result_AllRowsCol0Perc33
            ()
        {
            var counts = new[,] { { 1 }, { 1 }, { 1 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0), 
                                     row => counts.GetLength(1), 
                                     (row, col) => counts[row, col], 
                                     (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].IsEqualTo(33);
            percentages[1, 0].IsEqualTo(33);
            percentages[2, 0].IsEqualTo(33);
        }

        [Test]
        public void CalculatePercentages_TwoRows_Row0Col0Count0_Row1Col0Count0_Result_Row0Col0Perc0_Row1Col0Perc0()
        {
            var counts = new[,] { { 0 }, { 0 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0), 
                                     row => counts.GetLength(1), 
                                     (row, col) => counts[row, col], 
                                     (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].IsEqualTo(0);
            percentages[1, 0].IsEqualTo(0);
        }

        [Test]
        public void CalculatePercentages_TwoRows_Row0Col0Count1_Row1Col0Count0_Result_Row0Col0Perc100_Row1Col0Perc0()
        {
            var counts = new[,] { { 1 }, { 0 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0), 
                                     row => counts.GetLength(1), 
                                     (row, col) => counts[row, col], 
                                     (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].IsEqualTo(100);
            percentages[1, 0].IsEqualTo(0);
        }

        [Test]
        public void CalculatePercentages_TwoRows_Row0Col0Count1_Row1Col0Count1_Result_Row0Col0Perc50_Row1Col0Perc50()
        {
            var counts = new[,] { { 1 }, { 1 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0), 
                                     row => counts.GetLength(1), 
                                     (row, col) => counts[row, col], 
                                     (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].IsEqualTo(50);
            percentages[1, 0].IsEqualTo(50);
        }

        [Test]
        public void CalculatePercentages_TwoRowsTwoColumns_CalculatesCorrectPercentages()
        {
            var counts = new[,]
                {
                    { 1, 0 },
                    { 2, 1 }
                };

            var expect = new[,]
                {
                    { 33, 0 },
                    { 67, 100 }
                };

            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0), 
                                     row => counts.GetLength(1), 
                                     (row, col) => counts[row, col], 
                                     (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].IsEqualTo(expect[0, 0]);
            percentages[0, 1].IsEqualTo(expect[0, 1]);
            percentages[1, 0].IsEqualTo(expect[1, 0]);
            percentages[1, 1].IsEqualTo(expect[1, 1]);
        }

        #endregion
    }
}