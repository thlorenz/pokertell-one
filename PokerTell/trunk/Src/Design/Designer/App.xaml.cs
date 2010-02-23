namespace Designer
{
    using System.Windows;
    using System.Windows.Media;

    using PokerTell.LiveTracker.Design.LiveTracker;
    using PokerTell.LiveTracker.Views.Overlay;

    using Tools.WPF.Controls;
    using Tools.WPF.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RunTableOverlaySettings();
           // RunColorPickerExpanderView();
        }

        static void RunTableOverlaySettings()
        {
            var win = new TableOverlayView(TableOverlayDesign.Model) { Topmost = true, Background = Brushes.DarkRed };
            win.Show();
        }

        static void RunColorPickerExpanderView()
        {
            var win = new Window() { Topmost = true, Content = new ColorPickerExpander(), DataContext = new ColorViewModel("#FF00FF00") };
            win.Show();
        }
    }
}