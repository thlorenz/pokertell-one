namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
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
            IRepositoryHandBrowserViewModel handBrowserViewModel, 
            IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel preFlopUnraisedPotCallingHandStrengthStatisticsViewModel, 
            IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel preFlopRaisedPotCallingHandStrengthStatisticsViewModel, 
            IPreFlopRaisingHandStrengthStatisticsViewModel preFlopRaisingHandStrengthStatisticsViewModel, 
            IPreFlopRaiseReactionStatisticsViewModel raiseReactionStatisticsViewModel, 
            IDetailedPreFlopStatisticsDescriber detailedStatisticsDescriber)
            : base(handBrowserViewModel, detailedStatisticsDescriber, "Position")
        {
            _preFlopUnraisedPotCallingHandStrengthStatisticsViewModel = preFlopUnraisedPotCallingHandStrengthStatisticsViewModel;
            _preFlopRaisedPotCallingHandStrengthStatisticsViewModel = preFlopRaisedPotCallingHandStrengthStatisticsViewModel;
            _preFlopRaisingHandStrengthStatisticsViewModel = preFlopRaisingHandStrengthStatisticsViewModel;
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
                        CanExecuteDelegate = arg =>
                            SelectedCells.Count() > 0 && ActionSequencesUtility.Raises.Contains(SelectedActionSequence) &&
                            SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

        ICommand _showCardsCommand;

        readonly IPreFlopRaisingHandStrengthStatisticsViewModel _preFlopRaisingHandStrengthStatisticsViewModel;

        readonly IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel _preFlopRaisedPotCallingHandStrengthStatisticsViewModel;

        readonly IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel _preFlopUnraisedPotCallingHandStrengthStatisticsViewModel;

        public ICommand InvestigateHoleCardsCommand
        {
            get
            {
                return _showCardsCommand ?? (_showCardsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            SaveSelectedCells();

                            switch (SelectedActionSequence)
                            {
                                case ActionSequences.HeroC: 
                                    _preFlopUnraisedPotCallingHandStrengthStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers, PlayerName, SelectedActionSequence);
                                    ChildViewModel = _preFlopUnraisedPotCallingHandStrengthStatisticsViewModel;
                                    break;
                                case ActionSequences.HeroR: 
                                case ActionSequences.OppRHeroR: 
                                    _preFlopRaisingHandStrengthStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers, PlayerName, SelectedActionSequence);
                                    ChildViewModel = _preFlopRaisingHandStrengthStatisticsViewModel;
                                    break;
                                case ActionSequences.OppRHeroC:
                                    _preFlopRaisedPotCallingHandStrengthStatisticsViewModel.InitializeWith(SelectedAnalyzablePlayers, PlayerName, SelectedActionSequence);
                                    ChildViewModel = _preFlopRaisedPotCallingHandStrengthStatisticsViewModel;
                                    break;

                                default: throw new ArgumentException("Cannot investigate holecards for this action sequence: " + SelectedActionSequence);
                            }
                        }, 
                        CanExecuteDelegate = arg => 
                            SelectedCells.Count() > 0 && 
                            ActionSequencesUtility.GetLastActionIn(SelectedActionSequence) != ActionTypes.F && 
                            SelectedAnalyzablePlayers.Count() > 0
                    });
            }
        }

    }
}