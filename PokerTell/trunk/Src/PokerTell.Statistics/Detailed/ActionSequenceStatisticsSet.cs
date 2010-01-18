namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;

    public class ActionSequenceStatisticsSet : IActionSequenceStatisticsSet
    {
        #region Constants and Fields

        protected readonly IPercentagesCalculator _percentagesCalculator;

        protected readonly IEnumerable<IActionSequenceStatistic> _statistics;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        #endregion

        #region Constructors and Destructors

        public ActionSequenceStatisticsSet(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            _statistics = statistics;
            _percentagesCalculator = percentagesCalculator;
        }

        #endregion

        #region Properties

        public virtual IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics
        {
            get { return _statistics; }
        }

        public int[] CumulativePercentagesByRow { get; private set; }

        public virtual int[] SumOfCountsByColumn
        {
            get { return _percentagesCalculator.SumsOfCountsByColumn; }
        }

        public int[] TotalCounts
        {
            get { return (from statistic in _statistics select statistic.TotalCounts).ToArray(); }
        }

        #endregion

        #region Implemented Interfaces

        #region IActionSequenceStatisticsSet

        public IActionSequenceStatisticsSet UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            _analyzablePokerPlayers = analyzablePokerPlayers;

            foreach (var statistic in _statistics)
            {
                statistic.UpdateWith(_analyzablePokerPlayers);
            }

            CalculateIndividualPercentages();
            CalculateCumulativePercentages();

            return this;
        }

        #endregion

        #region IFluentInterface

        public override string ToString()
        {
            var sb = new StringBuilder();
            _statistics.ToList().ForEach(s => sb.AppendLine(s.ToString()));
            sb.Append("Sum of counts by column: ");
            SumOfCountsByColumn.ToList().ForEach(sumOfCounts => sb.Append(sumOfCounts.ToString() + ", "));
            sb.AppendLine().Append("CumulativePercentages: ");
            CumulativePercentagesByRow.ToList().ForEach(perc => sb.Append(perc + "%, "));
            sb.AppendLine();
            return sb.ToString();
        }

        #endregion

        #endregion

        #region Methods

        protected virtual void CalculateCumulativePercentages()
        {
            CumulativePercentagesByRow = new int[_statistics.Count()];
            var sumOfTotalCounts = (from statistic in _statistics select statistic.TotalCounts).Sum();

            for (int row = 0; row < CumulativePercentagesByRow.Length; row++)
            {
                if (_statistics.ElementAt(row).TotalCounts == 0)
                {
                    CumulativePercentagesByRow[row] = 0;
                }
                else
                {
                    double percentage = (double)_statistics.ElementAt(row).TotalCounts / sumOfTotalCounts * 100;
                    double roundedPercentage = Math.Round(percentage, MidpointRounding.AwayFromZero);
                    CumulativePercentagesByRow[row] = (int)roundedPercentage;
                }
            }
        }

        protected virtual void CalculateIndividualPercentages()
        {
            Func<int> getNumberOfRows = () => _statistics.Count();

            Func<int, int> getNumberOfColumnsAtRow = row => _statistics.ElementAt(row).MatchingPlayers.Length;

            Func<int, int, int> getCountAtRowColumn =
                (row, col) => _statistics.ElementAt(row).MatchingPlayers[col].Count;

            Action<int, int, int> setPercentageAtRowColumn =
                (row, col, percentage) => _statistics.ElementAt(row).Percentages[col] = percentage;

            _percentagesCalculator.CalculatePercentages(getNumberOfRows, 
                                                        getNumberOfColumnsAtRow, 
                                                        getCountAtRowColumn, 
                                                        setPercentageAtRowColumn);
        }

        #endregion
    }
}