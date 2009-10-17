namespace PokerTell.PokerHand.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.GenericUtilities;
    using Tools.WPF.ViewModels;

    public class HandHistoriesViewModel : IHandHistoriesViewModel
    {
        #region Constants and Fields

        readonly CompositeAction<IPokerHandCondition> _applyFilterCompositeAction;

        readonly IConstructor<IHandHistoryViewModel> _handHistoryViewModelMake;

        readonly ObservableCollection<IHandHistoryViewModel> _handHistoryViewModels;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesViewModel(IConstructor<IHandHistoryViewModel> handHistoryViewModelMake)
        {
            _handHistoryViewModelMake = handHistoryViewModelMake;
            _handHistoryViewModels = new ObservableCollection<IHandHistoryViewModel>();

            _applyFilterCompositeAction = new CompositeAction<IPokerHandCondition>();
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

        #endregion

        #region Implemented Interfaces

        #region IHandHistoriesViewModel

        public IHandHistoriesViewModel InitializeWith(IEnumerable<IConvertedPokerHand> convertedPokerHands)
        {
            foreach (IConvertedPokerHand hand in convertedPokerHands)
            {
                IHandHistoryViewModel handHistoryViewModel = _handHistoryViewModelMake.New;

                handHistoryViewModel
                    .Initialize(false)
                    .UpdateWith(hand);

                _handHistoryViewModels.Add(handHistoryViewModel);

                ApplyFilterCompositeAction.RegisterAction(handHistoryViewModel.AdjustToConditionAction);
            }

            return this;
        }

        #endregion

        #endregion

    }
}