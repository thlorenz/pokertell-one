namespace PokerTell.LiveTracker.Interfaces
{
    using System.Collections.Generic;

    using Tools.Interfaces;

    public interface IPokerRoomInfoLocator : IFluentInterface
    {
        IPokerRoomInfo GetPokerRoomInfoFor(string pokerSite);

        IEnumerable<IPokerRoomInfo> SupportedPokerRoomInfos { get; }
    }
}