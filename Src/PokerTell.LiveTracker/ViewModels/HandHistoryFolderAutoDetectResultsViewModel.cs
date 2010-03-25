namespace PokerTell.LiveTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using Interfaces;

    using Tools.Interfaces;
    using Tools.WPF.ViewModels;

    public class HandHistoryFolderAutoDetectResultsViewModel : NotifyPropertyChanged, IHandHistoryFolderAutoDetectResultsViewModel
    {
        public IEnumerable<string> PokerRoomsWithDetectedHandHistoryDirectories
        {
            get { return _handHistoryFolderAutoDetector.PokerRoomsWithDetectedHandHistoryDirectories.Select(room => room.First); }
        }

        public IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories
        {
            get { return _handHistoryFolderAutoDetector.PokerRoomsWithoutDetectedHandHistoryDirectories; }
        }

        ITuple<string, string> _selectedUndetectedPokerRoom;

        public ITuple<string, string> SelectedUndetectedPokerRoom
        {
            get { return _selectedUndetectedPokerRoom; }
            set
            {
                _selectedUndetectedPokerRoom = value;
                SelectedUndetectedPokerRoomInformation =
                   string.Format(Properties.Resources.AutoDetectHandHistoryFoldersResultsViewModel_RoomWhoseHandHistoryFolderWasNotDetectedInformation, _selectedUndetectedPokerRoom.First);
            }
        }

        string _selectedUndetectedPokerRoomInformation;

        readonly IHandHistoryFolderAutoDetector _handHistoryFolderAutoDetector;

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

        public HandHistoryFolderAutoDetectResultsViewModel(IHandHistoryFolderAutoDetector handHistoryFolderAutoDetector)
        {
            _handHistoryFolderAutoDetector = handHistoryFolderAutoDetector;

            if (PokerRoomsWithoutDetectedHandHistoryDirectories.Count > 0)
                SelectedUndetectedPokerRoom = PokerRoomsWithoutDetectedHandHistoryDirectories.First();
        }
    }
}