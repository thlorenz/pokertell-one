namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;

    using Tools.WPF;

    public class PreFlopRaisingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, 
                                                                 IPreFlopRaisingHandStrengthStatisticsViewModel
    {
        public PreFlopRaisingHandStrengthStatisticsViewModel(
            IPreFlopStartingHandsVisualizerViewModel startingHandsVisualizerViewModel, 
            IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, 
            IPreFlopRaisingHandStrengthDescriber preFlopRaisingHandStrengthDescriber)
            : base(startingHandsVisualizerViewModel, preFlopHandStrengthStatistics, preFlopRaisingHandStrengthDescriber, "Raise Sizes")
        {
        }
    }

    public class PreFlopRaisedPotCallingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, 
                                                                          IPreFlopRaisedPotCallingHandStrengthStatisticsViewModel
    {
        public PreFlopRaisedPotCallingHandStrengthStatisticsViewModel(
            IPreFlopStartingHandsVisualizerViewModel startingHandsVisualizerViewModel, 
            IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, 
            IPreFlopRaisedPotCallingHandStrengthDescriber preFlopRaisedPotCallingHandStrengthDescriber)
            : base(startingHandsVisualizerViewModel, preFlopHandStrengthStatistics, preFlopRaisedPotCallingHandStrengthDescriber, "Pot Odds")
        {
        }
    }

    public class PreFlopUnraisedPotCallingHandStrengthStatisticsViewModel : PreFlopHandStrengthStatisticsViewModel, 
                                                                            IPreFlopUnraisedPotCallingHandStrengthStatisticsViewModel
    {
        public PreFlopUnraisedPotCallingHandStrengthStatisticsViewModel(
            IPreFlopStartingHandsVisualizerViewModel startingHandsVisualizerViewModel, 
            IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, 
            IPreFlopUnraisedPotCallingHandStrengthDescriber preFlopUnraisedPotCallingHandStrengthDescriber)
            : base(startingHandsVisualizerViewModel, preFlopHandStrengthStatistics, preFlopUnraisedPotCallingHandStrengthDescriber, "Pot Odds")
        {
        }
    }

    public abstract class PreFlopHandStrengthStatisticsViewModel : StatisticsTableViewModel, IPreFlopHandStrengthStatisticsViewModel
    {
        readonly IPreFlopHandStrengthStatistics _preFlopHandStrengthStatistics;

        static readonly double[] UnraisedPotCallingRatios = ApplicationProperties.UnraisedPotCallingRatios;

        static readonly double[] RaisedPotCallingRatios = ApplicationProperties.RaisedPotCallingRatios;

        static readonly double[] RaiseSizeKeys = ApplicationProperties.RaiseSizeKeys;

        readonly IPreFlopHandStrengthDescriber _handStrengthDescriber;

        readonly IPreFlopStartingHandsVisualizerViewModel _startingHandsVisualizerViewModel;

        protected PreFlopHandStrengthStatisticsViewModel(
            IPreFlopStartingHandsVisualizerViewModel startingHandsVisualizerViewModel, 
            IPreFlopHandStrengthStatistics preFlopHandStrengthStatistics, 
            IPreFlopHandStrengthDescriber handStrengthDescriber, 
            string columnHeaderTitle)
            : base(columnHeaderTitle)
        {
            _startingHandsVisualizerViewModel = startingHandsVisualizerViewModel;
            _handStrengthDescriber = handStrengthDescriber;
            _preFlopHandStrengthStatistics = preFlopHandStrengthStatistics;
            _preFlopHandStrengthStatistics.InitializeWith(UnraisedPotCallingRatios, RaisedPotCallingRatios, RaiseSizeKeys);

            MayVisualizeHands = true;
        }

        public IPreFlopHandStrengthStatisticsViewModel InitializeWith(
            IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, string playerName, ActionSequences actionSequence)
        {
            _preFlopHandStrengthStatistics.BuildStatisticsFor(analyzablePokerPlayers, actionSequence);
            var kownCardsCount = _preFlopHandStrengthStatistics.KnownCards.Select(c => c.Count());

            Rows = new List<IStatisticsTableRowViewModel>
                {
                    new StatisticsTableRowViewModel("Chen", _preFlopHandStrengthStatistics.AverageChenValues, "(avg)"), 
                    new StatisticsTableRowViewModel("S&M", _preFlopHandStrengthStatistics.AverageSklanskyMalmuthGroupings, "(avg)"), 
                    new StatisticsTableRowViewModel("Counts", kownCardsCount, string.Empty), 
                };

            StatisticsDescription = _handStrengthDescriber.Describe(playerName, actionSequence);
            StatisticsHint = _handStrengthDescriber.Hint(playerName);
            return this;
        }

        ICommand _visualizeStartingHandsCommand;

        public ICommand VisualizeStartingHandsCommand
        {
            get
            {
                return _visualizeStartingHandsCommand ?? (_visualizeStartingHandsCommand = new SimpleCommand
                {
                    ExecuteDelegate = arg => { 
                        SaveSelectedCells();
                        _startingHandsVisualizerViewModel.Visualize(SelectedKnownCards);
                        ChildViewModel = _startingHandsVisualizerViewModel;
                    },
                    CanExecuteDelegate = arg => SelectedCells.Count > 0
                });
            }
        }

        IEnumerable<string> SelectedKnownCards
        {
            get
            {
                // Look in first Row only b/c we don't want to add any column twice.
                // This of course assumes, that the view will ensure that once a cell in a column i selected
                // all the other cells in the same column will be selected as well.
                // Essentially implementing a entire column selection mode.
                var selectedColumns = SelectedCells
                    .Where(c => c.First == 0)
                    .Select(c => c.Second);

                var selectedKnownCards =
                    selectedColumns.Aggregate(new List<IValuedHoleCards>(), 
                                              (list, col) => {
                                                  list.AddRange(_preFlopHandStrengthStatistics.KnownCards[col]);
                                                  return list;
                                              });

                return selectedKnownCards.Select(c => c.Name);
            }
        }
    }
}