namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.Statistics;

    using Tools.FunctionalCSharp;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class DetailedStatisticsAnalyzerViewModel : NotifyPropertyChanged, IDetailedStatisticsAnalyzerViewModel
    {
        #region Constants and Fields

        readonly IConstructor<IDetailedStatisticsViewModel> _detailedPostFlopActionStatisticsViewModelMake;

        readonly IConstructor<IDetailedStatisticsViewModel> _detailedPostFlopReactionStatisticsViewModelMake;

        readonly IConstructor<IDetailedStatisticsViewModel> _detailedPreFlopStatisticsViewModelMake;

        ICommand _navigateBackwardCommand;

        ICommand _navigateForwardCommand;

        #endregion

        #region Constructors and Destructors

       public DetailedStatisticsAnalyzerViewModel(
            IConstructor<IDetailedStatisticsViewModel> detailedPreFlopStatisticsViewModelMake,
            IConstructor<IDetailedStatisticsViewModel> detailedPostFlopActionStatisticsViewModelMake,
            IConstructor<IDetailedStatisticsViewModel> detailedPostFlopReactionStatisticsViewModelMake)
        {
            _detailedPostFlopReactionStatisticsViewModelMake = detailedPostFlopReactionStatisticsViewModelMake;
            _detailedPostFlopActionStatisticsViewModelMake = detailedPostFlopActionStatisticsViewModelMake;
            _detailedPreFlopStatisticsViewModelMake = detailedPreFlopStatisticsViewModelMake;

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
            get { return ViewModelHistory != null && ViewModelHistory.Count > 0; }
        }

        #endregion

        #region Implemented Interfaces

        #region IDetailedStatisticsAnalyzerViewModel

        public IDetailedStatisticsAnalyzerViewModel AddViewModel(IDetailedStatisticsViewModel viewModel)
        {
            RemoveAllViewModelsInHistoryThatAreBehindCurrentViewModel();

            ViewModelHistory.Add(viewModel);

            CurrentViewModel = viewModel;
            RaisePropertyChanged(() => CurrentViewModel);
            

            viewModel.ChildViewModelChanged += vm => AddViewModel(vm);

            return this;
        }

        public IDetailedStatisticsAnalyzerViewModel InitializeWith(IActionSequenceStatisticsSet actionSequenceStatisticsSet)
        {
            ViewModelHistory.Clear();
            
            Predicate<IActionSequenceStatisticsSet> isPreflop = s => s.Street == Streets.PreFlop;
            Predicate<IActionSequenceStatisticsSet> isHeroActs = s => s.ActionSequence == ActionSequences.HeroActs;
            Predicate<IActionSequenceStatisticsSet> isOppB = s => s.ActionSequence == ActionSequences.OppB;
            Predicate<IActionSequenceStatisticsSet> isHeroXOppB = s => s.ActionSequence == ActionSequences.HeroXOppB;

            actionSequenceStatisticsSet.Match()
                .With(isPreflop, _ => AddViewModel(_detailedPreFlopStatisticsViewModelMake.New))
                .With(isHeroActs, _ => AddViewModel(_detailedPostFlopActionStatisticsViewModelMake.New))
                .With(isOppB, _ => AddViewModel(_detailedPostFlopReactionStatisticsViewModelMake.New))
                .With(isHeroXOppB, _ => AddViewModel(_detailedPostFlopReactionStatisticsViewModelMake.New))
                .Do();

            CurrentViewModel.InitializeWith(actionSequenceStatisticsSet);

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

        #endregion

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