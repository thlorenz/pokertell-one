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
            IRepositoryHandBrowserViewModel handBrowserViewModel,
            IPostFlopHeroActsRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel,
            IDetailedPostFlopHeroActsStatisticsDescriber detailedStatisticsDescriber)
            : base(handBrowserViewModel, detailedStatisticsDescriber, "Bet Size")
        {
            _raiseReactionStatisticsViewModel = raiseReactionStatisticsViewModel;

            MayBrowseHands = true;
            MayInvestigateHoleCards = false;
            MayInvestigateRaise = true;
        }

        public ICommand InvestigateRaiseReactionCommand
        {
            get
            {
                return _investigateRaiseReactionCommand ?? (_investigateRaiseReactionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            SaveSelectedCells();
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
                        CanExecuteDelegate = arg => SelectedCells.Count() > 0 && SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

        protected override IDetailedStatisticsViewModel CreateTableFor(IActionSequenceStatisticsSet statisticsSet)
        {
            var betRow =
                new StatisticsTableRowViewModel("Bet", statisticsSet.ActionSequenceStatistics.Last().Percentages, "%");
            var countRow =
                new StatisticsTableRowViewModel("Count", statisticsSet.SumOfCountsByColumn, string.Empty);

            Rows = new List<IStatisticsTableRowViewModel>(new[] { betRow, countRow });

            return this;
        }

        public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
            return SelectedCells.SelectMany(
               selectedCell => {
                  int col = selectedCell.Second;
                  return ActionSequenceStatisticsSet.ActionSequenceStatistics.Last().MatchingPlayers[col];
               });
            }
        }

        public override ActionSequences SelectedActionSequence
        {
            get
            {
                return ActionSequences.HeroB;
            }
        }

    }
}