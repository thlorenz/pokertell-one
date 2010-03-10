namespace PokerTell.PokerHand.Tests.ViewModels
{
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Machine.Specifications;

    using Moq;

    using PokerTell.PokerHand.ViewModels;

    using Tools;
    using Tools.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class HandHistoriesViewModelSpecs
    {
        protected static Mock<IHandHistoryViewModel> _handHistoryVM_Mock1;
        protected static Mock<IHandHistoryViewModel> _handHistoryVM_Mock2;

        protected static Mock<IConstructor<IHandHistoryViewModel>> _handHistoryConstructor_Stub;

        protected static Mock<IItemsPagesManager<IHandHistoryViewModel>> _itemsPageManager_Stub;

        protected static Mock<IHandHistoriesFilter> _handHistoriesFilter_Stub;

        protected static IHandHistoriesViewModel _sut;

        Establish specContext = () => {
           _handHistoryVM_Mock1 = new Mock<IHandHistoryViewModel>();
            _handHistoryVM_Mock2 = new Mock<IHandHistoryViewModel>();
            _handHistoryConstructor_Stub = new Mock<IConstructor<IHandHistoryViewModel>>();

            _itemsPageManager_Stub = new Mock<IItemsPagesManager<IHandHistoryViewModel>>();
            _itemsPageManager_Stub.SetupGet(pm => pm.AllItems).Returns(new[] { _handHistoryVM_Mock1.Object, _handHistoryVM_Mock2.Object });

            _handHistoriesFilter_Stub = new Mock<IHandHistoriesFilter>();

            _sut = new HandHistoriesViewModel(_handHistoryConstructor_Stub.Object, _itemsPageManager_Stub.Object, _handHistoriesFilter_Stub.Object);
        };

        [Subject(typeof(HandHistoriesViewModel), "ShowSelectedOption")]
        public class when_I_set_the_ShowSelectedOption_to_true : HandHistoriesViewModelSpecs
        {
            Because of = () => _sut.ShowSelectOption = true;

            It should_set_the_ShowSelectOption_for_all_handhistories_of_the_items_manager_to_true = () => {
                _handHistoryVM_Mock1.VerifySet(hv => hv.ShowSelectOption = true);
                _handHistoryVM_Mock2.VerifySet(hv => hv.ShowSelectOption = true);
            };
        }

        [Subject(typeof(HandHistoriesViewModel), "ShowHandNotes")]
        public class when_I_set_ShowHandNotes_to_true : HandHistoriesViewModelSpecs
        {
            Because of = () => _sut.ShowHandNotes = true;

            It should_set_the_ShowHandNotes_for_all_handhistories_of_the_items_manager_to_true = () => {
                _handHistoryVM_Mock1.VerifySet(hv => hv.ShowHandNote = true);
                _handHistoryVM_Mock2.VerifySet(hv => hv.ShowHandNote = true);
            };
        }
    }
}