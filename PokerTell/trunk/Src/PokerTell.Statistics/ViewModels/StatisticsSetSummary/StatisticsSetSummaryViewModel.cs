namespace PokerTell.Statistics.ViewModels.StatisticsSetSummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using Tools.WPF.ViewModels;

    public class StatisticsSetSummaryViewModel : NotifyPropertyChanged, IStatisticsSetSummaryViewModel
    {
        public IList<IStatisticsSetSummaryRowViewModel> Rows { get;  private set; }

        public StatisticsSetSummaryViewModel()
        {
            Rows = new List<IStatisticsSetSummaryRowViewModel>();
        }

        public IStatisticsSetSummaryViewModel UpdateWith(IActionSequenceStatisticsSet statisticsSet)
        {
            if (statisticsSet.ActionSequenceStatistics.Count() < 1)
            {
                throw new ArgumentException("ActionSequenceStatistics cannot have count zero", "statisticsSet");
            }

            for (int row = 0; row < statisticsSet.ActionSequenceStatistics.Count(); row++)
            {
                var statistic = statisticsSet.ActionSequenceStatistics.ElementAt(row);

                if (Rows.Count < row + 1)
                {
                    Rows.Add(new StatisticsSetSummaryRowViewModel(statistic.ActionSequence, new BarGraphViewModel()));
                }

                Rows[row].UpdateWith(statisticsSet.CumulativePercentagesByRow[row], statistic.Percentages);
            }
            
            return this;
        }
    }
}