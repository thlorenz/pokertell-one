namespace Designer
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Moq;

    using PokerTell.LiveTracker.Design.LiveTracker;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Persistence;
    using PokerTell.LiveTracker.Tracking;
    using PokerTell.LiveTracker.Views;
    using PokerTell.LiveTracker.Views.Overlay;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    // Resharper disable InconsistentNaming
    public partial class App : Application
    {
        const string DesignerPath = @"C:\SD\PokerTell\Src\Design\Designer\";

        const string PokerStars_6max_Background = "PokerTables/PokerStars/PokerStars.6-max.jpg";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // RunTableOverlaySettings();
            // RunColorPickerExpanderView();
            // RunOverlayToTableAttacher();
            
           // RunHandHistoryFilesWatcher();
           // RunLiveTrackerSettingsWindow();
        }

        static void RunTableOverlaySettings()
        {
            var img = new ImageSourceConverter().ConvertFromString(DesignerPath + PokerStars_6max_Background);
            var viewModel = TableOverlayDesign.Model;
            var controllerViewModel = new TableOverlayControllerViewModel(viewModel);
            var win = new TableOverlayView(viewModel)
                { Topmost = true, Background = new ImageBrush((ImageSource)img) { Stretch = Stretch.UniformToFill } };
            win.Show();

            // var ctrlWin = new TableOverlayControllerWindow(controllerViewModel) { Topmost = true };
            // ctrlWin.Show();
        }

        static void RunOverlayToTableAttacher()
        {
            var pokerTable = new Window { Title = "The PokerTable" };
            var overlayWindowManager = new WindowManager(() => new Window {
                    AllowsTransparency = true,
                    Background = new SolidColorBrush { Color = Colors.Transparent },
                    WindowStyle = WindowStyle.None,
                    ShowInTaskbar = false,
                    Content = new Button { Content = "OverlayWindow", Margin = new Thickness(50) }
                });

            var watchTableTimer = new DispatcherTimerAdapter { Interval = TimeSpan.FromMilliseconds(100) };
            var waitThenTryToFindTableAgainTimer = new DispatcherTimerAdapter { Interval = TimeSpan.FromMilliseconds(3000) };

            var attacher =
                AutoWirerForTableAttacher
                    .ConfigureTableAttacherDependencies()
                    .Resolve<IOverlayToTableAttacher>();

            attacher.TableClosed += attacher.Dispose;

            pokerTable.Show();
            overlayWindowManager.Show();

            var pokerRoomInfo_Stub = new Mock<IPokerRoomInfo>();
            pokerRoomInfo_Stub
                .SetupGet(i => i.ProcessName).Returns(string.Empty);
            pokerRoomInfo_Stub
                .Setup(i => i.TableNameFoundInPokerTableTitleFrom(pokerTable.Title)).Returns(pokerTable.Title);

                attacher
                    .InitializeWith(overlayWindowManager, watchTableTimer, waitThenTryToFindTableAgainTimer, pokerRoomInfo_Stub.Object, pokerTable.Title)
                .Activate();
        }

        static void RunColorPickerExpanderView()
        {
            var win = new Window { Topmost = true, Content = new ColorPickerExpander(), DataContext = new ColorViewModel("#FF00FF00") };
            win.Show();
        }

        static void RunHandHistoryFilesWatcher()
        {
            const string path = @"C:\Program Files\PokerStars\HandHistory";
            var watcher = new HandHistoryFilesWatcher()
                .InitializeWith(path);
            watcher.IncludeSubdirectories = true;
            watcher.Changed += (s, e) => Console.WriteLine("Changed:\nType: {0}\nName: {1}\nFullPath: {2}", e.ChangeType, e.Name, e.FullPath);
        }

        public void RunLiveTrackerSettingsWindow()
        {
            var settings = new LiveTrackerSettingsDesignModel(new LiveTrackerSettingsXDocumentHandler());
            var win = new LiveTrackerSettingsView(settings) { Topmost = true };
            win.Show();
        }
    }
}