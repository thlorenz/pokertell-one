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

        readonly ObservableCollection<IHandHistoryViewModel> _handHistoryViewModels;

        string _hashCode;

        bool _showPreflopFolds;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesViewModel(IConstructor<IHandHistoryViewModel> handHistoryViewModelMake)
        {
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _handHistoryViewModels = new ObservableCollection<IHandHistoryViewModel>();

            _applyFilterCompositeAction = new CompositeAction<IPokerHandCondition>();
            HashCode = "Not Set";
        }

        #endregion

        #region Properties

        public CompositeAction<IPokerHandCondition> ApplyFilterCompositeAction
        {
            get { return _applyFilterCompositeAction; }
        }

        public IEnumerable<IHandHistoryViewModel> HandHistoryViewModels
        {
            get { return _handHistoryViewModels; }
        }

        public string HashCode
        {
            get { return _hashCode; }
            set
            {
                _hashCode = value;
                RaisePropertyChanged(() => HashCode);
            }
        }

        public bool ShowPreflopFolds
        {
            get { return _showPreflopFolds; }
            set
            {
                _showPreflopFolds = value;
                foreach (IHandHistoryViewModel model in HandHistoryViewModels)
                {
                    model.ShowPreflopFolds = value;
                }
            }
        }

        public bool ShowSelectedOnly
        {
            set
            {
                foreach (IHandHistoryViewModel model in HandHistoryViewModels)
                {
                    model.Visible = (! value) || model.IsSelected;
                }
            }
        }

        public bool ShowSelectOption
        {
            set
            {
                foreach (IHandHistoryViewModel model in HandHistoryViewModels)
                {
                    model.ShowSelectOption = value;
                }
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IHandHistoriesViewModel

        public IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands)
        {
            foreach (IConvertedPokerHand hand in convertedPokerHands)
            {
                IHandHistoryViewModel handHistoryViewModel = _handHistoryViewModelMake.New;

                handHistoryViewModel
                    .Initialize(_showPreflopFolds)
                    .UpdateWith(hand);

                ApplyFilterCompositeAction.RegisterAction(handHistoryViewModel.AdjustToConditionAction);

                _handHistoryViewModels.Add(handHistoryViewModel);

                HashCode = GetHashCode().ToString();
            }

            return this;
        }

        #endregion

        #endregion
    }
}