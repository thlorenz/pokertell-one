namespace PokerTell.LiveTracker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using System.Xml.Linq;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;
    using Tools.Xml;

    public class LiveTrackerSettingsViewModel : NotifyPropertyChanged, ILiveTrackerSettingsViewModel
    {
        const string LiveTrackerSettings = "LiveTrackerSettings";

        const string AutoTrackElement = "AutoTrack";

        const string ShowHoleCardsDurationElement = "ShowHoleCardsDuration";

        const string ShowLiveStatsWindowOnStartupElement = "ShowLiveStatsWindowOnStartup";

        const string ShowTableOverlayElement = "ShowTableOverlay";

        const string HandHistoryFilesPathsElement = "HandHistoryFilesPaths";

        readonly ILiveTrackerSettingsXDocumentHandler _xDocumentHandler;

        public LiveTrackerSettingsViewModel(ILiveTrackerSettingsXDocumentHandler xDocumentHandler)
        {
            _xDocumentHandler = xDocumentHandler;
        }

        bool _autoTrack;

        public bool AutoTrack
        {
            get { return _autoTrack; }
            set
            {
                _autoTrack = value;
                RaisePropertyChanged(() => AutoTrack);
            }
        }

        bool _showTableOverlay;

        public bool ShowTableOverlay
        {
            get { return _showTableOverlay; }
            set
            {
                _showTableOverlay = value;
                RaisePropertyChanged(() => ShowTableOverlay);
            }
        }

        int _showHoleCardsDuration;

        public int ShowHoleCardsDuration
        {
            get { return _showHoleCardsDuration; }
            set
            {
                _showHoleCardsDuration = value;
                RaisePropertyChanged(() => ShowHoleCardsDuration);
            }
        }

        bool _showLiveStatsWindowOnStartup;

        public bool ShowLiveStatsWindowOnStartup
        {
            get { return _showLiveStatsWindowOnStartup; }
            set
            {
                _showLiveStatsWindowOnStartup = value;
                RaisePropertyChanged(() => ShowLiveStatsWindowOnStartup);
            }
        }

        IList<string> _handHistoryFilesPaths;

        public IList<string> HandHistoryFilesPaths
        {
            get { return _handHistoryFilesPaths; }
            set
            {
                _handHistoryFilesPaths = value;
                RaisePropertyChanged(() => HandHistoryFilesPaths);
            }
        }

        ICommand _saveSettingsCommand;

        public ICommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var xDoc = CreateXDocumentFor(this);
                            _xDocumentHandler.Save(xDoc);
                        }
                    });
            }
        }

        public static XDocument CreateXDocumentFor(ILiveTrackerSettingsViewModel lts)
        {
            return
                new XDocument(
                    new XElement(LiveTrackerSettings, 
                                 new XElement(AutoTrackElement, lts.AutoTrack), 
                                 new XElement(ShowHoleCardsDurationElement, lts.ShowHoleCardsDuration), 
                                 new XElement(ShowLiveStatsWindowOnStartupElement, lts.ShowLiveStatsWindowOnStartup), 
                                 new XElement(ShowTableOverlayElement, lts.ShowTableOverlay),
                                 Utils.XElementForCollection(HandHistoryFilesPathsElement, lts.HandHistoryFilesPaths)));
        }

        public void LoadSettings()
        {
            var xDoc = _xDocumentHandler.Load();

            if (xDoc == null)
            {
                SetPropertiesToDefault();
                return;
            }

            var xml = xDoc.Element(LiveTrackerSettings);

            if (xml == null)
            {
                SetPropertiesToDefault();
                return;
            }

            AutoTrack = Utils.GetBoolFrom(xml.Element(AutoTrackElement), true);
            ShowLiveStatsWindowOnStartup = Utils.GetBoolFrom(xml.Element(ShowLiveStatsWindowOnStartupElement), true);
            ShowTableOverlay = Utils.GetBoolFrom(xml.Element(ShowTableOverlayElement), true);
            ShowHoleCardsDuration = Utils.GetIntFrom(xml.Element(ShowHoleCardsDurationElement), 5);

            HandHistoryFilesPaths = 
                Utils.GetStringsFrom(xml.Element(HandHistoryFilesPathsElement), new List<string>())
                .Where(path => new DirectoryInfo(path).Exists)
                .ToList();
        }

        void SetPropertiesToDefault()
        {
            AutoTrack = true;
            ShowLiveStatsWindowOnStartup = true;
            ShowTableOverlay = true;
            ShowHoleCardsDuration = 5;

            HandHistoryFilesPaths = new List<string>();
        }
    }
}