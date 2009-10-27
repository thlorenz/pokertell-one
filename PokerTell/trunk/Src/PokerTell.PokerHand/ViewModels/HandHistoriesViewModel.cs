namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    [Serializable]
    public class HandHistoriesViewModel : ViewModel, IHandHistoriesViewModel
    {
        #region Constants and Fields

        [NonSerialized]
        readonly IConstructor<IHandHistoryViewModel> _handHistoryViewModelMake;

        readonly IItemsPagesManager<IHandHistoryViewModel> _itemsPagesManager;

        [NonSerialized]
        readonly ObservableCollection<int> _pageNumbers;

        string _heroName;

        [NonSerialized]
        ICommand _navigateBackwardCommand;

        [NonSerialized]
        ICommand _navigateForwardCommand;

        bool _showPreflopFolds;

        bool _showSelectedOnly;

        bool _showSelectOption;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesViewModel(
            IConstructor<IHandHistoryViewModel> handHistoryViewModelMake, 
            IItemsPagesManager<IHandHistoryViewModel> itemsPagesManager)
        {
            _itemsPagesManager = itemsPagesManager;
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _pageNumbers = new ObservableCollection<int>();
        }

        #endregion

        #region Events

        [field: NonSerialized]
        public event Action PageTurn;

        #endregion

        #region Properties

        public int CurrentPage
        {
            get { return (int)_itemsPagesManager.CurrentPage; }
            set
            {
                _itemsPagesManager.NavigateToPage((uint)value);
                UpdatePageInfo();
            }
        }

        public ObservableCollection<IHandHistoryViewModel> HandHistoriesOnPage
        {
            get { return _itemsPagesManager.ItemsOnCurrentPage; }
        }

        public string HeroName
        {
            get { return _heroName; }
            set { _heroName = value; }
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

        public IEnumerable<IHandHistoryViewModel> SelectedHandHistories
        {
            get
            {
                return from historyViewModel in _itemsPagesManager.AllItems
                       where historyViewModel.IsSelected
                       select historyViewModel;
            }
        }

        public bool ShowPreflopFolds
        {
            get { return _showPreflopFolds; }
            set
            {
                _showPreflopFolds = value;
                OnShowPreflopFoldsChanged();
            }
        }

        public bool ShowSelectedOnly
        {
            get { return _showSelectedOnly; }
            set
            {
                _showSelectedOnly = value;
                OnShowSelectedOnlyChanged();
            }
        }

        public bool ShowSelectOption
        {
            get { return _showSelectOption; }
            set
            {
                _showSelectOption = value;
                OnShowSelectOptionChanged();
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHandHistoriesViewModel

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

            foreach (IConvertedPokerHand hand in convertedPokerHands)
            {
                IHandHistoryViewModel handHistoryViewModel = _handHistoryViewModelMake.New;

                handHistoryViewModel
                    .Initialize(_showPreflopFolds)
                    .UpdateWith(hand);

                handHistoryViewModels.Add(handHistoryViewModel);
            }

            _itemsPagesManager.InitializeWith((uint)itemsPerPage, handHistoryViewModels);

            return this;
        }

        public IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands)
        {
            InitializeWith(convertedPokerHands, 10);
            return this;
        }

        public void SelectPlayer(bool clearSelection)
        {
            foreach (IHandHistoryViewModel handHistoryViewModel in _itemsPagesManager.AllItems)
            {
                handHistoryViewModel.SelectRowOfPlayer(clearSelection ? null : _heroName);
            }
        }

        #endregion

        #endregion

        #region Methods

        protected virtual void OnShowPreflopFoldsChanged()
        {
            foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
            {
                model.ShowPreflopFolds = _showPreflopFolds;
            }
        }

        protected virtual void OnShowSelectedOnlyChanged()
        {
            _itemsPagesManager.FilterItems(model => ((!_showSelectedOnly) || model.IsSelected));
            UpdatePageInfo();
        }

        protected virtual void OnShowSelectOptionChanged()
        {
            foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
            {
                model.ShowSelectOption = _showSelectOption;
            }
        }

        void InvokePageTurn()
        {
            Action changed = PageTurn;
            if (changed != null)
            {
                changed();
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

        #endregion
    }
}