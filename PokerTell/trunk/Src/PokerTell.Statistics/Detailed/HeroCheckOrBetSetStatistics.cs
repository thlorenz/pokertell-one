namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;

    using Interfaces;

    public class HeroCheckOrBetSetStatistics : ActionSequenceSetStatistics
    {
        public HeroCheckOrBetSetStatistics(IEnumerable<IActionSequenceStatistic> statistics, IPercentagesCalculator percentagesCalculator)
            : base(statistics, percentagesCalculator)
        {
        }

        protected override void CalculatePercentages()
        {
            var heroBStatistic = (from statistic in _statistics
                                 where statistic.ActionSequence == ActionSequences.HeroB
                                 select statistic)
                                 .Single();

            Func<int> getNumberOfRows = () => 1;

            Func<int, int> getNumberOfColumnsAtRow = row => heroBStatistic.MatchingPlayers.Length;

            Func<int, int, int> getCountAtRowColumn =
                (row, col) => heroBStatistic.MatchingPlayers[col].Count;

            Action<int, int, int> setPercentageAtRowColumn =
                (row, col, percentage) => heroBStatistic.Percentages[col] = percentage;

            _percentagesCalculator.CalculatePercentages(getNumberOfRows,
                                                        getNumberOfColumnsAtRow,
                                                        getCountAtRowColumn,
                                                        setPercentageAtRowColumn);
        }
    }
}