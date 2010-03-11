namespace PokerTell.LiveTracker.Views
{
    using System.Windows;

    using Infrastructure;

    using Interfaces;

    using Tools;

    /// <summary>
    /// Interaction logic for LiveTrackerSettingsView.xaml
    /// </summary>
    public partial class LiveTrackerSettingsView 
    {
        public LiveTrackerSettingsView()
        {
            InitializeComponent();

            if (Static.OperatingSystemIsWindowsXPOrOlder())
            {
                Background = ApplicationProperties.BorderedWindowBackgoundBrush;
                AllowsTransparency = false;
            }
        }

        public LiveTrackerSettingsView(ILiveTrackerSettingsViewModel viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}