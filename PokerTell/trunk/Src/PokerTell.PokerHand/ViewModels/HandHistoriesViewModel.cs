namespace PokerTell.PokerHand.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;
    using Tools.WPF.ViewModels;

    public class HandHistoriesViewModel : ViewModel, IHandHistoriesViewModel
    {
        #region Constants and Fields

        readonly CompositeAction<IPokerHandCondition> _applyFilterCompositeAction;

        readonly IConstructor<IHandHistoryViewModel> _handHistoryViewModelMake;

        readonly IList<IHandHistoryViewModel> _handHistoryViewModels;

        readonly ObservableCollection<IHandHistoryViewModel> _handHistoryViewModelsOnPage;

        int _itemsPerPage;

        bool _showPreflopFolds;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesViewModel(IConstructor<IHandHistoryViewModel> handHistoryViewModelMake)
        {
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _handHistoryViewModelsOnPage = new ObservableCollection<IHandHistoryViewModel>();
            _handHistoryViewModels = new List<IHandHistoryViewModel>();

            _applyFilterCompositeAction = new CompositeAction<IPokerHandCondition>();
        }

        #endregion

        #region Properties

        public CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction
        {
            get { return _applyFilterCompositeAction; }
        }

        public ObservableCollection<IHandHistoryViewModel> HandHistoryViewModelsOnPage
        {
            get { return _handHistoryViewModelsOnPage; }
        }

        public bool ShowPreflopFolds
        {
            get { return _showPreflopFolds; }
            set
            {
                _showPreflopFolds = value;
                foreach (IHandHistoryViewModel model in HandHistoryViewModelsOnPage)
                {
                    model.ShowPreflopFolds = value;
                }
            }
        }

        public bool ShowSelectedOnly
        {
            set
            {
                foreach (IHandHistoryViewModel model in HandHistoryViewModelsOnPage)
                {
                    model.Visible = (! value) || model.IsSelected;
                }
            }
        }

        public bool ShowSelectOption
        {
            set
            {
                foreach (IHandHistoryViewModel model in HandHistoryViewModelsOnPage)
                {
                    model.ShowSelectOption = value;
                }
            }
        }

        #endregion

        #region Public Methods

        public IHandHistoriesViewModel InitializeWith(
            IEnumerable<IConvertedPokerHand> convertedPokerHands, int itemsPerPage)
        {
            _itemsPerPage = itemsPerPage;
            
            foreach (IConvertedPokerHand hand in convertedPokerHands)
            {
                IHandHistoryViewModel handHistoryViewModel = _handHistoryViewModelMake.New;

                handHistoryViewModel
                    .Initialize(_showPreflopFolds)
                    .UpdateWith(hand);

                ApplyFilterCompositeAction.RegisterAction(handHistoryViewModel.AdjustToConditionAction);

                _handHistoryViewModels.Add(handHistoryViewModel);
            }

           // NavigateToPage(0);

            return this;
        }

        
        #endregion

        #region Implemented Interfaces

        #region IHandHistoriesViewModel

        public IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands)
        {
            InitializeWith(convertedPokerHands, 10);
            return this;
        }

        public void SelectPlayer(string name)
        {
            foreach (IHandHistoryViewModel handHistoryViewModel in _handHistoryViewModelsOnPage)
            {
                handHistoryViewModel.SelectRowOfPlayer(name);
            }
        }

        #endregion

        #endregion
    }
}