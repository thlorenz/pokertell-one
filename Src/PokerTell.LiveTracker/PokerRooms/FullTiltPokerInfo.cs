namespace PokerTell.LiveTracker.PokerRooms
{
    using System;

    using Infrastructure;

    using Interfaces;

    public class FullTiltPokerInfo : IPokerRoomInfo
    {
        public string Site
        {
            get { return PokerSites.FullTiltPoker; }
        }

        public string TableClass
        {
            get { return "NotImplemented"; }
        }

        public string ProcessName
        {
            get { return "FullTiltPoker"; }
        }

        public string FileExtension
        {
            get { return "txt"; }
        }

        public IPokerRoomDetective Detective
        {
            get { return new FullTiltPokerDetective(); }
        }

        public string HelpWithHandHistoryDirectorySetupLink
        {
            get { return @"http:\\www.GetPokerTell.com\Help"; }
        }

        public string TableNameFoundInPokerTableTitleFrom(string parsedName)
        {
            return parsedName;
        }
    }
}