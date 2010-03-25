namespace PokerTell.LiveTracker.PokerRooms
{
    using System.Collections.Generic;

    using PokerTell.LiveTracker.Interfaces;

    using Tools.FunctionalCSharp;
    using Tools.Interfaces;

    public class HandHistoryFolderAutoDetector : IHandHistoryFolderAutoDetector
    {
        IEnumerable<IPokerRoomInfo> _pokerRoomInfos;

        public IList<ITuple<string, string>> PokerRoomsWithDetectedHandHistoryDirectories { get; protected set; }

        public IList<ITuple<string, string>> PokerRoomsWithoutDetectedHandHistoryDirectories { get; protected set; }

        public IHandHistoryFolderAutoDetector InitializeWith(IEnumerable<IPokerRoomInfo> pokerRoomInfos)
        {
            _pokerRoomInfos = pokerRoomInfos;

            return this;
        }

        public IHandHistoryFolderAutoDetector Detect()
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
    }
}