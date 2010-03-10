namespace PokerTell.LiveTracker.Design.LiveTracker
{
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.ViewModels;

    public class LiveTrackerSettingsDesignModel : LiveTrackerSettingsViewModel
    {
        public LiveTrackerSettingsDesignModel(ILiveTrackerSettingsXDocumentHandler xDocumentHandler)
            : base(xDocumentHandler)
        {
            AutoTrack = true;
            ShowLiveStatsWindowOnStartup = false;
            ShowTableOverlay = true;

            HandHistoryFilesPaths = new[]
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
            get { return new LiveTrackerSettingsDesignModel(null); }
        }  
    }
}