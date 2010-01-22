namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Infrastructure.Interfaces.Statistics;

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

        public override IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
        {
            var foldRow = 
                new DetailedStatisticsRowViewModel("Fold", statisticsSet.ActionSequenceStatistics.First().Percentages, "%");
            var callRow =
                new DetailedStatisticsRowViewModel("Call", statisticsSet.ActionSequenceStatistics.ElementAt(1).Percentages, "%");
            var raiseRow =
                new DetailedStatisticsRowViewModel("Raise", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
            var countRow =
                new DetailedStatisticsRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

            Rows = new List<IDetailedStatisticsRowViewModel>(new[] { foldRow, callRow, raiseRow, countRow });

            DetailedStatisticsDescription =
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