namespace Tools.Tests.WPF
{
    using System.Windows;

    using Machine.Specifications;

    using Moq;

    using Tools.WPF;
    using Tools.WPF.Interfaces;

    using It=Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class WindowManagerSpecs
    {
        protected static Mock<Window> _window_Mock;

        protected static WindowManager _sut;

        protected static int _creationsCounter;

        Establish specContext = () => {
            _window_Mock = new Mock<Window>();
            _creationsCounter = 0;
            _sut = new WindowManager(() => {
                _creationsCounter++;
                return _window_Mock.Object;
            });
        };

        [Subject(typeof(WindowManager), "LazyCreation")]
        public class when_window_is_accessed_for_the_first_time : WindowManagerSpecs
        {
            static Window targetWindow;
            
            Because of = () => targetWindow = _sut.Window;

            It should_be_created = () => _creationsCounter.ShouldEqual(1);

            It should_return_the_window_that_is_created_via_the_passed_function = () => targetWindow.ShouldEqual(_window_Mock.Object);
        }


        [Subject(typeof(WindowManager), "LazyCreation")]
        public class when_window_is_accessed_for_the_second_time : WindowManagerSpecs
        {
            static Window previouslyCreatedWindow;

            static Window targetWindow;

            Establish context = () => previouslyCreatedWindow = _sut.Window;
            
            Because of = () => targetWindow = _sut.Window;

            It should_not_be_created_again = () => _creationsCounter.ShouldEqual(1);

            It should_return_the_window_that_was_created_previously = () => targetWindow.ShouldEqual(previouslyCreatedWindow);
        }

        [Subject(typeof(WindowManager), "Dispose")]
        public class when_Manager_is_disposed_and_the_window_was_not_created_yet : WindowManagerSpecs
        {
            Because of = () => _sut.Dispose();

            It should_not_close_the_window = () => _window_Mock.Verify(w => w.Close(), Times.Never());
        }
    }
}