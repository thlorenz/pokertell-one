namespace PokerTell.LiveTracker.Tests.Overlay
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;

    using Machine.Specifications;

    using Moq;

    using PokerRooms;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class OverlayToTableAttacherSpecs
    {
        protected static Mock<IWindowManipulator> _windowManipulator_Mock;

        protected static Mock<IWindowFinder> _windowFinder_Mock;

        protected static Mock<IWindowManager> _overlayWindow_Mock;

        protected static Mock<IDispatcherTimer> _watchTableTimer_Stub;

        protected static Mock<IDispatcherTimer> _findTableAgainTimer_Mock;

        protected static Mock<IPokerRoomInfo> _pokerRoomInfo_Stub;

        protected static string _processName;

        protected static string _tableName;

        protected static OverlayToTableAttacherSut _sut;

        Establish specContext = () => {
            _windowManipulator_Mock = new Mock<IWindowManipulator>();
            _windowFinder_Mock = new Mock<IWindowFinder>();
            _overlayWindow_Mock = new Mock<IWindowManager>();
            _watchTableTimer_Stub = new Mock<IDispatcherTimer>();
            _findTableAgainTimer_Mock = new Mock<IDispatcherTimer>();

            _processName = "SomeProcessName";

            _pokerRoomInfo_Stub = new Mock<IPokerRoomInfo>();
            _pokerRoomInfo_Stub
                .SetupGet(i => i.ProcessName).Returns(_processName);

            _tableName = "someTable";
            _sut = new OverlayToTableAttacherSut(_windowManipulator_Mock.Object, _windowFinder_Mock.Object);
            _sut.InitializeWith(_overlayWindow_Mock.Object, 
                                _watchTableTimer_Stub.Object, 
                                _findTableAgainTimer_Mock.Object, 
                                _pokerRoomInfo_Stub.Object,
                                _tableName);
        };

        [Subject(typeof(OverlayToTableAttacher), "Activate")]
        public class when_it_is_activated : OverlayToTableAttacherSpecs
        {
            Because of = () => _sut.Activate();

            It should_try_to_find_the_poker_table = () =>
                _windowFinder_Mock.Verify(
                    wf => wf.FindWindowMatching(Moq.It.IsAny<Regex>(), Moq.It.IsAny<Regex>(), Moq.It.IsAny<Func<IntPtr, bool>>()));
        }

        [Subject(typeof(OverlayToTableAttacher), "Activate")]
        public class when_it_is_activated_and_the_window_finder_finds_the_poker_table : OverlayToTableAttacherSpecs
        {
            protected static IntPtr foundHandle;

            protected static bool tableClosedWasRaised;

            Establish context = () => {
                foundHandle = (IntPtr)1;
                _windowFinder_Mock
                    .Setup(wf => wf.FindWindowMatching(Moq.It.IsAny<Regex>(), Moq.It.IsAny<Regex>(), Moq.It.IsAny<Func<IntPtr, bool>>()))
                    .Callback((Regex r1, Regex r2, Func<IntPtr, bool> cb) => cb(foundHandle));

                _sut.TableClosed += () => tableClosedWasRaised = true;
            };

            Because of = () => _sut.Activate();

            It should_assign_the_returned_WindowHandle_to_the_window_manipulator
                = () => _windowManipulator_Mock.VerifySet(wm => wm.WindowHandle = foundHandle);

            It should_start_to_watch_the_table = () => _watchTableTimer_Stub.Verify(wt => wt.Start());

            It should_not_tell_me_that_the_table_is_closed = () => tableClosedWasRaised.ShouldBeFalse();

        }

        [Subject(typeof(OverlayToTableAttacher), "Activate")]
        public class when_it_is_activated_and_the_window_finder_does_not_find_the_poker_table : OverlayToTableAttacherSpecs
        {
            protected static bool tableClosedWasRaised;

            Establish context = () => {
                _windowFinder_Mock
                    .Setup(wf => wf.FindWindowMatching(Moq.It.IsAny<Regex>(), Moq.It.IsAny<Regex>(), Moq.It.IsAny<Func<IntPtr, bool>>()))
                    .Callback((Regex r1, Regex r2, Func<IntPtr, bool> cb) => { /* doesn't find anything */ });

                _sut.TableClosed += () => tableClosedWasRaised = true;
            };

            Because of = () => _sut.Activate();

            It should_not_start_to_watch_the_table = () => _watchTableTimer_Stub.Verify(wt => wt.Start(), Times.Never());

            It should_tell_me_that_the_table_is_closed = () => tableClosedWasRaised.ShouldBeTrue();
        }

        [Subject(typeof(OverlayToTableAttacher), "Watching Table")]
        public class when_watch_timer_fires_when_not_waiting_for_new_table_name : OverlayToTableAttacherSpecs
        {
            protected static string fullText;

            Establish context = () => fullText = _sut.FullText;

            Because of = () => _watchTableTimer_Stub.Raise(t => t.Tick += null, null, null);

            It should_check_for_any_size_or_position_changes_of_the_poker_table
                = () => _windowManipulator_Mock.Verify(wm => wm.CheckWindowLocationAndSize(Moq.It.IsAny<Action<Point, Size>>()));

            It shoud_ensure_that_the_overlay_is_still_directly_on_top_of_the_poker_table
                =
                () => _windowManipulator_Mock.Verify(wm => wm.PlaceThisWindowDirectlyOnTopOfYours(_overlayWindow_Mock.Object, Moq.It.IsAny<Action>()));

            It should_set_the_window_text_to_the_TableName
                = () => _windowManipulator_Mock.Verify(wm => wm.SetTextTo(_tableName, Moq.It.IsAny<Action<string>>()));
        }

        [Subject(typeof(OverlayToTableAttacher), "Watching Table")]
        public class when_watch_timer_fires_while_waiting_for_new_table_name : OverlayToTableAttacherSpecs
        {
            protected static string fullText;

            Establish context = () => {
                fullText = _sut.FullText;
                _sut.SetWaitingForNewTableName(true);
            };

            Because of = () => _watchTableTimer_Stub.Raise(t => t.Tick += null, null, null);

            It should_check_for_any_size_or_position_changes_of_the_poker_table
                = () => _windowManipulator_Mock.Verify(wm => wm.CheckWindowLocationAndSize(Moq.It.IsAny<Action<Point, Size>>()));

            It shoud_ensure_that_the_overlay_is_still_directly_on_top_of_the_poker_table
                =
                () => _windowManipulator_Mock.Verify(wm => wm.PlaceThisWindowDirectlyOnTopOfYours(_overlayWindow_Mock.Object, Moq.It.IsAny<Action>()));

            It should_not_set_the_window_text_to_the_TableName
                = () => _windowManipulator_Mock.Verify(wm => wm.SetTextTo(_tableName, Moq.It.IsAny<Action<string>>()), Times.Never());
        }

        [Subject(typeof(OverlayToTableAttacher), "Watching Table")]
        public class when_watch_timer_fires_and_the_window_Manipulator_detects_a_change_in_location_and_size_of_the_poker_table : OverlayToTableAttacherSpecs
        {
            protected static Point newLocation;

            protected static Size newSize;

            Establish context = () => {
                newLocation = new Point(1.0, 2.0);
                newSize = new Size(1.0, 2.0);

                _windowManipulator_Mock
                    .Setup(wm => wm.CheckWindowLocationAndSize(Moq.It.IsAny<Action<Point, Size>>()))
                    .Callback((Action<Point, Size> cb) => cb(newLocation, newSize));
            };

            Because of = () => _watchTableTimer_Stub.Raise(wt => wt.Tick += null, null, null);

            It should_adjust_the_location_of_the_overlay_window_to_the_new_location = () => {
                    _overlayWindow_Mock.VerifySet(ow => ow.Left = newLocation.X);
                    _overlayWindow_Mock.VerifySet(ow => ow.Top = newLocation.Y);
                };

            It should_adjust_the_size_of_the_overlay_window_to_the_new_size = () => {
                _overlayWindow_Mock.VerifySet(ow => ow.Width = newSize.Width);
                _overlayWindow_Mock.VerifySet(ow => ow.Height = newSize.Height);
            };
        }

        [Subject(typeof(OverlayToTableAttacher), "Watching Table")]
        public class when_the_watcher_timer_fires_and_the_window_manipulator_detects_that_the_table_name_is_not_contained_in_the_poker_table_title
             : OverlayToTableAttacherSpecs
        {
            protected static bool tableChangedWasRaised;

            protected static string newFullText;

            protected static string raisedWithFullText;
            Establish context = () => {
                newFullText = "new Text";
            
                _windowManipulator_Mock
                    .Setup(wm => wm.SetTextTo(Moq.It.IsAny<string>(), Moq.It.IsAny<Action<string>>()))
                    .Callback((string s1, Action<string> cb) => cb(newFullText));

                _sut.TableChanged += ft => {
                    raisedWithFullText = ft;
                    tableChangedWasRaised = true;
                };
            };

            Because of = () => _watchTableTimer_Stub.Raise(wt => wt.Tick += null, null, null);

            It should_start_waiting_for_a_new_table_name = () => _sut.WaitingForNewTableName.ShouldBeTrue();

            It should_tell_me_that_the_table_changed = () => tableChangedWasRaised.ShouldBeTrue();

            It should_tell_me_the_new_title_of_the_table = () => raisedWithFullText.ShouldEqual(newFullText);
        }

        [Subject(typeof(OverlayToTableAttacher), "SetTableName")]
        public class when_table_name_is_set_while_waiting_for_new_table_name
        {

            Establish context = () => _sut.SetWaitingForNewTableName(true);

            Because of = () => _sut.TableName = "new table name";

            It should_stop_waiting_for_a_new_table_name = () => _sut.WaitingForNewTableName.ShouldBeFalse();
        }

        [Subject(typeof(OverlayToTableAttacher), "Table is lost")]
        public class when_the_window_manipulator_says_it_lost_the_table_while_trying_to_place_the_overlay_on_top_of_it : OverlayToTableAttacherSpecs
        {
            Establish context = () => _windowManipulator_Mock
                                          .Setup(wm => wm.PlaceThisWindowDirectlyOnTopOfYours(Moq.It.IsAny<IWindowManager>(), Moq.It.IsAny<Action>()))
                                          .Callback((IWindowManager _, Action tableLostCallback) => tableLostCallback());

            Because of = () => _watchTableTimer_Stub.Raise(wt => wt.Tick += null, null, null);

            It should_stop_to_watch_the_table = () => _watchTableTimer_Stub.Verify(wt => wt.Stop());

            It should_try_to_find_the_table_again = () => _findTableAgainTimer_Mock.Verify(ft => ft.Start());
        }

        [Subject(typeof(OverlayToTableAttacher), "Finding table again")]
        public class when_the_find_table_again_timer_fires : OverlayToTableAttacherSpecs
        {
            Because of = () => _findTableAgainTimer_Mock.Raise(ft => ft.Tick += null, null, null);

            It should_try_to_find_the_poker_table = () =>
                _windowFinder_Mock.Verify(
                    wf => wf.FindWindowMatching(Moq.It.IsAny<Regex>(), Moq.It.IsAny<Regex>(), Moq.It.IsAny<Func<IntPtr, bool>>()));
        }

        [Subject(typeof(OverlayToTableAttacher), "Dispose")]
        public class when_it_is_disposed : OverlayToTableAttacherSpecs
        {
            Because of = () => _sut.Dispose();

            It should_stop_the_watcher_timer = () => _watchTableTimer_Stub.Verify(wt => wt.Stop());

            It should_stop_the_find_table_again_timer = () => _findTableAgainTimer_Mock.Verify(ft => ft.Stop());

            It should_dispose_the_overlay_window = () => _overlayWindow_Mock.Verify(ow => ow.Dispose());
        }

    }

    public class OverlayToTableAttacherSut : OverlayToTableAttacher
    {
        public OverlayToTableAttacherSut(IWindowManipulator windowManipulator, IWindowFinder windowFinder)
            : base(windowManipulator, windowFinder)
        {
        }

        public OverlayToTableAttacherSut SetWaitingForNewTableName(bool waiting)
        {
            _waitingForNewTableName = true;
            return this;
        }

        public bool WaitingForNewTableName
        {
            get { return _waitingForNewTableName; }
        }
    }
}