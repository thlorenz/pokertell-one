namespace PokerTell.PokerHand.Tests.Fakes
{
    using System;

    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.ViewModels;

    using Tools.Interfaces;

    [Serializable]
    public class FakeHandHistoriesViewModel : HandHistoriesViewModel
    {
        #region Constants and Fields

        public bool InterceptOnSetMethods;

        #endregion

        #region Constructors and Destructors

        public FakeHandHistoriesViewModel(
            IConstructor<IHandHistoryViewModel> handHistoryViewModelMake, 
            IItemsPagesManager<IHandHistoryViewModel> itemsPagesManager, 
            IHandHistoriesFilter handHistoriesFilter)
            : base(handHistoryViewModelMake, itemsPagesManager, handHistoriesFilter)
        {
            InterceptOnSetMethods = false;
        }

        #endregion

        #region Methods

        protected override void OnShowPreflopFoldsChanged()
        {
            if (!InterceptOnSetMethods)
            {
                base.OnShowPreflopFoldsChanged();
            }
        }

        protected override void OnShowSelectedOnlyChanged()
        {
            if (!InterceptOnSetMethods)
            {
                base.OnShowSelectedOnlyChanged();
            }
        }

        protected override void OnShowSelectOptionChanged()
        {
            if (!InterceptOnSetMethods)
            {
                base.OnShowSelectOptionChanged();
            }
        }

        #endregion
    }
}