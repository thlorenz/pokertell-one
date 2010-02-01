namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Base;

    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.WPF;

    public class DetailedPreFlopStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        public DetailedPreFlopStatisticsViewModel()
            : base("Position")
        {
        }

        #endregion

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet)
        {
            var foldRow = 
                new StatisticsTableRowViewModel("Fold", statisticsSet.ActionSequenceStatistics.First().Percentages, "%");
            var callRow =
                new StatisticsTableRowViewModel("Call", statisticsSet.ActionSequenceStatistics.ElementAt(1).Percentages, "%");
            var raiseRow =
                new StatisticsTableRowViewModel("Raise", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
            var countRow =
                new StatisticsTableRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

            Rows = new List<IStatisticsTableRowViewModel>(new[] { foldRow, callRow, raiseRow, countRow });

            StatisticsDescription =
                string.Format(
                            "Player {0} {1} on the {2} in {3} pot",
                            statisticsSet.PlayerName,
                            statisticsSet.ActionSequence,
                            statisticsSet.Street,
                            statisticsSet.RaisedPot ? "a raised" : "an unraised");

            return this;
        }
    }
}