namespace PokerTell.LiveTracker.PokerRooms
{
    using System.Collections.Generic;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class PokerRoomSettingsDetector : IPokerRoomSettingsDetector
    {
        IEnumerable<IPokerRoomInfo> _pokerRoomInfos;

        public IList<ITuple<string, string>> PokerRoomsWithDetectedHandHistoryDirectories { get; protected set; }

        public IList<ITuple<string, IDictionary<int, int>>> PokerRoomsWithDetectedPreferredSeats { get; protected set; }

        public IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories { get; protected set; }

        public IPokerRoomSettingsDetector InitializeWith(IEnumerable<IPokerRoomInfo> pokerRoomInfos)
        {
            _pokerRoomInfos = pokerRoomInfos;

            return this;
        }

        public IPokerRoomSettingsDetector DetectHandHistoryFolders()
        {
            PokerRoomsWithDetectedHandHistoryDirectories = new List<ITuple<string, string>>();
            PokerRoomsWithoutDetectedHandHistoryDirectories = new List<ITuple<string, string>>();

            _pokerRoomInfos.ForEach(info => {
                var detective = info.Detective.Investigate();

                if (detective.PokerRoomIsInstalled)
                {
                    if (detective.DetectedHandHistoryDirectory)
                        PokerRoomsWithDetectedHandHistoryDirectories.Add(Tuple.New(info.Site, detective.HandHistoryDirectory.Trim().TrimEnd('\\')));
                    else
                        PokerRoomsWithoutDetectedHandHistoryDirectories.Add(Tuple.New(info.Site, info.HelpWithHandHistoryDirectorySetupLink));
                }
            });

            return this;
        }

        public IPokerRoomSettingsDetector DetectPreferredSeats()
        {
            PokerRoomsWithDetectedPreferredSeats = new List<ITuple<string, IDictionary<int, int>>>();

            _pokerRoomInfos.ForEach(info => {
                var detective = info.Detective;
                
                if (detective.PokerRoomSavesPreferredSeats)
                {
                    detective.Investigate();

                    if (detective.PokerRoomIsInstalled && detective.DetectedPreferredSeats)
                        PokerRoomsWithDetectedPreferredSeats.Add(Tuple.New(info.Site, detective.PreferredSeats));
                }
            });
            return this;
        }
    }
}