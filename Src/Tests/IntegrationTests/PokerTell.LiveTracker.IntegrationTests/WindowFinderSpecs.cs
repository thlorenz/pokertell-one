namespace PokerTell.LiveTracker.IntegrationTests
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    using Interfaces;

    using Machine.Specifications;

    using Overlay;

    // Resharper disable InconsistentNaming
    public abstract class WindowFinderSpecs
    {
        protected static Window _tableWindow_Stub;

        protected static IntPtr _handleOfWindow_Stub;

        protected static Regex _windowTextToMatch;

        protected static IWindowFinder _sut;

        Establish specContext = () => {
           _tableWindow_Stub = new Window 
                {
                    Title = "windowToFind", 
                    Left = 0, 
                    Top = 0, 
                    Width = 120, 
                    Height = 100, 
                    AllowsTransparency = true, 
                    Background = new SolidColorBrush { Color = Colors.Transparent }, 
                    WindowStyle = WindowStyle.None, 
                    ShowInTaskbar = false
                };
            _tableWindow_Stub.Show();

            _windowTextToMatch = new Regex(_tableWindow_Stub.Title);
            _handleOfWindow_Stub = new WindowInteropHelper(_tableWindow_Stub).Handle;

            _sut = new WindowFinder();
        };

        Cleanup tearDown = () => _tableWindow_Stub.Close();

        [Subject(typeof(WindowFinder), "FindWindowMatching")]
        public class when_window_with_the_matching_text_exists_and_the_process_is_specified : WindowFinderSpecs
        {
            protected static Regex processNameToMatch;
            protected static bool windowWasFound;

            protected static IntPtr foundHandle;

            Establish context = () => processNameToMatch = new Regex(Process.GetCurrentProcess().ProcessName);

            Because of = () => _sut.FindWindowMatching(
                                   _windowTextToMatch,
                                   processNameToMatch,
                                   handle => {
                                       windowWasFound = true;
                                       foundHandle = handle;
                                       return true;
                                   });

            It should_find_a_window = () => windowWasFound.ShouldBeTrue();

            It the_found_window_should_be_the_window_we_were_looking_for = () => foundHandle.ShouldEqual(_handleOfWindow_Stub);
        }

        [Subject(typeof(WindowFinder), "FindWindowMatching")]
        public class when_window_with_the_matching_text_exists_and_the_process_is_not_specified : WindowFinderSpecs
        {
            protected static Regex matchAnyProcess;
            protected static bool windowWasFound;

            protected static IntPtr foundHandle;

            Establish context = () => matchAnyProcess = new Regex(".*");

            Because of = () => _sut.FindWindowMatching(
                                   _windowTextToMatch,
                                   matchAnyProcess,
                                   handle => {
                                       windowWasFound = true;
                                       foundHandle = handle;
                                       return true;
                                   });

            It should_find_a_window = () => windowWasFound.ShouldBeTrue();

            It the_found_window_should_be_the_window_we_were_looking_for = () => foundHandle.ShouldEqual(_handleOfWindow_Stub);
        }

        [Subject(typeof(WindowFinder), "FindWindowMatching")]
        public class when_window_with_the_matching_text_exists_but_the_process_specified_does_not_match : WindowFinderSpecs
        {
            protected static Regex processNameToMatch;
            protected static bool windowWasFound;

            Establish context = () => processNameToMatch = new Regex("something it will never find");

            Because of = () => _sut.FindWindowMatching(
                                   _windowTextToMatch,
                                   processNameToMatch,
                                   handle => {
                                       windowWasFound = true;
                                       return true;
                                   });

            It should_not_find_a_window = () => windowWasFound.ShouldBeFalse();
        }
       
        [Subject(typeof(WindowFinder), "FindWindowMatching")]
        public class when_window_with_the_process_is_not_specified_but_the_text_does_not_match_the_window_title : WindowFinderSpecs
        {
            protected static Regex matchAnyProcess;
            protected static bool windowWasFound;

            Establish context = () => {
                matchAnyProcess = new Regex(".*");
                _windowTextToMatch = new Regex("Wrong Title");
            };

            Because of = () => _sut.FindWindowMatching(
                                   _windowTextToMatch,
                                   matchAnyProcess,
                                   handle => {
                                       windowWasFound = true;
                                       return true;
                                   });

            It should_not_find_a_window = () => windowWasFound.ShouldBeFalse();
        }
    }
}