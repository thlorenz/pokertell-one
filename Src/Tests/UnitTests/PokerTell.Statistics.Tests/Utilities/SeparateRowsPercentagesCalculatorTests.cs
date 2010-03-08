namespace PokerTell.Statistics.Tests.Utilities
{
    using System;

    using PokerTell.Statistics.Interfaces;

    using NUnit.Framework;

    using PokerTell.Statistics.Utilities;
    using PokerTell.UnitTests.Tools;

    public class SeparateRowsPercentagesCalculatorTests
    {
        #region Constants and Fields

        readonly Action<int, int, int> _noOp = delegate { };

        IPercentagesCalculator _sut;

        #endregion

        [SetUp]
        public void _Init()
        {
            _sut = new SeparateRowsPercentagesCalculator();
        }

        [Test]
        public void CalculatePercentages_ZeroRows_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 0, null, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercentages_TwoRowsWithDifferentColumnCount_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 2, row => row == 0 ? 1 : 2, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercentages_TwoRowsWithSameColumnCount_DoesntThrowException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 2, row => 1, (row, col) => 0, _noOp);
            Assert.DoesNotThrow(calculate);
        }

        [Test]
        public void CalculatePercentages_OneRowWithOneColumn_ThrowsArgumentException()
        {
            TestDelegate calculate = () => _sut.CalculatePercentages(() => 1, row => 0, null, null);
            Assert.Throws<ArgumentException>(calculate);
        }

        [Test]
        public void CalculatePercentages_OneRowTwoColumns_Col0Count0_Col1Count1_Result_Col0Perc0_Col1Perc100()
        {
            var counts = new[,] { { 0, 1 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].ShouldBeEqualTo(0);
            percentages[0, 1].ShouldBeEqualTo(100);
        }

        [Test]
        public void CalculatePercentages_OneRowThreeColumns_Col0Count0_Col1Count1__Col1Count3_Result_Col0Perc0_Col1Perc25_Col1Perc75()
        {
            var counts = new[,] { { 0, 1, 3 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].ShouldBeEqualTo(0);
            percentages[0, 1].ShouldBeEqualTo(25);
            percentages[0, 2].ShouldBeEqualTo(75);
        }

        [Test]
        public void CalculatePercentages_OneRowThreeColumns_Col0Count0_Col1Count1__Col1Count3_Result_Col0Sum0_Col1Sum1_Col2Sum3()
        {
            var counts = new[,] { { 0, 1, 3 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            _sut.SumsOfCountsByColumn[0].ShouldBeEqualTo(0);
            _sut.SumsOfCountsByColumn[1].ShouldBeEqualTo(1);
            _sut.SumsOfCountsByColumn[2].ShouldBeEqualTo(3);
        }

        [Test]
        public void CalculatePercentages_OneRowTwoColumns_Col0Count0_Col1Count1_Result_Col0Sum0_Col1Sum1()
        {
            var counts = new[,] { { 0, 1 } };
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            _sut.SumsOfCountsByColumn[0].ShouldBeEqualTo(0);
            _sut.SumsOfCountsByColumn[1].ShouldBeEqualTo(1);
        }

        [Test]
        public void CalculatePercentages_TwoRowsTwoColumns_CalculatesPercentagesCorrectly()
        {
            var counts = new[,]
                {
                    { 0, 1 },
                    { 1, 1 },
                };

            var expect = new[,]
                {
                    { 0, 100 },
                    { 50, 50 },
                };
    
            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            percentages[0, 0].ShouldBeEqualTo(expect[0, 0]);
            percentages[0, 1].ShouldBeEqualTo(expect[0, 1]);
            percentages[1, 0].ShouldBeEqualTo(expect[1, 0]);
            percentages[1, 1].ShouldBeEqualTo(expect[1, 1]);
        }

        [Test]
        public void CalculatePercentages_TwoRowsTwoColumns_CalculatesSumsCorrectly()
        {
            var counts = new[,]
                {
                    { 0, 1 },
                    { 1, 1 },
                };

            var percentages = new int[counts.GetLength(0), counts.GetLength(1)];

            _sut.CalculatePercentages(() => counts.GetLength(0),
                                      row => counts.GetLength(1),
                                      (row, col) => counts[row, col],
                                      (row, col, perc) => percentages[row, col] = perc);

            _sut.SumsOfCountsByColumn[0].ShouldBeEqualTo(1);
            _sut.SumsOfCountsByColumn[1].ShouldBeEqualTo(2);
        }

    }
}