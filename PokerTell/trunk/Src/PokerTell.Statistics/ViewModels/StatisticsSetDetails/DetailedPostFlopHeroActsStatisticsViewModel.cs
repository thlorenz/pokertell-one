namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Base;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class DetailedPostFlopHeroActsStatisticsViewModel : DetailedStatisticsViewModel
    {
        static readonly double[] BetSizeKeys = ApplicationProperties.BetSizeKeys;

        readonly IPostFlopHeroActsRaiseReactionStatisticsViewModel _raiseReactionStatisticsViewModel;

        ICommand _investigateRaiseReactionCommand;

        public DetailedPostFlopHeroActsStatisticsViewModel(
            IHandBrowserViewModel handBrowserViewModel, IPostFlopHeroActsRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel)
            : base(handBrowserViewModel, "Bet Size")
        {
            _raiseReactionStatisticsViewModel = raiseReactionStatisticsViewModel;
        }

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

                            ChildViewModel =
                                _raiseReactionStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers,
                                                                                 selectedBetSizes,
                                                                                 PlayerName,
                                                                                 SelectedActionSequence,
                                                                                 Street);
                        },
                        CanExecuteDelegate =
                            arg => SelectedCells.Count() > 0 && SelectedActionSequence == ActionSequences.HeroB
                    });
            }
        }

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(
            IActionSequenceStatisticsSet statisticsSet)
        {
            var betRow =
                new StatisticsTableRowViewModel("Bet", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
            var countRow =
                new StatisticsTableRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

            Rows = new List<IStatisticsTableRowViewModel>(new[] { betRow, countRow });

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