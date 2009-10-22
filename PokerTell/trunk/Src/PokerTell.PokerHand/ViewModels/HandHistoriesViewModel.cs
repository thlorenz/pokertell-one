namespace PokerTell.PokerHand.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class HandHistoriesViewModel : ViewModel, IHandHistoriesViewModel
    {
        #region Constants and Fields

        readonly IConstructor<IHandHistoryViewModel> _handHistoryViewModelMake;

        readonly IItemsPagesManager<IHandHistoryViewModel> _itemsPagesManager;

        ICommand _navigateBackwardCommand;

        ICommand _navigateForwardCommand;

        bool _showPreflopFolds;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesViewModel(
            IConstructor<IHandHistoryViewModel> handHistoryViewModelMake, 
            IItemsPagesManager<IHandHistoryViewModel> itemsPagesManager)
        {
            _itemsPagesManager = itemsPagesManager;
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _pageIndex = 0;
        }

        #endregion

        #region Properties

        int[] _pages;

        public int[] Pages
        {
            get { return new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; }
            set
            {
                _pages = value;
                RaisePropertyChanged(() => Pages);
            }
        }

        uint _pageIndex;
        public uint PageIndex
        {
            get { return _pageIndex;  }
            set { _pageIndex = value; 
                 RaisePropertyChanged(() => PageIndex);
            }
        }

        public ObservableCollection<IHandHistoryViewModel> HandHistoryViewModelsOnPage
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
                            _pageIndex = _itemsPagesManager.CurrentPage - 1;
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
                            _pageIndex = _itemsPagesManager.CurrentPage - 1;
                        },
                        CanExecuteDelegate = arg => _itemsPagesManager.CanNavigateForward
                    });
            }
        }

        public bool ShowPreflopFolds
        {
            get { return _showPreflopFolds; }
            set
            {
                _showPreflopFolds = value;
                foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
                {
                    model.ShowPreflopFolds = value;
                }
            }
        }

        public bool ShowSelectedOnly
        {
            set { _itemsPagesManager.FilterItems(model => ((!value) || model.IsSelected)); }
        }

        public bool ShowSelectOption
        {
            set
            {
                foreach (IHandHistoryViewModel model in _itemsPagesManager.AllItems)
                {
                    model.ShowSelectOption = value;
                }
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

        public void SelectPlayer(string name)
        {
            foreach (IHandHistoryViewModel handHistoryViewModel in _itemsPagesManager.AllItems)
            {
                handHistoryViewModel.SelectRowOfPlayer(name);
            }
        }

        #endregion

        #endregion
    }
}