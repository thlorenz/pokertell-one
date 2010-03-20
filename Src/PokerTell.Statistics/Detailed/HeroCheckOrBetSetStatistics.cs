namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    public class HeroCheckOrBetSetStatistics : ActionSequenceStatisticsSet
    {
        #region Constructors and Destructors

        public HeroCheckOrBetSetStatistics(
            IPercentagesCalculator percentagesCalculator,
            IEnumerable<IActionSequenceStatistic> statistics,
            string playerName,
            Streets street,
            bool inPosition)
            : base(percentagesCalculator, statistics, playerName, street, ActionSequences.HeroActs, inPosition)
        {
        }

        #endregion

        #region Methods

        protected override void CalculateIndividualPercentages()
        {
            IActionSequenceStatistic heroBStatistic = (from statistic in ActionSequenceStatistics
                                                       where statistic._actionSequence == ActionSequences.HeroB
                                                       select statistic)
                .Single();

            Func<int> getNumberOfRows = () => 1;

            Func<int, int> getNumberOfColumnsAtRow = row => heroBStatistic.MatchingPlayers.Length;

            Func<int, int, int> getCountAtRowColumn =
                (row, col) => heroBStatistic.MatchingPlayers[col].Count;

            Action<int, int, int> setPercentageAtRowColumn =
                (row, col, percentage) => heroBStatistic.Percentages[col] = percentage;

            _percentagesCalculator.CalculatePercentages(
                getNumberOfRows,
                getNumberOfColumnsAtRow,
                getCountAtRowColumn,
                setPercentageAtRowColumn);
        }

        #endregion
    }
}