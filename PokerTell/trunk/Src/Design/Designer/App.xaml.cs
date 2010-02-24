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

        const string DesignerPath = @"C:\SD\PokerTell\Src\Design\Designer\";

        const string PokerStars_6max_Background = "PokerTables/PokerStars/PokerStars.6-max.jpg";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RunTableOverlaySettings();
           // RunColorPickerExpanderView();
        }

        static void RunTableOverlaySettings()
        {
            var img = new ImageSourceConverter().ConvertFromString(DesignerPath + PokerStars_6max_Background);

            var win = new TableOverlayView(TableOverlayDesign.Model) { Topmost = true, Background = new ImageBrush((ImageSource)img) { Stretch = Stretch.UniformToFill } };
            win.Show();
        }

        static void RunColorPickerExpanderView()
        {
            var win = new Window() { Topmost = true, Content = new ColorPickerExpander(), DataContext = new ColorViewModel("#FF00FF00") };
            win.Show();
        }
    }
}