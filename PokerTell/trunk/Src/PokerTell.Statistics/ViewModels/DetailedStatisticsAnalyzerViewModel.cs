namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using StatisticsSetDetails;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class DetailedStatisticsAnalyzerViewModel : NotifyPropertyChanged, IDetailedStatisticsAnalyzerViewModel
    {
        #region Constants and Fields

        ICommand _navigateBackwardCommand;

        ICommand _navigateForwardCommand;

        #endregion

        #region Constructors and Destructors

        public DetailedStatisticsAnalyzerViewModel()
        {
            ViewModelHistory = new List<IDetailedStatisticsViewModel>();
        }



        #endregion

        #region Properties

        public IDetailedStatisticsViewModel CurrentViewModel { get; set; }

        public ICommand NavigateBackwardCommand
        {
            get
            {
                return _navigateBackwardCommand ?? (_navigateBackwardCommand = new SimpleCommand
                    {
                        ExecuteDelegate =
                            xx => NavigateTo(ViewModelHistory.IndexOf(CurrentViewModel) - 1),
                        CanExecuteDelegate = xx => ViewModelHistory.IndexOf(CurrentViewModel) > 0
                    });
            }
        }

        public ICommand NavigateForwardCommand
        {
            get
            {
                return _navigateForwardCommand ?? (_navigateForwardCommand = new SimpleCommand
                    {
                        ExecuteDelegate =
                            xx => NavigateTo(ViewModelHistory.IndexOf(CurrentViewModel) + 1),
                        CanExecuteDelegate =
                            xx => ViewModelHistory.IndexOf(CurrentViewModel) < ViewModelHistory.Count - 1
                    });
            }
        }

        public IList<IDetailedStatisticsViewModel> ViewModelHistory { get; private set; }

        public bool Visible
        {
            get { return ViewModelHistory.Count > 0; }
        }

        #endregion

        #region Public Methods

        public IDetailedStatisticsAnalyzerViewModel AddViewModel(IDetailedStatisticsViewModel viewModel)
        {
            RemoveAllViewModelsInHistoryThatAreBehindCurrentViewModel();

            ViewModelHistory.Add(viewModel);

            CurrentViewModel = viewModel;

            viewModel.ChildViewModelChanged += vm => AddViewModel(vm);

            return this;
        }

        public IDetailedStatisticsAnalyzerViewModel NavigateTo(int index)
        {
            if (index > -1 && index < ViewModelHistory.Count)
            {
                CurrentViewModel = ViewModelHistory[index];
                RaisePropertyChanged(() => CurrentViewModel);
            }
            return this;
        }

        public IDetailedStatisticsAnalyzerViewModel InitializeWith(IActionSequenceStatisticsSet actionSequenceStatisticsSet)
        {
            Predicate<IActionSequenceStatisticsSet> isPreflop = s => s.Street == Streets.PreFlop;
            Predicate<IActionSequenceStatisticsSet> isHeroActs = s => s.ActionSequence == ActionSequences.HeroActs;
            Predicate<IActionSequenceStatisticsSet> isOppB = s => s.ActionSequence == ActionSequences.OppB;
            Predicate<IActionSequenceStatisticsSet> isHeroXOppB = s => s.ActionSequence == ActionSequences.HeroXOppB;

            actionSequenceStatisticsSet.Match()
                .With(isPreflop, _ => CurrentViewModel = new DetailedPreFlopStatisticsViewModel())
                .With(isHeroActs, _ => CurrentViewModel = new DetailedPostFlopActionStatisticsViewModel())
                .With(isOppB, _ => CurrentViewModel = new DetailedPostFlopReactionStatisticsViewModel())
                .With(isHeroXOppB, _ => CurrentViewModel = new DetailedPostFlopReactionStatisticsViewModel())
                .Do();
          
            return this;
        }

        #endregion

        #region Methods

        void RemoveAllViewModelsInHistoryThatAreBehindCurrentViewModel()
        {
            int indexOfCurrentView = ViewModelHistory.IndexOf(CurrentViewModel);

            if (indexOfCurrentView > -1 && indexOfCurrentView < ViewModelHistory.Count - 1)
            {
                int removalIndex = indexOfCurrentView + 1;
                while (ViewModelHistory.Count > removalIndex)
                {
                    ViewModelHistory.RemoveAt(removalIndex);
                }
            }
        }

        #endregion
    }
}