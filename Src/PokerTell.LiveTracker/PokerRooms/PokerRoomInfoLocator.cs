namespace PokerTell.LiveTracker.PokerRooms
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public class PokerRoomInfoLocator : IPokerRoomInfoLocator
    {
        public IPokerRoomInfo GetPokerRoomInfoFor(string pokerSite)
        {
            return pokerSite.ToLower().Match()
                .With<IPokerRoomInfo>(s => s == new PokerStarsInfo().Site.ToLower(), _ => new PokerStarsInfo())
                .With(s => s == new FullTiltPokerInfo().Site.ToLower(), _ => new FullTiltPokerInfo())
                .Do();
        }

        public IEnumerable<IPokerRoomInfo> SupportedPokerRoomInfos
        {
            get
            {
                yield return new PokerStarsInfo();
                yield return new FullTiltPokerInfo();
            }
        }
    }
}