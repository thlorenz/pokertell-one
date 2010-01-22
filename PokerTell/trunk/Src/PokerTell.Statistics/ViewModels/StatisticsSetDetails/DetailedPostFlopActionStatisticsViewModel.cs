namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF;

    public class DetailedPostFlopActionStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields


        #endregion

        #region Constructors and Destructors

        public DetailedPostFlopActionStatisticsViewModel()
            : base("Bet Size")
        {
        }

        #endregion

        #region Properties


        #endregion

        public override IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
        {
            var betRow =
                new DetailedStatisticsRowViewModel("Bet", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
            var countRow =
                new DetailedStatisticsRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

            Rows = new List<IDetailedStatisticsRowViewModel>(new[] { betRow, countRow });

            DetailedStatisticsDescription =
                string.Format(
                            "Player {0} {1} on the {2} {3} position",
                            statisticsSet.PlayerName,
                            statisticsSet.ActionSequence,
                            statisticsSet.Street,
                            statisticsSet.InPosition ? "in" : "out of");
            return this;
        }
    }
}