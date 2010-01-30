namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Analyzation;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class DetailedPostFlopActionStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields

        static readonly double[] BetSizeKeys = ApplicationProperties.BetSizeKeys;

        ICommand _investigateRaiseReactionCommand;

        #endregion

        #region Constructors and Destructors

        public DetailedPostFlopActionStatisticsViewModel()
            : base("Bet Size")
        {
            
        }

        #endregion

        #region Properties

        public ICommand InvestigateRaiseReactionCommand
        {
            get
            {
                return _investigateRaiseReactionCommand ?? (_investigateRaiseReactionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            Tuple<double, double> selectedBetSizes = Tuple.New(
                                BetSizeKeys[SelectedColumnsSpan.First],
                                BetSizeKeys[SelectedColumnsSpan.Second]);

                        //    ChildViewModel = new DetailedRaiseReactionStatisticsViewModel(raiseReactionStatisticsMock.Object, null, SelectedAnalyzablePlayers, PlayerName, Street, SelectedActionSequence, selectedBetSizes);
                        },
                        CanExecuteDelegate = arg => SelectedCells.Count() > 0 && SelectedActionSequence == ActionSequences.HeroB
                    });
            }
        }

        #endregion

        #region Methods

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet)
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

        void CreateRaiseReactionViewModel()
        {
        }

        #endregion
    }
}