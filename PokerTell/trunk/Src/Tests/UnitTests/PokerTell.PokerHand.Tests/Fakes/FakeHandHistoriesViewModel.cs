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

        protected override void ShowPreflopFoldsInAllHandHistoriesIfShowPreflopFoldsIsTrue()
        {
            if (!InterceptOnSetMethods)
            {
                base.ShowPreflopFoldsInAllHandHistoriesIfShowPreflopFoldsIsTrue();
            }
        }

        protected override void FilterOutUnselectedHandHistoriesIfShowSelectedOnlyIsTrue()
        {
            if (!InterceptOnSetMethods)
            {
                base.FilterOutUnselectedHandHistoriesIfShowSelectedOnlyIsTrue();
            }
        }

        protected override void SetAllHandHistoriesShowSelectOption()
        {
            if (!InterceptOnSetMethods)
            {
                base.SetAllHandHistoriesShowSelectOption();
            }
        }

        #endregion

        public IHandHistoriesViewModel RemoveFirstShownHand()
        {
            _itemsPagesManager.AllShownItems.RemoveAt(0);
            return this;
        }
    }
}