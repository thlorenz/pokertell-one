namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Base;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.WPF;

    public class DetailedPostFlopReactionStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields

        ICommand _investigateCommand;

        #endregion

        #region Constructors and Destructors

        public DetailedPostFlopReactionStatisticsViewModel()
            : base("Bet Size")
        {

        }

        #endregion

        #region Properties

        public ICommand InvestigateCommand
        {
            get
            {
                return _investigateCommand ?? (_investigateCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            var sb = new StringBuilder();
                            sb.AppendLine("Investigating: ");
                            SelectedCells.ToList().ForEach(coord => sb.Append(coord + "; "));
                            Console.WriteLine(sb);
                        },
                        CanExecuteDelegate = _ => SelectedCells.FirstOrDefault(tuple => tuple.First != (int)ActionTypes.F) != null
                    });
            }
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
                            "Player {0} {1} on the {2} {3} position",
                            statisticsSet.PlayerName,
                            statisticsSet.ActionSequence,
                            statisticsSet.Street,
                            statisticsSet.InPosition ? "in" : "out of");
            return this;
        }
    }
}