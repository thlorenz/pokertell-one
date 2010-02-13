namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class DetailedPreFlopStatisticsViewModel : DetailedStatisticsViewModel
    {
        readonly IPreFlopRaiseReactionStatisticsViewModel _raiseReactionStatisticsViewModel;

        public DetailedPreFlopStatisticsViewModel(
            IHandBrowserViewModel handBrowserViewModel, 
            IPreFlopRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel, 
            IDetailedPreFlopStatisticsDescriber detailedStatisticsDescriber)
            : base(handBrowserViewModel, detailedStatisticsDescriber, "Position")
        {
            _raiseReactionStatisticsViewModel = raiseReactionStatisticsViewModel;

            MayBrowseHands = true;
            MayInvestigateHoleCards = true;
            MayInvestigateRaise = true;
        }

        protected override IDetailedStatisticsViewModel CreateTableFor(IActionSequenceStatisticsSet statisticsSet)
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

            return this;
        }

        ICommand _investigateRaiseReactionCommand;

        public ICommand InvestigateRaiseReactionCommand
        {
            get
            {
                return _investigateRaiseReactionCommand ?? (_investigateRaiseReactionCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            SaveSelectedCells();
                            Tuple<StrategicPositions, StrategicPositions> selectedPositions = Tuple.New(
                                StrategicPositionsUtility.GetAllPositionsInOrder().ElementAt(SelectedColumnsSpan.First), 
                                StrategicPositionsUtility.GetAllPositionsInOrder().ElementAt(SelectedColumnsSpan.Second));

                            ChildViewModel =
                                _raiseReactionStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers, 
                                                                                 selectedPositions, 
                                                                                 PlayerName, 
                                                                                 SelectedActionSequence, 
                                                                                 Street);
                        }, 
                        CanExecuteDelegate =
                            arg =>
                            SelectedCells.Count() > 0 && ActionSequencesUtility.Raises.Contains(SelectedActionSequence) &&
                            SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

        ICommand _showCardsCommand;

        public ICommand InvestigateHoleCardsCommand
        {
            get
            {
                return _showCardsCommand ?? (_showCardsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => { }, 
                        CanExecuteDelegate = arg => SelectedCells.Count() > 0 && SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }
    }
}