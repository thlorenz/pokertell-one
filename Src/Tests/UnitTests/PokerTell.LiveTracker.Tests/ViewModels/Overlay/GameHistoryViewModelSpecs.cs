namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GameHistoryViewModelSpecs
    {
        protected static IGameHistoryViewModel _sut;

        protected static Mock<IHandHistoryViewModel> _handHistoryVM_Mock;

        protected static Mock<IConvertedPokerHand> _hand_Stub;

        Establish specContext = () => {
            _handHistoryVM_Mock = new Mock<IHandHistoryViewModel>();
            _hand_Stub = new Mock<IConvertedPokerHand>();
            _sut = new GameHistoryViewModel(_handHistoryVM_Mock.Object);
        };

        [Subject(typeof(GameHistoryViewModel), "AddNewHand")]
        public class when_the_history_was_empty : GameHistoryViewModelSpecs
        {
            Because of = () => _sut.AddNewHand(_hand_Stub.Object);

            It should_update_the_CurrentHandHistory_viewmodel_with_the_passed_hand
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))));

            It should_add_the_hand_to_the_History = () => _sut.HandCount.ShouldEqual(1);

            It should_set_the_currenthand_index_to_0 = () => _sut.CurrentHandIndex.ShouldEqual(0);
        }

        [Subject(typeof(GameHistoryViewModel), "AddNewHand")]
        public class when_the_history_already_contained_another_hand : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object);

            Because of = () => _sut.AddNewHand(_hand_Stub.Object);

            It should_update_the_CurrentHandHistory_viewmodel_with_the_passed_hand
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))));

            It should_add_the_hand_to_the_History = () => _sut.HandCount.ShouldEqual(2);

            It should_set_the_currenthand_index_to_1 = () => _sut.CurrentHandIndex.ShouldEqual(1);
        }

        [Subject(typeof(GameHistoryViewModel), "AddNewHand")]
        public class when_the_history_already_contained_the_same_hand : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(_hand_Stub.Object);

            Because of = () => _sut.AddNewHand(_hand_Stub.Object);

            It should_not_update_the_CurrentHandHistory_viewmodel_with_the_passed_hand_again
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))), Times.Exactly(1));

            It should_not_add_the_hand_to_the_history_again = () => _sut.HandCount.ShouldEqual(1);

            It should_leave_the_currenthand_index_at_0 = () => _sut.CurrentHandIndex.ShouldEqual(0);
        }

        [Subject(typeof(GameHistoryViewModel), "AddNewHand")]
        public class _when_3_hands_were_added_before_and_the_current_hand_index_is_1_because_the_user_is_browsing : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .CurrentHandIndex = 1;

            Because of = () => _sut.AddNewHand(_hand_Stub.Object);

            It should_not_update_the_CurrentHandHistory_viewmodel_with_the_passed_hand
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))), Times.Never());

            It should_add_the_hand_to_the_History = () => _sut.HandCount.ShouldEqual(4);

            It should_leave_the_currenthand_index_at_1 = () => _sut.CurrentHandIndex.ShouldEqual(1);
        }

        [Subject(typeof(GameHistoryViewModel), "CurrentHandIndex")]
        public class when_3_hands_where_added_and_the_current_handindex_is_set_to_1 : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(_hand_Stub.Object)
                                          .AddNewHand(new Mock<IConvertedPokerHand>().Object);

            Because of = () => _sut.CurrentHandIndex = 1;

            It should_update_the_CurrentHandHistory_with_the_hand_at_index_1
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))), Times.Exactly(2));
        }
    }
}