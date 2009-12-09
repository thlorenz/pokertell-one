namespace PokerTell.Statistics.Detailed
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.Statistics.Interfaces;

    public class ActionSequenceSetAcrossRowsPercentagesCalculator : IActionSequenceSetPercentagesCalculator
    {
        readonly int _indexes;

        public ActionSequenceSetAcrossRowsPercentagesCalculator(int indexes)
        {
            _indexes = indexes;
        }

        #region Implemented Interfaces

        #region IActionSequenceSetPercentagesCalculator

        public IActionSequenceSetPercentagesCalculator CalculatePercenagesOf(
            IEnumerable<IActionSequenceStatistic> statistics)
        {
            if (statistics.Count() < 2)
            {
                throw new ArgumentException(
                    "Needs to contain at least two statistics but contained only " + statistics.Count(), "statistics");
            }

            var sums = new int[_indexes];
            for (int i = 0; i < _indexes; i++)
            {
                foreach (var statistic in statistics)
                {
                    sums[i] += statistic.MatchingPlayers[i].Count;
                }

                foreach (var statistic in statistics)
                {
                    if (sums[i] == 0)
                    {
                        statistic.Percentages[i] = 0;
                    }
                    else
                    {
                        statistic.Percentages[i] = (statistic.MatchingPlayers[i].Count * 100) / sums[i];
                    }
                }
            }

            return this;
        }

        #endregion

        #endregion
    }
}