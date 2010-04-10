namespace PokerTell.LiveTracker.Tests.ViewModels.Overlay
{
    using System;
    using System.Drawing;

    using Infrastructure.Interfaces;

    using Machine.Specifications;

    using Moq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels.Overlay;

    using Tools.Interfaces;
    using Tools.Validation;
    using Tools.WPF.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class GameHistoryViewModelSpecs
    {
        protected static IGameHistoryViewModel _sut;

        protected static Mock<IHandHistoryViewModel> _handHistoryVM_Mock;

        protected static Mock<IConvertedPokerHand> _hand_Stub;

        protected static Mock<ISettings> _settings_Mock;

        protected static Mock<IDimensionsViewModel> _dimensionsVM_Mock;

        protected static Mock<IDispatcherTimer> _scrollToNewestHandTimer_Mock;
        
        Establish specContext = () => {
            _handHistoryVM_Mock = new Mock<IHandHistoryViewModel>();
            _hand_Stub = new Mock<IConvertedPokerHand>();
            _settings_Mock = new Mock<ISettings>();

            _dimensionsVM_Mock = new Mock<IDimensionsViewModel>();
            _dimensionsVM_Mock
                .Setup(d => d.InitializeWith(Moq.It.IsAny<Rectangle>()))
                .Returns(_dimensionsVM_Mock.Object);
            
            _scrollToNewestHandTimer_Mock = new Mock<IDispatcherTimer>();

            _sut = new GameHistoryViewModel(_settings_Mock.Object, _dimensionsVM_Mock.Object, _handHistoryVM_Mock.Object, _scrollToNewestHandTimer_Mock.Object, new CollectionValidator());
        };

        [Subject(typeof(GameHistoryViewModel), "Instantiation")]
        public class when_it_is_instantiated : GameHistoryViewModelSpecs
        {
            static Rectangle returnedRectangle;

            Establish context = () => {
                returnedRectangle = new Rectangle(1, 1, 2, 2);
                _settings_Mock
                    .Setup(s => s.RetrieveRectangle(GameHistoryViewModel.DimensionsKey, Moq.It.IsAny<Rectangle>()))
                    .Returns(returnedRectangle);
            };

            Because of = () => _sut = new GameHistoryViewModel(_settings_Mock.Object, _dimensionsVM_Mock.Object, _handHistoryVM_Mock.Object, _scrollToNewestHandTimer_Mock.Object, new CollectionValidator());

            It should_ask_the_settings_for_its_dimensions_with_a_default_value
                = () => _settings_Mock.Verify(s => s.RetrieveRectangle(GameHistoryViewModel.DimensionsKey, Moq.It.IsAny<Rectangle>()));

            It should_initialize_the_dimensions_with_the_rectangle_returned_by_the_settings
                = () => _dimensionsVM_Mock.Verify(d => d.InitializeWith(returnedRectangle));

            It should_assign_its_dimensions_to_the_initialized_dimensions = () => _sut.Dimensions.ShouldEqual(_dimensionsVM_Mock.Object);

            It should_set_the_interval_of_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.VerifySet(t => t.Interval = Moq.It.IsAny<TimeSpan>());
        }

        [Subject(typeof(GameHistoryViewModel), "Save Dimensions")]
        public class when_told_to_save_its_dimensions : GameHistoryViewModelSpecs
        {
            static Rectangle returnedRectangle;

            Establish context = () => {
                returnedRectangle = new Rectangle(1, 1, 2, 2);
                _dimensionsVM_Mock
                    .SetupGet(d => d.Rectangle)
                    .Returns(returnedRectangle);
                _sut = new GameHistoryViewModel(_settings_Mock.Object, _dimensionsVM_Mock.Object, _handHistoryVM_Mock.Object, _scrollToNewestHandTimer_Mock.Object, new CollectionValidator());
            };

            Because of = () => _sut.SaveDimensions();

            It should_tell_the_settings_to_set_the_rectangle_for_its_key_to_the_one_returned_by_its_dimensions
                = () => _settings_Mock.Verify(s => s.Set(GameHistoryViewModel.DimensionsKey, returnedRectangle));
        }

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
            const string tableName = "some table";

            Establish context = () => {
                _hand_Stub.SetupGet(h => h.TableName).Returns(tableName);

                _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object);
            };

            Because of = () => _sut.AddNewHand(_hand_Stub.Object);

            It should_update_the_CurrentHandHistory_viewmodel_with_the_passed_hand
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))));

            It should_add_the_hand_to_the_History = () => _sut.HandCount.ShouldEqual(2);

            It should_set_the_currenthand_index_to_1 = () => _sut.CurrentHandIndex.ShouldEqual(1);

            It should_set_the_table_name_to_the_one_returned_by_the_new_hand = () => _sut.TableName.ShouldEqual(tableName);
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
        public class when_3_hands_were_added_before_and_the_current_hand_index_is_1_because_the_user_is_browsing : GameHistoryViewModelSpecs
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

            It should_not_start_the_scroll_to_newest_hand_timer_again = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Start(), Times.Once());

            It should_stop_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Stop());
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

            It should_start_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Start());
        }

        [Subject(typeof(GameHistoryViewModel), "CurrentHandIndex")]
        public class when_3_hands_where_added_and_the_current_handindex_is_set_to_2 : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(_hand_Stub.Object);

            Because of = () => _sut.CurrentHandIndex = 2;

            It should_update_the_CurrentHandHistory_with_the_hand_at_index_2
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.Is<IConvertedPokerHand>(hh => hh.Equals(_hand_Stub.Object))), Times.Exactly(2));

            It should_not_start_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Start(), Times.Never());

            It should_stop_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Stop());
        }

        [Subject(typeof(GameHistoryViewModel), "CurrentHandIndex")]
        public class when_no_hands_were_added_and_I_try_to_set_the_CurrentHandIndex_to_0 : GameHistoryViewModelSpecs
        {
            Because of = () => _sut.CurrentHandIndex = 0;

            It should_not_update_the_CurrentHandHistory
                = () => _handHistoryVM_Mock.Verify(hvm => hvm.UpdateWith(Moq.It.IsAny<IConvertedPokerHand>()), Times.Never());

            It should_not_start_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Start(), Times.Never());
        }

        [Subject(typeof(GameHistoryViewModel), "CurrentHandIndex")]
        public class when_3_hands_where_added_and_the_current_handindex_was_set_to_1_and_the_scroll_to_newest_hand_timer_ticks : GameHistoryViewModelSpecs
        {
            Establish context = () => _sut.AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .AddNewHand(_hand_Stub.Object)
                                          .AddNewHand(new Mock<IConvertedPokerHand>().Object)
                                          .CurrentHandIndex = 1;

            Because of = () => _scrollToNewestHandTimer_Mock.Raise(t => t.Tick += null, null, null);

            It should_set_the_CurrentHandIndex_to_2 = () => _sut.CurrentHandIndex.ShouldEqual(2);

            It should_stop_the_scroll_to_newest_hand_timer = () => _scrollToNewestHandTimer_Mock.Verify(t => t.Stop(), Times.Exactly(5));
        }
    }
}