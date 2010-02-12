namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure;
    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class DetailedPostFlopHeroReactsStatisticsViewModel : DetailedStatisticsViewModel
    {
        ICommand _investigateRaiseReactionCommand;


        static readonly double[] BetSizeKeys = ApplicationProperties.BetSizeKeys;

        readonly IPostFlopHeroReactsRaiseReactionStatisticsViewModel _raiseReactionStatisticsViewModel;

        public DetailedPostFlopHeroReactsStatisticsViewModel(
            IHandBrowserViewModel handBrowserViewModel, 
            IPostFlopHeroReactsRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel)
            : base(handBrowserViewModel, "Bet Size")
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
                            var betSizes = Tuple.New(BetSizeKeys[SelectedColumnsSpan.First], BetSizeKeys[SelectedColumnsSpan.Second]);

                            ChildViewModel =
                                _raiseReactionStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers,
                                                                                 betSizes,
                                                                                 PlayerName,
                                                                                 SelectedActionSequence,
                                                                                 Street);
                        }, 
                        CanExecuteDelegate = _ => SelectedCells.Count() > 0 && ActionSequencesUtility.Raises.Contains(SelectedActionSequence) && SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(
            IActionSequenceStatisticsSet statisticsSet)
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