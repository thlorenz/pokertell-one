namespace PokerTell.Infrastructure.PokerRooms
{
    using Tools.FunctionalCSharp;

    public static class PokerStarsInfo
    {
        public const string Site = "PokerStars";

        public const string TableClass = "PokerTableClass";

        public const string ProcessName = "PokerStars";

        public const string FileExtension = "txt";
    }

    public static class FullTiltPokerInfo
    {
        public const string Site = "Full Tilt Poker";

        public const string TableClass = "NotImplemented";

        public const string ProcessName = "FullTilt";

        public const string FileExtension = "txt";
    }

    public static class PokerRoomInfoUtility
    {
        public static string GetProcessNameFor(string pokerSite)
        {
            return pokerSite.ToLower().Match()
                .With(s => s == PokerStarsInfo.Site.ToLower(), _ => PokerStarsInfo.ProcessName)
                .With(s => s == FullTiltPokerInfo.Site.ToLower(), _ => FullTiltPokerInfo.ProcessName)
                .Do();
        }
    }
}