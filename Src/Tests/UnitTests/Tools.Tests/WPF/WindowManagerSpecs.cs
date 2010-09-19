namespace Tools.Tests.WPF
{
    using System;
    using System.Threading;
    using System.Windows;

    using Machine.Specifications;

    using Moq;
    using Tools.WPF;

    using it = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class WindowManagerSpecs
    {
        protected static Mock<Window> _window_Mock;

        protected static WindowManagerSut _sut;

        protected static int _creationsCounter;

        Establish specContext = () => {
            _window_Mock = new Mock<Window>();
            _creationsCounter = 0;
            _sut = new WindowManagerSut(() => {
                _creationsCounter++;
                return _window_Mock.Object;
            });
        };

        [Subject(typeof(WindowManager), "LazyCreation")]
        public class when_window_is_accessed_for_the_first_time : WindowManagerSpecs
        {
            static Window targetWindow;

            Because of = () => targetWindow = _sut.Window;

            it should_be_created = () => _creationsCounter.ShouldEqual(1);
           
            it should_return_the_window_that_is_created_via_the_passed_function = () => targetWindow.ShouldEqual(_window_Mock.Object);
        }

        [Subject(typeof(WindowManager), "LazyCreation")]
        public class when_window_is_accessed_for_the_second_time : WindowManagerSpecs
        {
            static Window previouslyCreatedWindow;

            static Window targetWindow;

            Establish context = () => previouslyCreatedWindow = _sut.Window;

            Because of = () => targetWindow = _sut.Window;

            it should_not_be_created_again = () => _creationsCounter.ShouldEqual(1);

            it should_return_the_window_that_was_created_previously = () => targetWindow.ShouldEqual(previouslyCreatedWindow);
        }

        [Subject(typeof(WindowManager), "Closing Window")]
        public class when_window_is_closed_by_the_user : WindowManagerSpecs
        {
            static Window window;

            Establish context = () => {
                window = new Window();
                _sut._Window = window;
                var fakeAccessToRegisterEvent = _sut.Window;
            };

            Because of = () => window.Close();

            it should_set_the_window_to_null_to_force_recreation = () => _sut._Window.ShouldBeNull();
        }

        [Subject(typeof(WindowManager), "Dispose")]
        public class when_Manager_is_disposed_and_the_window_was_not_created_yet : WindowManagerSpecs
        {
            static bool windowWasClosed;
            Establish catchClose = () => {
                var win = new Window();
                win.Closed += (s, e) => windowWasClosed = true;
                _sut = new WindowManagerSut(() => win);
            };

            Because of = () => _sut.Dispose();

            it should_not_close_the_window = () => windowWasClosed.ShouldBeFalse();

            it should_set_the_window_to_null_to_force_recreation = () => _sut._Window.ShouldBeNull();
            
        }
    }

    public class WindowManagerSut : WindowManager
    {
        public WindowManagerSut(Func<Window> createWindow)
            : base(createWindow)
        {
        }

        internal Window _Window
        {
            get { return _window; }
            set { _window = value; }
        }
    }
}