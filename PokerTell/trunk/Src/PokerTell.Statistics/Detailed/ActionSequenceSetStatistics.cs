namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    public class ActionSequenceSetStatistics : IActionSequenceSetStatistics
    {
        readonly IEnumerable<IActionSequenceStatistic> _statistics;

        readonly IPercentagesCalculator _percentagesCalculator;

        IEnumerable<IAnalyzablePokerPlayer> _analyzablePokerPlayers;

        public IActionSequenceSetStatistics UpdateWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
        {
            _analyzablePokerPlayers = analyzablePokerPlayers;
           
            foreach (var statistic in _statistics)
            {
               statistic.UpdateWith(_analyzablePokerPlayers);
            }

            CalculatePercentages();
            return this;
        }

        public IEnumerable<IActionSequenceStatistic> ActionSequenceStatistics
        {
            get { return _statistics; }
        }

        public virtual int[] SumOfCountsByColumn
        {
            get { return _percentagesCalculator.SumOfCountsByColumn; }
        }

        public ActionSequenceSetStatistics(
            IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
        {
            _statistics = statistics;
            _percentagesCalculator = percentagesCalculator;
        }

        protected virtual void CalculatePercentages()
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
    }
}