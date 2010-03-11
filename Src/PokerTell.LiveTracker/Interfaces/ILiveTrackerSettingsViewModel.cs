namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    using Tools.Interfaces;

    public interface ILiveTrackerSettingsViewModel : IFluentInterface, INotifyPropertyChanged
    {
        bool AutoTrack { get; }

        bool ShowTableOverlay { get; }

        int ShowHoleCardsDuration { get; }

        bool ShowLiveStatsWindowOnStartup { get; }

        IList<string> HandHistoryFilesPaths { get; set; }

        ICommand SaveSettingsCommand { get; }

        bool ShowMyStatistics { get; set; }

        ICommand RemoveSelectedHandHistoryPathCommand { get; }

        string SelectedHandHistoryFilesPath { get; set; }

        string HandHistoryPathToBeAdded { get; set; }

        ICommand AddHandHistoryPathCommand { get; }

        ICommand BrowseCommand { get; }

        void LoadSettings();
    }
}