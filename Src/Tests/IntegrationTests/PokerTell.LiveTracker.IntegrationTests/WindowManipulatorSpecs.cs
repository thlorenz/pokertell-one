namespace PokerTell.LiveTracker.IntegrationTests
{
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    using Machine.Specifications;

    using Moq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Overlay;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class WindowManipulatorSpecs
    {
        protected static Window _pokerTableWindow;

        protected static IWindowManipulator _sut;

        Establish specContext = () => {
           _pokerTableWindow = new Window
            {
                Left = 0, 
                Top = 0, 
                Width = 100, 
                Height = 100, 
                AllowsTransparency = true, 
                Background = new SolidColorBrush { Color = Colors.Transparent }, 
                WindowStyle = WindowStyle.None, 
                ShowInTaskbar = false
            };
            _pokerTableWindow.Show();

            _sut = new WindowManipulator(new WindowInteropHelper(_pokerTableWindow).Handle);
        };

        Cleanup tearDown = () => _pokerTableWindow.Close();

        public abstract class Ctx_Window_Location_And_Size_Established : WindowManipulatorSpecs
        {
            Establish context = () => _sut.CheckWindowLocationAndSize(null);
        }

        [Subject(typeof(WindowManipulator), "Check Location and Size")]
        public class when_location_and_size_did_not_change : Ctx_Window_Location_And_Size_Established
        {
            static bool itCalledBack;

            Because of = () => _sut.CheckWindowLocationAndSize((loc, size) => itCalledBack = true);

            It should_not_call_back = () => itCalledBack.ShouldBeFalse();
        }

        [Subject(typeof(WindowManipulator), "Check Location and Size")]
        public class when_location_changed : Ctx_Window_Location_And_Size_Established
        {
            static bool itCalledBack;

            static Point calledBackWithPosition;
            static Point expectedPosition;

            const double newLeft = 11;
            const double newTop = 111;


            Establish context = () => {
                _pokerTableWindow.Left = newLeft;
                _pokerTableWindow.Top = newTop;
                 expectedPosition = new Point(newLeft, newTop); 
            };

            Because of = () => _sut.CheckWindowLocationAndSize((loc, size) => {
                itCalledBack = true;
                calledBackWithPosition = loc;
            });

            It should_call_back = () => itCalledBack.ShouldBeTrue();

            It should_call_back_with_the_new_position = () => calledBackWithPosition.ShouldEqual(expectedPosition);
        }

        [Subject(typeof(WindowManipulator), "Check Location and Size")]
        public class when_size_changed : Ctx_Window_Location_And_Size_Established
        {
            static bool itCalledBack;

            static Size calledBackWithSize;
            static Size expectedSize;

            const double newWidth = 11;
            const double newHeight = 111;


            Establish context = () => {
                _pokerTableWindow.Width = newWidth;
                _pokerTableWindow.Height = newHeight;
                 expectedSize = new Size(newWidth, newHeight); 
            };

            Because of = () => _sut.CheckWindowLocationAndSize((loc, size) => {
                itCalledBack = true;
                calledBackWithSize = size;
            });

            It should_call_back = () => itCalledBack.ShouldBeTrue();

            It should_call_back_with_the_new_size = () => calledBackWithSize.ShouldEqual(expectedSize);
        }

        [Subject(typeof(WindowManipulator), "GetInfo")]
        public class when_table_name_was_assigned_and_GetInfo_is_called : WindowManipulatorSpecs
        {
           const string classNameGivenByWindowInteropHelper = "HwndWrapper";

           const string tableName = "some Table Name";

           static string returnedInfo;

            Establish context = () => _pokerTableWindow.Title = tableName; 

            Because of = () => returnedInfo = _sut.GetInfo();

            It info_should_contain_window_class = () => returnedInfo.ShouldContain("Class: " + classNameGivenByWindowInteropHelper);

            It info_should_contain_table_name = () => returnedInfo.ShouldContain("Text: " + tableName);
        }

        [Subject(typeof(WindowManipulator), "GetText")]
        public class when_table_name_was_assigned_and_GetText_is_called : WindowManipulatorSpecs
        {
            const string tableName = "some Table Name";

            static string returnedText;

            Establish context = () => _pokerTableWindow.Title = tableName;

            Because of = () => returnedText = _sut.GetText();

            It text_should_equal_table_name = () => returnedText.ShouldEqual(tableName);
        }

        [Subject(typeof(WindowManipulator), "SetTextTo")]
        public class when_SetTextTo_is_called_with_null_as_subText_and_WindowText_is_MyWindow : WindowManipulatorSpecs
        {
            const string windowText = "MyWindow";

            static bool calledBackSubTextNotContained;

            static string returnedFullText;

            Establish context = () => _pokerTableWindow.Title = windowText;

            Because of = () => returnedFullText = _sut.SetTextTo(null, s => calledBackSubTextNotContained = true);

            It should_not_call_back_to_let_me_know_that_the_subtext_was_not_contained = () => calledBackSubTextNotContained.ShouldBeFalse();

            It should_return_the_current_window_text = () => returnedFullText.ShouldEqual(windowText);

            It should_not_change_the_window_text = () => _sut.GetText().ShouldEqual(windowText);
        }

        [Subject(typeof(WindowManipulator), "SetTextTo")]
        public class when_SetTextTo_is_called_with_a_subText_that_is_contained_in_the_window_text : WindowManipulatorSpecs
        {
            const string windowText = "MyWindow";

            const string subText = "Wind";

            static bool calledBackSubTextNotContained;

            static string returnedFullText;

            Establish context = () => _pokerTableWindow.Title = windowText;

            Because of = () => returnedFullText = _sut.SetTextTo(subText, s => calledBackSubTextNotContained = true);

            It should_not_call_back_to_let_me_know_that_the_subtext_was_not_contained = () => calledBackSubTextNotContained.ShouldBeFalse();

            It should_return_the_window_Text_as_it_was_before_the_call = () => returnedFullText.ShouldEqual(windowText);

            It should_set_the_window_text_to_the_sub_text = () => _sut.GetText().ShouldEqual(subText);
        }

        [Subject(typeof(WindowManipulator), "SetTextTo")]
        public class when_SetTextTo_is_called_with_a_subText_that_is_not_contained_in_the_window_text : WindowManipulatorSpecs
        {
            const string windowText = "New Table";

            const string subText = "Old Table";

            static bool calledBackSubTextNotContained;

            static string returnedFullText;

            Establish context = () => _pokerTableWindow.Title = windowText;

            Because of = () => returnedFullText = _sut.SetTextTo(subText, s => calledBackSubTextNotContained = true);

            It should_call_back_to_let_me_know_that_the_subtext_was_not_contained = () => calledBackSubTextNotContained.ShouldBeTrue();

            It should_return_the_window_Text_as_it_was_before_the_call = () => returnedFullText.ShouldEqual(windowText);
        }
    }
}