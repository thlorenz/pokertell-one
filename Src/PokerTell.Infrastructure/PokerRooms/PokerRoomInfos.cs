namespace PokerTell.Infrastructure.PokerRooms
{
    using System;

    using Tools.FunctionalCSharp;

    public interface IPokerRoomInfo
    {
        string Site { get; }

        string TableClass { get; }

        string ProcessName { get; }

        string FileExtension { get; }
    }

    public class PokerStarsInfo : IPokerRoomInfo
    {
        public string Site
        {
            get { return "PokerStars"; }
        }

        public string TableClass
        {
            get { return "PokerTableClass"; }
        }

        public string ProcessName
        {
            get { return "PokerStars"; }
        }

        public string FileExtension
        {
            get { return "txt"; }
        }
    }

    public class FullTiltPokerInfo : IPokerRoomInfo
    {
        public string Site
        {
            get { return "Full Tilt Poker"; }
        }

        public string TableClass
        {
            get { return "NotImplemented"; }
        }

        public string ProcessName
        {
            get { return "FullTilt"; }
        }

        public string FileExtension
        {
            get { return "txt"; }
        }
    }

    public class PokerRoomInfoStub : IPokerRoomInfo
    {
        public string Site
        {
            get { return string.Empty; }
        }

        public string TableClass
        {
            get { throw new NotImplementedException(); }
        }

        public string ProcessName
        {
            get { throw new NotImplementedException(); }
        }

        public string FileExtension
        {
            get { throw new NotImplementedException(); }
        }
    }

    public static class PokerRoomInfoUtility
    {
        public static IPokerRoomInfo GetPokerRoomInfoFor(string pokerSite)
        {
            return pokerSite.ToLower().Match()
                .With<IPokerRoomInfo>(s => s == new PokerStarsInfo().Site.ToLower(), _ => new PokerStarsInfo())
                .With(s => s == new FullTiltPokerInfo().Site.ToLower(), _ => new FullTiltPokerInfo())
                .Else(s => new PokerRoomInfoStub()) // needed for testing purposes
                .Do();
        }
    }
}