namespace PokerTell.Statistics.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Enumerations.PokerHand;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.Statistics;
    using PokerTell.Statistics.Interfaces;
    using PokerTell.Statistics.ViewModels.Base;

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

            ViewModelHistory = new List<IDetailedStatisticsAnalyzerContentViewModel>();
            CurrentViewModel = StatisticsTableViewModel.Emty;
        }

        #endregion

        #region Properties

        public IDetailedStatisticsAnalyzerContentViewModel CurrentViewModel { get; set; }

        IDetailedStatisticsAnalyzerContentViewModel _popupViewModel;

        public IDetailedStatisticsAnalyzerContentViewModel PopupViewModel
        {
            get { return _popupViewModel; }
            set
            {
                _popupViewModel = value;
                RaisePropertyChanged(() => PopupViewModel);
            }
        }

        bool _showPopup;

        public bool ShowPopup
        {
            get { return _showPopup; }
            set
            {
                _showPopup = value;
                RaisePropertyChanged(() => ShowPopup);
            }
        }

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

        public IList<IDetailedStatisticsAnalyzerContentViewModel> ViewModelHistory { get; private set; }

        public bool Visible
        {
            get { return ViewModelHistory != null && ViewModelHistory.Count > 0; }
        }

        #endregion

        #region Implemented Interfaces

        #region IDetailedStatisticsAnalyzerViewModel

        public IDetailedStatisticsAnalyzerViewModel AddViewModel(IDetailedStatisticsAnalyzerContentViewModel viewModel)
        {
            if (viewModel.ShowAsPopup)
            {
                PopupViewModel = viewModel;
                ShowPopup = true;
                return this;
            }

            ShowPopup = false;

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

            IDetailedStatisticsViewModel detailedStatisticsViewModel = null;

            actionSequenceStatisticsSet.Match()
                .With(isPreflop, _ => detailedStatisticsViewModel = _detailedPreFlopStatisticsViewModelMake.New)
                .With(isHeroActs, _ => detailedStatisticsViewModel = _detailedPostFlopActionStatisticsViewModelMake.New)
                .With(isOppB, _ => detailedStatisticsViewModel = _detailedPostFlopReactionStatisticsViewModelMake.New)
                .With(isHeroXOppB, _ => detailedStatisticsViewModel = _detailedPostFlopReactionStatisticsViewModelMake.New)
                .Do();

            AddViewModel(detailedStatisticsViewModel);
            detailedStatisticsViewModel.InitializeWith(actionSequenceStatisticsSet);

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