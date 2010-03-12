namespace PokerTell.LiveTracker.PokerRooms
{
    using System.Text.RegularExpressions;

    using Infrastructure;

    using Interfaces;

    public class PokerStarsInfo : IPokerRoomInfo
    {
        public string Site
        {
            get { return PokerSites.PokerStars; }
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

        public string TableNameFoundInPokerTableTitleFrom(string parsedName)
        {
            const string patTournamentTableName = @"(?<TournamentId>\d+) (?<TableNumber>\d+)";

            var match = new Regex(patTournamentTableName).Match(parsedName);
            if (match.Success)
            {
                return string.Format("{0} Table {1} ", match.Groups["TournamentId"].Value, match.Groups["TableNumber"]);
            }

            return parsedName;
        }
    }
}