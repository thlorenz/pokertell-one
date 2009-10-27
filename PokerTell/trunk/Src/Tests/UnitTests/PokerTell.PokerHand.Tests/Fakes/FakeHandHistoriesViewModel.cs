namespace PokerTell.PokerHand.Tests.Fakes
{
    using System;

    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.Interfaces;

    using ViewModels;

    [Serializable]
    public class FakeHandHistoriesViewModel : HandHistoriesViewModel
    {
        public bool InterceptOnSetMethods;

        public FakeHandHistoriesViewModel(IConstructor<IHandHistoryViewModel> handHistoryViewModelMake, IItemsPagesManager<IHandHistoryViewModel> itemsPagesManager)
            : base(handHistoryViewModelMake, itemsPagesManager)
        {
            InterceptOnSetMethods = false;
        }

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
    }
}