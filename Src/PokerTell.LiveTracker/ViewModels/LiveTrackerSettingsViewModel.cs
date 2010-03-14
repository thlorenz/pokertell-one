namespace PokerTell.LiveTracker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Xml.Linq;

    using Infrastructure.Events;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.LiveTracker.Interfaces;

    using Properties;

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

        const string ShowMyStatisticsElement = "ShowMyStatistics";

        readonly ILiveTrackerSettingsXDocumentHandler _xDocumentHandler;

        public LiveTrackerSettingsViewModel(IEventAggregator eventAggregator, ILiveTrackerSettingsXDocumentHandler xDocumentHandler)
        {
            _eventAggregator = eventAggregator;
            _xDocumentHandler = xDocumentHandler;
            ShowHoleCardsDurations = new List<int> { 0, 3, 5, 10, 15, 20 };
        }

        public IEnumerable<int> ShowHoleCardsDurations { get; protected set; }

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

        bool _showMyStatistics;

        public bool ShowMyStatistics
        {
            get { return _showMyStatistics; }
            set
            {
                _showMyStatistics = value;
                RaisePropertyChanged(() => ShowMyStatistics);
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

        public string SelectedHandHistoryFilesPath { get; set; }

        string _handHistoryPathToBeAdded;

        public string HandHistoryPathToBeAdded
        {
            get { return _handHistoryPathToBeAdded; }
            set
            {
                _handHistoryPathToBeAdded = value;
                RaisePropertyChanged(() => HandHistoryPathToBeAdded);
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

        ICommand _removeSelectedHandHistoryPathCommand;

        public ICommand RemoveSelectedHandHistoryPathCommand
        {
            get
            {
                return _removeSelectedHandHistoryPathCommand ?? (_removeSelectedHandHistoryPathCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            HandHistoryFilesPaths.Remove(SelectedHandHistoryFilesPath);
                            SelectedHandHistoryFilesPath = null;
                        },
                        CanExecuteDelegate = arg => SelectedHandHistoryFilesPath != null
                    });
            }
        }

        ICommand _addHandHistoryPathCommand;

        public ICommand AddHandHistoryPathCommand
        {
            get
            {
                return _addHandHistoryPathCommand ?? (_addHandHistoryPathCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            if (HandHistoryFilesPaths.Contains(HandHistoryPathToBeAdded))
                            {
                                _eventAggregator
                                    .GetEvent<UserMessageEvent>()
                                    .Publish(new UserMessageEventArgs(UserMessageTypes.Warning, Resources.Warning_HandHistoryFolderIsTrackedAlready));
                            }
                            else
                                HandHistoryFilesPaths.Add(HandHistoryPathToBeAdded.TrimStart(' ').TrimEnd(' '));

                            HandHistoryPathToBeAdded = null;
                        },
                        CanExecuteDelegate = arg => {
                            if (string.IsNullOrEmpty(HandHistoryPathToBeAdded))
                                    return false;
                                try
                                {
                                    return Directory.Exists(HandHistoryPathToBeAdded);
                                }
                                catch 
                                {
                                    return false;
                                } 
                            } 
                    });
            }
        }

        ICommand _browseCommand;

        readonly IEventAggregator _eventAggregator;

        public ICommand BrowseCommand
        {
            get
            {
                return _browseCommand ?? (_browseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = BrowseForDirectory
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
                                 new XElement(ShowMyStatisticsElement, lts.ShowMyStatistics),
                                 Utils.XElementForCollection(HandHistoryFilesPathsElement, lts.HandHistoryFilesPaths)));
        }

        public ILiveTrackerSettingsViewModel LoadSettings()
        {
            var xDoc = _xDocumentHandler.Load();

            if (xDoc == null)
            {
                SetPropertiesToDefault();
                return this;
            }

            var xml = xDoc.Element(LiveTrackerSettings);

            if (xml == null)
            {
                SetPropertiesToDefault();
                return this;
            }

            AutoTrack = Utils.GetBoolFrom(xml.Element(AutoTrackElement), true);
            ShowLiveStatsWindowOnStartup = Utils.GetBoolFrom(xml.Element(ShowLiveStatsWindowOnStartupElement), true);
            ShowTableOverlay = Utils.GetBoolFrom(xml.Element(ShowTableOverlayElement), true);
            ShowMyStatistics = Utils.GetBoolFrom(xml.Element(ShowMyStatisticsElement), false);
            ShowHoleCardsDuration = Utils.GetIntFrom(xml.Element(ShowHoleCardsDurationElement), 5);

            HandHistoryFilesPaths = new ObservableCollection<string>( 
                Utils.GetStringsFrom(xml.Element(HandHistoryFilesPathsElement), new List<string>())
                .Where(path => new DirectoryInfo(path).Exists));

            return this;
        }

        void SetPropertiesToDefault()
        {
            AutoTrack = true;
            ShowLiveStatsWindowOnStartup = true;
            ShowTableOverlay = true;
            ShowMyStatistics = false;
            ShowHoleCardsDuration = 5;

            HandHistoryFilesPaths = new ObservableCollection<string>();
        }

        void BrowseForDirectory(object arg)
        {
            using (var browserDialog = new FolderBrowserDialog
                {
                    SelectedPath = HandHistoryPathToBeAdded, 
                    ShowNewFolderButton = false
                })
            {
                browserDialog.ShowDialog();
                HandHistoryPathToBeAdded = browserDialog.SelectedPath;
            }
        }
    }
}