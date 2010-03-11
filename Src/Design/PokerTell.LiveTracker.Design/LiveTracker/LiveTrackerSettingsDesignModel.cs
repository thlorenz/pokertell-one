namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Microsoft.Practices.Composite.Events;

    using Persistence;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels;

    public class LiveTrackerSettingsDesignModel : LiveTrackerSettingsViewModel
    {
        public LiveTrackerSettingsDesignModel(ILiveTrackerSettingsXDocumentHandler xDocumentHandler)
            : base(new EventAggregator(), xDocumentHandler)
        {
            AutoTrack = true;
            ShowLiveStatsWindowOnStartup = false;
            ShowTableOverlay = true;
            ShowMyStatistics = true;
            ShowHoleCardsDuration = 5;

            HandHistoryFilesPaths = new ObservableCollection<string>
                {
                    @"C:\Program Files\PokerStars\HandHistory", 
                    @"C:\Program Files\Full Tilt Poker\HandHistory", 
                    @"C:\Documents and Settings\Owner.LapThor\Application Data\PokerStars\HandHistory", 
                };
        }
    }

    public static class LiveTrackerSettingsDesign
    {
        public static LiveTrackerSettingsViewModel Model
        {
            get { return new LiveTrackerSettingsDesignModel(new LiveTrackerSettingsXDocumentHandler()); }
        }  
    }
}