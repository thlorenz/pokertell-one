namespace PokerTell.LiveTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools.Interfaces;
    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class HandHistoryFolderAutoDetectResultsViewModel : NotifyPropertyChanged, IHandHistoryFolderAutoDetectResultsViewModel
    {
        IHandHistoryFolderAutoDetector _handHistoryFolderAutoDetector;

        ITuple<string, string> _selectedUndetectedPokerRoom;

        string _selectedUndetectedPokerRoomInformation;

        public IEnumerable<string> PokerRoomsWithDetectedHandHistoryDirectories
        {
            get { return _handHistoryFolderAutoDetector.PokerRoomsWithDetectedHandHistoryDirectories.Select(room => room.First); }
        }

        public IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories
        {
            get { return _handHistoryFolderAutoDetector.PokerRoomsWithoutDetectedHandHistoryDirectories; }
        }

        public ITuple<string, string> SelectedUndetectedPokerRoom
        {
            get { return _selectedUndetectedPokerRoom; }
            set
            {
                _selectedUndetectedPokerRoom = value;
                SelectedUndetectedPokerRoomInformation =
                    string.Format(Resources.AutoDetectHandHistoryFoldersResultsViewModel_RoomWhoseHandHistoryFolderWasNotDetectedInformation, 
                                  _selectedUndetectedPokerRoom.First);
            }
        }

        public string SelectedUndetectedPokerRoomInformation
        {
            get { return _selectedUndetectedPokerRoomInformation; }
            set
            {
                _selectedUndetectedPokerRoomInformation = value;
                RaisePropertyChanged(() => SelectedUndetectedPokerRoomInformation);
            }
        }

        public bool SomeDetectionsFailed
        {
            get { return PokerRoomsWithoutDetectedHandHistoryDirectories.Count > 0; }
        }

        public IHandHistoryFolderAutoDetectResultsViewModel InitializeWith(IHandHistoryFolderAutoDetector handHistoryFolderAutoDetector)
        {
            _handHistoryFolderAutoDetector = handHistoryFolderAutoDetector;

            if (PokerRoomsWithoutDetectedHandHistoryDirectories.Count > 0)
                SelectedUndetectedPokerRoom = PokerRoomsWithoutDetectedHandHistoryDirectories.First();

            return this;
        }

        ICommand _browseToSelectedUndetectedPokerRoomHelpCommand;

        public ICommand BrowseToSelectedUndetectedPokerRoomHelpCommand
        {
            get
            {
                return _browseToSelectedUndetectedPokerRoomHelpCommand ?? (_browseToSelectedUndetectedPokerRoomHelpCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => Process.Start(SelectedUndetectedPokerRoom.Second),
                    });
            }
        }

        ICommand _browseToSupportedPokerRoomsListCommand;

        public ICommand BrowseToSupportedPokerRoomsListCommand
        {
            get
            {
                return _browseToSupportedPokerRoomsListCommand ?? (_browseToSupportedPokerRoomsListCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => Process.Start(Infrastructure.Properties.Resources.Links_ListOfSupportedPokerRooms),
                    });
            }
        }

    }
}