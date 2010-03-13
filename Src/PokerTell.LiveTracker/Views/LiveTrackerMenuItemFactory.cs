namespace PokerTell.LiveTracker.Views
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Properties;

    using Tools.WPF;
    using Tools.WPF.Interfaces;

    public class LiveTrackerMenuItemFactory
    {
        readonly IWindowManager _liveTrackerSettingsWindow;

        public LiveTrackerMenuItemFactory(IWindowManager liveTrackerSettingsWindow)
        {
            _liveTrackerSettingsWindow = liveTrackerSettingsWindow;
        }

        public MenuItem Create()
        {
            var menuItem = new MenuItem { Header = Resources.LiveTracker_MenuItem_Title };
            menuItem.Items.Add(new MenuItem { Header = Resources.LiveTracker_MenuItem_TrackHandHistory /*TODO Command*/ });
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
    }
}