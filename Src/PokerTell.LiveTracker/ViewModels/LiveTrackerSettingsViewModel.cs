namespace PokerTell.LiveTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Xml.Linq;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.LiveTracker.Events;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools.FunctionalCSharp;
    using Tools.IO;
    using Tools.WPF;
    using Tools.WPF.ViewModels;
    using Tools.Xml;

    public class LiveTrackerSettingsViewModel : NotifyPropertyChanged, ILiveTrackerSettingsViewModel
    {
        const string AutoTrackElement = "AutoTrack";

        const string HandHistoryFilesPathsElement = "HandHistoryFilesPaths";

        const string LiveTrackerSettings = "LiveTrackerSettings";

        const string ShowHoleCardsDurationElement = "ShowHoleCardsDuration";

        const string ShowLiveStatsWindowOnStartupElement = "ShowLiveStatsWindowOnStartup";

        const string ShowMyStatisticsElement = "ShowMyStatistics";

        const string ShowTableOverlayElement = "ShowTableOverlay";

        readonly IHandHistoryFolderAutoDetector _autoDetector;

        readonly IHandHistoryFolderAutoDetectResultsViewModel _autoDetectResultsViewModel;

        readonly IHandHistoryFolderAutoDetectResultsWindowManager _autoDetectResultsWindow;

        readonly IEventAggregator _eventAggregator;

        readonly ILiveTrackerSettingsXDocumentHandler _xDocumentHandler;

        ICommand _addHandHistoryPathCommand;

        ICommand _autoDetectHandHistoryFoldersCommand;

        bool _autoTrack;

        ICommand _browseCommand;

        IList<string> _handHistoryFilesPaths;

        string _handHistoryPathToBeAdded;

        ICommand _removeSelectedHandHistoryPathCommand;

        ICommand _saveSettingsCommand;

        int _showHoleCardsDuration;

        bool _showLiveStatsWindowOnStartup;

        bool _showMyStatistics;

        bool _showTableOverlay;

        readonly IPokerRoomInfoLocator _pokerRoomInfoLocator;

        public LiveTrackerSettingsViewModel(
            IEventAggregator eventAggregator, 
            ILiveTrackerSettingsXDocumentHandler xDocumentHandler, 
            IHandHistoryFolderAutoDetector autoDetector, 
            IHandHistoryFolderAutoDetectResultsViewModel autoDetectResultsViewModel, 
            IHandHistoryFolderAutoDetectResultsWindowManager autoDetectResultsWindow, 
            IPokerRoomInfoLocator pokerRoomInfoLocator)
        {
            _pokerRoomInfoLocator = pokerRoomInfoLocator;
            _eventAggregator = eventAggregator;
            _xDocumentHandler = xDocumentHandler;

            _autoDetector = autoDetector;
            _autoDetectResultsViewModel = autoDetectResultsViewModel;
            _autoDetectResultsWindow = autoDetectResultsWindow;

            ShowHoleCardsDurations = new List<int> { 0, 3, 5, 10, 15, 20 };
        }

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
                                HandHistoryFilesPaths.Add(HandHistoryPathToBeAdded.Trim().TrimEnd('\\'));

                            HandHistoryPathToBeAdded = null;
                        }, 
                        CanExecuteDelegate = arg => HandHistoryPathToBeAdded.IsExistingDirectory()
                    });
            }
        }

        public ICommand AutoDetectHandHistoryFoldersCommand
        {
            get
            {
                return _autoDetectHandHistoryFoldersCommand ?? (_autoDetectHandHistoryFoldersCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            DetectAndAddHandHistoryFolders();

                            _autoDetectResultsWindow.DataContext = _autoDetectResultsViewModel.InitializeWith(_autoDetector);
                            _autoDetectResultsWindow.ShowDialog();
                        }
                    });
            }
        }

        public bool AutoTrack
        {
            get { return _autoTrack; }
            set
            {
                _autoTrack = value;
                RaisePropertyChanged(() => AutoTrack);
            }
        }

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

        public IList<string> HandHistoryFilesPaths
        {
            get { return _handHistoryFilesPaths; }
            set
            {
                _handHistoryFilesPaths = value;
                RaisePropertyChanged(() => HandHistoryFilesPaths);
            }
        }

        public string HandHistoryPathToBeAdded
        {
            get { return _handHistoryPathToBeAdded; }
            set
            {
                _handHistoryPathToBeAdded = value;
                RaisePropertyChanged(() => HandHistoryPathToBeAdded);
            }
        }

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

        public ICommand SaveSettingsCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            var xDoc = CreateXDocumentFor(this);
                            _xDocumentHandler.Save(xDoc);
                            _eventAggregator
                                .GetEvent<LiveTrackerSettingsChangedEvent>()
                                .Publish(this);
                        }
                    });
            }
        }

        public string SelectedHandHistoryFilesPath { get; set; }

        public int ShowHoleCardsDuration
        {
            get { return _showHoleCardsDuration; }
            set
            {
                _showHoleCardsDuration = value;
                RaisePropertyChanged(() => ShowHoleCardsDuration);
            }
        }

        public IEnumerable<int> ShowHoleCardsDurations { get; protected set; }

        public bool ShowLiveStatsWindowOnStartup
        {
            get { return _showLiveStatsWindowOnStartup; }
            set
            {
                _showLiveStatsWindowOnStartup = value;
                RaisePropertyChanged(() => ShowLiveStatsWindowOnStartup);
            }
        }

        public bool ShowMyStatistics
        {
            get { return _showMyStatistics; }
            set
            {
                _showMyStatistics = value;
                RaisePropertyChanged(() => ShowMyStatistics);
            }
        }

        public bool ShowTableOverlay
        {
            get { return _showTableOverlay; }
            set
            {
                _showTableOverlay = value;
                RaisePropertyChanged(() => ShowTableOverlay);
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

        public void DetectAndAddHandHistoryFolders()
        {
            _autoDetector
                .InitializeWith(_pokerRoomInfoLocator.SupportedPokerRoomInfos)
                .Detect();

            _autoDetector.PokerRoomsWithDetectedHandHistoryDirectories.ForEach(pair => {
                if (! HandHistoryFilesPaths.Contains(pair.Second))
                    HandHistoryFilesPaths.Add(pair.Second);
            });
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

        void SetPropertiesToDefault()
        {
            AutoTrack = true;
            ShowLiveStatsWindowOnStartup = true;
            ShowTableOverlay = true;
            ShowMyStatistics = false;
            ShowHoleCardsDuration = 5;

            HandHistoryFilesPaths = new ObservableCollection<string>();
        }
    }
}