namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    public class ActionSequenceStatisticsSet : IActionSequenceStatisticsSet
    {
        protected readonly IEnumerable<IActionSequenceStatistic> _statistics;

        protected readonly IPercentagesCalculator _percentagesCalculator;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

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

        public virtual IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics
        {
            get { return _statistics; }
        }

        public virtual int[] SumOfCountsByColumn
        {
            get { return _percentagesCalculator.SumsOfCountsByColumn; }
        }

        public int[] TotalCounts
        {
            get { return (from statistic in _statistics select statistic.TotalCounts).ToArray(); }
        }

        public int[] CumulativePercentagesByRow { get; private set; }

        public ActionSequenceStatisticsSet(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            _statistics = statistics;
            _percentagesCalculator = percentagesCalculator;
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

        protected virtual void CalculateCumulativePercentages()
        {
            CumulativePercentagesByRow = new int[_statistics.Count()];
            var sumOfTotalCounts = (from statistic in _statistics select statistic.TotalCounts).Sum();
            
            for (int row = 0; row < CumulativePercentagesByRow.Length; row++)
            {
                double percentage = (double)_statistics.ElementAt(row).TotalCounts / sumOfTotalCounts * 100;
                double roundedPercentage = Math.Round(percentage, MidpointRounding.AwayFromZero);
                CumulativePercentagesByRow[row] = (int) roundedPercentage;  
            }
        }

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
    }
}