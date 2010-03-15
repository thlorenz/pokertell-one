namespace PokerTell.LiveTracker.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Interfaces;

    using Properties;

    using Tools.WPF;
    using Tools.WPF.Interfaces;

    public class LiveTrackerMenuItemFactory
    {
        readonly IWindowManager _liveTrackerSettingsWindow;

        readonly IGamesTracker _gamesTracker;

        public LiveTrackerMenuItemFactory(IWindowManager liveTrackerSettingsWindow, IGamesTracker gamesTracker)
        {
            _liveTrackerSettingsWindow = liveTrackerSettingsWindow;
            _gamesTracker = gamesTracker;
        }

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.LiveTracker_MenuItem_Title };
            menuItem.Items.Add(new MenuItem { Header = Resources.LiveTracker_MenuItem_TrackHandHistory, Command = TrackHandHistoryCommand });
            menuItem.Items.Add(new Separator());
            menuItem.Items.Add(new MenuItem { Header = Resources.LiveTracker_MenuItem_Settings, Command = ShowLiveTrackerSettingsCommand });
            return menuItem;
        }

        ICommand _showLiveTrackerSettingsCommand;

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

        ICommand _trackHandHistoryCommand;


        public ICommand TrackHandHistoryCommand
        {
            get
            {
                return _trackHandHistoryCommand ?? (_trackHandHistoryCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => _gamesTracker.StartTracking(@"C:\Documents and Settings\Owner.LapThor\Desktop\hh\history.txt")
                    });
            }
        }
    }
}