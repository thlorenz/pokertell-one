namespace PokerTell.LiveTracker.Views
{
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;

    using PokerTell.Infrastructure;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools;
    using Tools.WPF;
    using Tools.WPF.Interfaces;

    using MenuItem = System.Windows.Controls.MenuItem;

    public class LiveTrackerMenuItemFactory
    {
        readonly IGamesTracker _gamesTracker;

        readonly ILiveTrackerSettingsViewModel _liveTrackerSettings;

        readonly IWindowManager _liveTrackerSettingsWindow;

        ICommand _showLiveTrackerSettingsCommand;

        ICommand _trackHandHistoryCommand;

        public LiveTrackerMenuItemFactory(
            IWindowManager liveTrackerSettingsWindow, IGamesTracker gamesTracker, ILiveTrackerSettingsViewModel liveTrackerSettings)
        {
            _liveTrackerSettings = liveTrackerSettings;
            _liveTrackerSettingsWindow = liveTrackerSettingsWindow;
            _gamesTracker = gamesTracker;
        }

        public ICommand ShowLiveTrackerSettingsCommand
        {
            get
            {
                return _showLiveTrackerSettingsCommand ?? (_showLiveTrackerSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _liveTrackerSettingsWindow.ShowDialog()
                    });
            }
        }

        // TODO: FileDialog
        public ICommand TrackHandHistoryCommand
        {
            get
            {
                return _trackHandHistoryCommand ?? (_trackHandHistoryCommand = new SimpleCommand
                    {
                        ExecuteDelegate = TrackManuallySelectedHandHistory
                    });
            }
        }

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.LiveTracker_MenuItem_Title };
            menuItem.Items.Add(new MenuItem { Header = Resources.LiveTracker_MenuItem_TrackHandHistory, Command = TrackHandHistoryCommand });
            menuItem.Items.Add(new Separator());
            menuItem.Items.Add(new MenuItem { Header = Resources.LiveTracker_MenuItem_Settings, Command = ShowLiveTrackerSettingsCommand });
            return menuItem;
        }

        void TrackManuallySelectedHandHistory(object ignore)
        {
            var firstHandHistoryDirectoryFound = _liveTrackerSettings.HandHistoryFilesPaths.FirstOrDefault();
            var initialDirectory = firstHandHistoryDirectoryFound ?? Files.ProgramFilesFolder;

            var fileDialog = new OpenFileDialog
                {
                    InitialDirectory = initialDirectory, 
                    Filter = string.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", Resources.TrackHandHistoryDialog_Handhistories, "txt"), 
                    FilterIndex = 1, 
                    Title = Resources.TrackHandHistoryDialog_Title, 
                    FileName = Utils.FindNewestFileInDirectory(initialDirectory, "txt")
                };

            if (fileDialog.ShowDialog() == DialogResult.OK)
                _gamesTracker.StartTracking(fileDialog.FileName);
        }
    }
}