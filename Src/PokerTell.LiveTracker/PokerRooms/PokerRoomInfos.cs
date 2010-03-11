namespace PokerTell.LiveTracker.PokerRooms
{
    using System;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public interface IPokerRoomInfoLocator
    {
        IPokerRoomInfo GetPokerRoomInfoFor(string pokerSite);
    }

    public class PokerRoomInfoLocator : IPokerRoomInfoLocator
    {
        public IPokerRoomInfo GetPokerRoomInfoFor(string pokerSite)
        {
            return pokerSite.ToLower().Match()
                .With<IPokerRoomInfo>(s => s == new PokerStarsInfo().Site.ToLower(), _ => new PokerStarsInfo())
                .With(s => s == new FullTiltPokerInfo().Site.ToLower(), _ => new FullTiltPokerInfo())
                .Do();
        }
    }
}