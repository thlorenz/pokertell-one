namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    [Serializable]
    public class HandHistoriesViewModel : NotifyPropertyChanged, IHandHistoriesViewModel
    {
        protected readonly IItemsPagesManager<IHandHistoryViewModel> _itemsPagesManager;

        readonly IHandHistoriesFilter _handHistoriesFilter;

        [NonSerialized]
        readonly IConstructor<IHandHistoryViewModel> _handHistoryViewModelMake;

        [NonSerialized]
        ICommand _navigateBackwardCommand;

        [NonSerialized]
        ICommand _navigateForwardCommand;

        [NonSerialized]
        ObservableCollection<int> _pageNumbers;

        [NonSerialized]
        ICommand _selectAllHandHistoriesOnPageCommand;

        [NonSerialized]
        ICommand _selectAllShownHandHistoriesCommand;

        [NonSerialized]
        bool _showSelectOption;

        [NonSerialized]
        ICommand _unselectAllHandHistoriesOnPageCommand;

        [NonSerialized]
        ICommand _unselectAllShownHandHistoriesCommand;

        public HandHistoriesViewModel(
            IConstructor<IHandHistoryViewModel> handHistoryViewModelMake, 
            IItemsPagesManager<IHandHistoryViewModel> itemsPagesManager, 
            IHandHistoriesFilter handHistoriesFilter)
        {
            _handHistoriesFilter = handHistoriesFilter;
            _itemsPagesManager = itemsPagesManager;
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _pageNumbers = new ObservableCollection<int>();
        }

        [field: NonSerialized]
        public event Action PageTurn;

        public int CurrentPage
        {
            get { return (int)_itemsPagesManager.CurrentPage; }
            set
            {
                _itemsPagesManager.NavigateToPage((uint)value);
                UpdatePageInfo();
            }
        }

        public IHandHistoriesFilter HandHistoriesFilter
        {
            get { return _handHistoriesFilter; }
        }

        public ObservableCollection<IHandHistoryViewModel> HandHistoriesOnPage
        {
            get { return _itemsPagesManager.ItemsOnCurrentPage; }
        }

        public ICommand NavigateBackwardCommand
        {
            get
            {
                return _navigateBackwardCommand ?? (_navigateBackwardCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _itemsPagesManager.NavigateBackward();
                            UpdatePageInfo();
                        }, 
                        CanExecuteDelegate = arg => _itemsPagesManager.CanNavigateBackward
                    });
            }
        }

        public ICommand NavigateForwardCommand
        {
            get
            {
                return _navigateForwardCommand ?? (_navigateForwardCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _itemsPagesManager.NavigateForward();
                            UpdatePageInfo();
                        }, 
                        CanExecuteDelegate = arg => _itemsPagesManager.CanNavigateForward
                    });
            }
        }

        public string PageNavigationInfo
        {
            get { return string.Format("{0}/{1}", _itemsPagesManager.CurrentPage, _itemsPagesManager.NumberOfPages); }
        }

        public ObservableCollection<int> PageNumbers
        {
            get { return _pageNumbers; }
        }

        public ICommand SelectAllHandHistoriesOnPageCommand
        {
            get
            {
                return _selectAllHandHistoriesOnPageCommand ?? (_selectAllHandHistoriesOnPageCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => SetAllHandHistoriesOnPageSelectedTo(true), 
                        CanExecuteDelegate = arg => true
                    });
            }
        }

        public ICommand SelectAllShownHandHistoriesCommand
        {
            get
            {
                return _selectAllShownHandHistoriesCommand ?? (_selectAllShownHandHistoriesCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => SetAllShownHandHistorieSelectedTo(true), 
                        CanExecuteDelegate = arg => true
                    });
            }
        }

        public IEnumerable<IHandHistoryViewModel> SelectedHandHistories
        {
            get
            {
                return from historyViewModel in _itemsPagesManager.AllItems
                       where historyViewModel.IsSelected
                       select historyViewModel;
            }
        }

        public bool ShowSelectOption
        {
            get { return _showSelectOption; }
            set
            {
                _showSelectOption = value;
                SetAllHandHistoriesShowSelectOption();
            }
        }

        public ICommand UnselectAllHandHistoriesOnPageCommand
        {
            get
            {
                return _unselectAllHandHistoriesOnPageCommand ??
                       (_unselectAllHandHistoriesOnPageCommand = new SimpleCommand
                           {
                               ExecuteDelegate = arg => SetAllHandHistoriesOnPageSelectedTo(false), 
                               CanExecuteDelegate = arg => true
                           });
            }
        }

        public ICommand UnselectAllShownHandHistoriesCommand
        {
            get
            {
                return _unselectAllShownHandHistoriesCommand ??
                       (_unselectAllShownHandHistoriesCommand = new SimpleCommand
                           {
                               ExecuteDelegate = arg => SetAllShownHandHistorieSelectedTo(false), 
                               CanExecuteDelegate = arg => true
                           });
            }
        }

        public void SelectHeroInAllHandHistoriesIfHeroSelectedIsTrue()
        {
            foreach (IHandHistoryViewModel handHistoryViewModel in _itemsPagesManager.AllItems)
            {
                handHistoryViewModel.SelectRowOfPlayer(
                    HandHistoriesFilter.SelectHero ? HandHistoriesFilter.HeroName : null);
            }
        }

        public IHandHistoriesViewModel ApplyFilter(IPokerHandCondition condition)
        {
            _itemsPagesManager
                .FilterItems(
                handHistoryViewModel => {
                    handHistoryViewModel.AdjustToConditionAction(condition);
                    return handHistoryViewModel.Visible;
                });

            _itemsPagesManager.NavigateToPage(1);
            UpdatePageInfo();

            return this;
        }

        public IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage)
        {
            var handHistoryViewModels = new List<IHandHistoryViewModel>();

            if (!string.IsNullOrEmpty(_handHistoriesFilter.HeroName) && convertedPokerHands.Count() > 0 &&
                !string.IsNullOrEmpty(convertedPokerHands.First().HeroName))
            {
                _handHistoriesFilter.HeroName = convertedPokerHands.First().HeroName;
            }

            foreach (IConvertedPokerHand hand in convertedPokerHands)
            {
                IHandHistoryViewModel handHistoryViewModel = _handHistoryViewModelMake.New;

                handHistoryViewModel
                    .InitializeWith(_handHistoriesFilter.ShowPreflopFolds)
                    .UpdateWith(hand);

                handHistoryViewModels.Add(handHistoryViewModel);
            }

            _itemsPagesManager.InitializeWith((uint)itemsPerPage, handHistoryViewModels);

            ConnectToHandHistoryFilterEvents();

            UpdatePageInfo();

            return this;
        }

        public IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands)
        {
            InitializeWith(convertedPokerHands, 10);
            return this;
        }

        protected virtual void FilterOutUnselectedHandHistoriesIfShowSelectedOnlyIsTrue()
        {
            _itemsPagesManager.FilterItems(model => ((!_handHistoriesFilter.ShowSelectedOnly) || model.IsSelected));
            UpdatePageInfo();
        }

        protected virtual void SetAllHandHistoriesShowSelectOption()
        {
            foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
            {
                model.ShowSelectOption = _showSelectOption;
            }
        }

        protected virtual void ShowPreflopFoldsInAllHandHistoriesIfShowPreflopFoldsIsTrue()
        {
            foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
            {
                model.ShowPreflopFolds = _handHistoriesFilter.ShowPreflopFolds;
            }

            SelectHeroInAllHandHistoriesIfHeroSelectedIsTrue();
        }

        void ConnectToHandHistoryFilterEvents()
        {
            _handHistoriesFilter.ShowPreflopFoldsChanged += ShowPreflopFoldsInAllHandHistoriesIfShowPreflopFoldsIsTrue;
            _handHistoriesFilter.ShowSelectedOnlyChanged += FilterOutUnselectedHandHistoriesIfShowSelectedOnlyIsTrue;
            _handHistoriesFilter.SelectHeroChanged += SelectHeroInAllHandHistoriesIfHeroSelectedIsTrue;
        }

        void InvokePageTurn()
        {
            Action changed = PageTurn;
            if (changed != null)
            {
                changed();
            }
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            ConnectToHandHistoryFilterEvents();
            _itemsPagesManager.Deserialized += OnItemsPagesManagerDeserialized;
        }

        void OnItemsPagesManagerDeserialized()
        {
            _pageNumbers = new ObservableCollection<int>();
            UpdatePageInfo();
        }

        void SetAllHandHistoriesOnPageSelectedTo(bool selected)
        {
            foreach (IHandHistoryViewModel handHistory in _itemsPagesManager.ItemsOnCurrentPage)
            {
                handHistory.IsSelected = selected;
            }
        }

        void SetAllShownHandHistorieSelectedTo(bool selected)
        {
            foreach (IHandHistoryViewModel handHistory in _itemsPagesManager.AllShownItems)
            {
                handHistory.IsSelected = selected;
            }
        }

        void UpdatePageInfo()
        {
            RaisePropertyChanged(() => PageNavigationInfo);
            RaisePropertyChanged(() => CurrentPage);
            InvokePageTurn();

            _pageNumbers.Clear();

            for (int i = 0; i < _itemsPagesManager.NumberOfPages; i++)
            {
                _pageNumbers.Add(i + 1);
            }
        }
    }
}