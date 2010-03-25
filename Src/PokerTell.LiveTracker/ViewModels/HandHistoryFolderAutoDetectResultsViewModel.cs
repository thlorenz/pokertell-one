namespace PokerTell.LiveTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.Properties;

    using Tools.Interfaces;
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
    }
}