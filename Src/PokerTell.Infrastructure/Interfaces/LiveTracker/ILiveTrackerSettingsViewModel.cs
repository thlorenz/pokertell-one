namespace PokerTell.Infrastructure.Interfaces.LiveTracker
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

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

        ICommand AutoDetectHandHistoryFoldersCommand { get; }

        ICommand DetectPreferredSeatsCommand { get; }

        ILiveTrackerSettingsViewModel LoadSettings();

        ILiveTrackerSettingsViewModel DetectAndAddHandHistoryFolders();

        IEnumerable<string> DetectAndSavePreferredSeats();
    }
}